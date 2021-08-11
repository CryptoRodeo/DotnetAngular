using Microsoft.EntityFrameworkCore;
using WorldCities.Data.Models;

namespace WorldCities.Data {

	public class ApplicationDbContext : DbContext 
	{

		public DbSet<City> Cities { get; set;}
		public DbSet<Country> Countries { get; set; }

		public ApplicationDbContext() : base() 
		{
		}
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}	

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			//Add the EntityTypeConfiguration classes in one line
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
		}
	}
}