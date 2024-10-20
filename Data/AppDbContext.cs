using Microsoft.EntityFrameworkCore;
using Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity;
using Data.Configs;
using Data.Entities;

namespace Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new QuestionConfig());
            modelBuilder.ApplyConfiguration(new TestConfig());
            modelBuilder.ApplyConfiguration(new TestResultConfig());
            modelBuilder.ApplyConfiguration(new SavedTestConfig());
            modelBuilder.ApplyConfiguration(new AppUserConfig());
            modelBuilder.ApplyConfiguration(new AppRoleConfig());
            modelBuilder.ApplyConfiguration(new ImageConfig());

            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins").HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens").HasKey(x => x.UserId);
        }

        #region DbSet
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<SavedTest> SavedTests { get; set; }
        public DbSet<Image> Images { get; set; }
        #endregion
    }
}
