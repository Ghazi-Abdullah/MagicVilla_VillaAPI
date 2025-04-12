using MagicVilla_VillaAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //We need to connect the applicationDbContext with connection string thae way it know where is the database  
        //and we need add that to dependency injection in the Program.cs
        public DbSet<Villa> Villas { get; set; }

        //We need to override for the migration 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Name = "Royal villa",
                    Details = "This is a royal villa",
                    ImageUrl= "https://www.google.com",
                    Occupancy = 4,
                    Rate = 1000,
                    Sqft = 2000,
                    Amenity = "",
                    CreatedDate = DateTime.Now,

                },
                 new Villa()
                 {
                     Id = 2,
                     Name = "Diamond villa",
                     Details = "This is a Diamond villa",
                     ImageUrl = "https://www.google.com",
                     Occupancy = 4,
                     Rate = 5000,
                     Sqft = 7000,
                     Amenity = "",
                     CreatedDate = DateTime.Now,

                 },
                  new Villa()
                  {
                      Id = 3,
                      Name = "Diamond pool villa",
                      Details = "This is a royal villa",
                      ImageUrl = "https://www.google.com",
                      Occupancy = 4,
                      Rate = 3000,
                      Sqft = 8000,
                      Amenity = "",
                      CreatedDate = DateTime.Now,

                  }

                );
        }

    }
}