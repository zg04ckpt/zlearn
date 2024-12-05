using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace  Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseMySql(
                "Server=localhost;Database=zlearn;User=root;Password=admin;",
                new MySqlServerVersion(new Version(8, 0, 37)) 
            );
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
