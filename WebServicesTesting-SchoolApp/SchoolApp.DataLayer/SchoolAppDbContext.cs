using SchoolApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.DataLayer
{
    public class SchoolAppDbContext : DbContext
    {
        public DbSet<School> Schools { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Mark> Marks { get; set; }

        public SchoolAppDbContext()
            : base("SchoolAppDb")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<School>().HasKey(x => x.Id);
            modelBuilder.Entity<School>().Property(x => x.Name).IsRequired().HasMaxLength(30);
            modelBuilder.Entity<School>().Property(x => x.Location).IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Student>().HasKey(x => x.Id);
            modelBuilder.Entity<Student>().Property(x => x.FirstName).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Student>().Property(x => x.LastName).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Student>().Property(x => x.Grade).IsRequired();

            modelBuilder.Entity<Mark>().HasKey(x => x.Id);
            modelBuilder.Entity<Mark>().Property(x => x.Subject).IsRequired().HasMaxLength(30);
            modelBuilder.Entity<Mark>().Property(x => x.Value).IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
