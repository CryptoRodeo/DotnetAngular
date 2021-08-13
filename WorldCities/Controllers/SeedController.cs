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
            // Todo - > Actually implement this
        }
        
    }
}