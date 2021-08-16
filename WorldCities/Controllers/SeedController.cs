using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorldCities.Data;
using OfficeOpenXml;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using WorldCities.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security;

namespace WorldCities.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SeedController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet]
        public async Task<ActionResult> Import()
        {
            // Prevent non-dev environments from running this method
            if (_env.IsDevelopment())
            {
                throw new SecurityException("Not Allowed");
            }
            var path = Path.Combine(_env.ContentRootPath, "Data/Source/worldcities.xlsx");
            using var stream = System.IO.File.OpenRead(path);
            using var excelPackage = new ExcelPackage(stream);
            //Get the first work sheet
            var worksheet = excelPackage.Workbook.Worksheets[0];
            // Define how many rows we want to process
            var nEndRow = worksheet.Dimension.End.Row;
            //Initialize record counters
            var numberOfCountriesAdded = 0;
            var numberOfCitiesAdded = 0;
            // Create a lookup dictionary containing all the countries already existing into the database (It will be empty on the first run)
            var countriesByName = _context.Countries
            .AsNoTracking()
            .ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);
            //Iterate through all rows, skipping the first one
            for (int nRow = 2; nRow < nEndRow; nRow ++)
            {
                var row = worksheet.Cells[nRow, 1, nRow, worksheet.Dimension.End.Column];
                var countryName = row[nRow, 5].GetValue<string>();
                var iso2 = row[nRow, 6].GetValue<string>();
                var iso3 = row[nRow, 7].GetValue<string>();
                //Skip country if it already exists in the DB
                if (countriesByName.ContainsKey(countryName))
                {
                    continue;
                }
                //Create country and fill it with xlsx data
                var country = new Country
                {
                    Name = countryName,
                    ISO2 = iso2,
                    ISO3 = iso3
                };

                //Add the new country to the DB Context
                await _context.Countries.AddAsync(country);
                //Store the country in our lookup to retrieve its ID later
                countriesByName.Add(countryName, country);
                //Increment the counter
                numberOfCitiesAdded++;
            }

            //Save all the countries in the DB
            if (numberOfCountriesAdded > 0)
            {
                await _context.SaveChangesAsync();


            }
            //Create a lookup dictionary
            //containing all the cities already existing
            //into the DB (it will be empty on the first run)
            var cities = _context.Cities
            .AsNoTracking()
            .ToDictionary(x => (
                Name: x.Name,
                Lat: x.Lat,
                Lon: x.Lon,
                CountryId: x.CountryId
            ));
            //Iterate through all rows, skipping the first one
            for (int nRow = 2; nRow <= nEndRow; nRow++)
            {
                var row = worksheet.Cells[
                    nRow, 1, nRow, worksheet.Dimension.End.Column
                ];
                var name = row[nRow, 1].GetValue<string>();
                var nameAscii = row[nRow, 2].GetValue<string>();
                var lat = row[nRow,3].GetValue<decimal>();
                var lon = row[nRow, 4].GetValue<decimal>();
                var countryName = row[nRow, 5].GetValue<string>();
                //retrieve the coutnry ID by country name
                var countryId = countriesByName[countryName].Id;
                //Skip this city if it already exists in the DB
                var cityExists = cities.ContainsKey((
                    Name: name,
                    Lat: lat,
                    Lon: lon,
                    CountryId: countryId
                ));

                if (cityExists)
                {
                    continue;
                }
                //Create the city entity fill it with xlsx data
                var city = new City
                {
                    Name = name,
                    Name_ASCII = nameAscii,
                    Lat = lat,
                    Lon = lon,
                    CountryId = countryId
                };

                //Add the new city to the DB Context
                _context.Cities.Add(city);
                numberOfCountriesAdded++;
            }
            //Save all the cities into the DB
            if (numberOfCountriesAdded > 0)
            {
                await _context.SaveChangesAsync();
            }
            return new JsonResult(
                new {
                    Cities = numberOfCitiesAdded,
                    Countries = numberOfCountriesAdded
                }
            );
        }
    }
}