using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace WorldCities.Data.Models
{
public class City
    {
        public City()
        {
        }
        /// <summary>
        /// The unique id and primary key for this City
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// City name (in UTF8 format)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// City name (in ASCII format)
        /// </summary>
        public string Name_ASCII { get; set; }
        /// <summary>
        /// City latitude
        /// </summary>
        public decimal Lat { get; set; }
        /// <summary>
        /// City longitude
        /// </summary>
        public decimal Lon { get; set; }
        /// <summary>
        /// Country Id (foreign key)
        /// </summary>
		[ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
		// Navigational property
		public virtual Country Country {get; set;}
    }

}