using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;

namespace ProductAPI.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        { }
        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData(data);
        }

        List<Product> data = new List<Product>()
        {
            new Product{ ProductId=1,Name="Mobile",Description="Samsun s23",Category="Electronics",Price=48925.50},
            new Product{ ProductId=2,Name="Pressure Cooker",Description="Prestige",Category="Utensils",Price=1892},
            new Product{ ProductId=3,Name="Spider Mna",Description="Amazing spider man 2",Category="Movies",Price=8925.50},
            new Product{ ProductId=4,Name="Mobile",Description="Iphone 13",Category="Electronics",Price=58925.50},
        };
    }
}
