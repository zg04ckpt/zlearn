
using  Data.Entities;
using Microsoft.EntityFrameworkCore;
using  Data.Configurations;

namespace  Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new QuestionConfig());
            modelBuilder.ApplyConfiguration(new QuestionSetConfig());
        }

        #region DbSet
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionSet> QuestionSets { get; set; }
        #endregion
    }
}
