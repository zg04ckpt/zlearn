using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace  Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=ZG04-CKPT\\SQLEXPRESS,1433;Database=zlearn;User Id=sa;Password=hcn14022004;");
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
