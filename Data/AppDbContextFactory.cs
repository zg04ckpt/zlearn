using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace  Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=tcp:zg04ckpt.database.windows.net,1433;Initial Catalog=zg04ckpt;Persist Security Info=False;User ID=zg04ckpt;Password=Hcn14022004!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
