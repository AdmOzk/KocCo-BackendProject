using KocCoAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KocCoAPI.Infrastructure.Persistence
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext(DbContextOptions<SqlDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=YourServer;Database=YourDatabase;Trusted_Connection=True;MultipleActiveResultSets=True;Integrated Security=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(c => c.UserId);

            modelBuilder.Entity<Cart>()
                .HasKey(c => c.CartId);

            modelBuilder.Entity<Package>()
                .HasKey(c => c.PackageID);

            modelBuilder.Entity<CoachPackage>()
       .HasNoKey()
       .ToTable("CoachPackages");

            modelBuilder.Entity<CartPackage>()
                .HasNoKey();

            modelBuilder.Entity<UserPurchased>()
                .HasKey(c => c.UserPackageID);

            modelBuilder.Entity<CartPackage>()
                .HasNoKey();

            modelBuilder.Entity<CoachingSession>()
                .HasKey(c => c.SessionID);

            modelBuilder.Entity<Day>()
                .HasKey(c => c.DayId);

            modelBuilder.Entity<SharedResource>()
                .HasKey(c => c.SharedResourceId);

            modelBuilder.Entity<Test>()
                .HasKey(c => c.TestId);

            modelBuilder.Entity<CoachPackage>()
              .HasKey(cp => new { cp.CoachId, cp.PackageId }); // Define composite key

            modelBuilder.Entity<TestStudent>()
                .HasNoKey();

            modelBuilder.Entity<TimeSlot>()
                .HasKey(c => c.TimeSlotID);

            modelBuilder.Entity<WorkSchedule>()
                .HasKey(c => c.WorkScheduleId);

            modelBuilder.Entity<TestStudent>()
.HasKey(ts => new { ts.TestId, ts.StudentId }); // Composite key

            modelBuilder.Entity<TestStudent>()
                .HasOne(ts => ts.Test)
                .WithMany(t => t.TestStudents)
                .HasForeignKey(ts => ts.TestId);

            modelBuilder.Entity<TestStudent>()
                .HasOne(ts => ts.User)
                .WithMany(u => u.TestStudents)
                .HasForeignKey(ts => ts.StudentId);

            modelBuilder.Entity<TestResult>()
          .HasKey(tr => tr.TestResultId); // Birincil anahtar olarak tanımlar

            // User -> UserPurchases (Bir Kullanıcı Birçok Satın Alım Yapabilir)
            modelBuilder.Entity<UserPurchased>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPurchases)
                .HasForeignKey(up => up.StudentID);

            // UserPurchased -> Package (Bir Satın Alımda Bir Paket Olabilir)
            modelBuilder.Entity<UserPurchased>()
                .HasOne(up => up.Package)
                .WithMany()
                .HasForeignKey(up => up.PackageID);

            modelBuilder.Entity<TestStudent>()
.HasKey(ts => new { ts.TestId, ts.StudentId }); // Composite key


            modelBuilder.Entity<CartPackage>()
       .HasKey(cp => new { cp.CartId, cp.PackageId }); // Composite key
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartPackage> CartPackages { get; set; }
        public DbSet<CoachingSession> CoachingSessions { get; set; }
        public DbSet<CoachPackage> CoachPackages { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<SharedResource> SharedResources { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestStudent> TestStudents { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<UserPurchased> UserPurchases { get; set; }
        public DbSet<WorkSchedule> WorkSchedules { get; set; }

        public DbSet<TestResult> TestResults { get; set; }
    }
}
