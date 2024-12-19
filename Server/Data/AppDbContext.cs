using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Data.Entities.TestEntities;
using Data.Entities.UserEntities;
using Data.Entities.CommonEntities;
using Data.Configs.TestConfig;
using Data.Configs.UserConfig;
using Data.Configs.CommonConfig;
using Data.Entities.DocumentEntities;
using Data.Entities.PostEnttities;
using Data.Entities.PostEntities;
using Data.Entities.SystemEntities;
using Data.Configs.DocumentConfig;
using Data.Configs.PostConfig;
using Data.Configs.SystemConfig;

namespace Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new QuestionConfiguration());
            modelBuilder.ApplyConfiguration(new TestConfiguration());
            modelBuilder.ApplyConfiguration(new TestResultConfiguration());
            modelBuilder.ApplyConfiguration(new SavedTestConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new AppRoleConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new UserLikeConfiguration());
            modelBuilder.ApplyConfiguration(new SummaryConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentInfoConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new PostImageConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
            modelBuilder.ApplyConfiguration(new PromotionConfiguration());
            modelBuilder.ApplyConfiguration(new PromotionOrderConfiguration());
            modelBuilder.ApplyConfiguration(new UserNotificationConfiguration());
            modelBuilder.ApplyConfiguration(new UploadedFileConfiguration());

            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins").HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens").HasKey(x => x.UserId);
        }

        #region DbSet
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<UserLike> UserLikes { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<SavedTest> SavedTests { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Summary> Summaries { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<PaymentInfo> PaymentInfos { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<PromotionOrder> PromotionOrders { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        #endregion
    }
}
