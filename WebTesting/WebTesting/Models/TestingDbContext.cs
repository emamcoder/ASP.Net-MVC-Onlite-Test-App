namespace WebTesting.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TestingDbContext : DbContext
    {
        public TestingDbContext()
            : base("DefaultConnection")
        {
        }

        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<Testing> Testings { get; set; }
        public virtual DbSet<TestResult> TestResults { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
                .HasMany(e => e.Testings)
                .WithRequired(e => e.Question)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Test>()
                .HasMany(e => e.Questions)
                .WithRequired(e => e.Test)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Test>()
                .HasMany(e => e.TestResults)
                .WithRequired(e => e.Test)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TestResult>()
                .Property(e => e.WonPoints)
                .HasPrecision(9, 3);
        }
    }
}
