using LabTests.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabTests.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) 
        {


        }

        #region Properties
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<LabReport> LabReports { get; set; }
        #endregion

        #region Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<ApplicationUser>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ApplicationUser>().HasMany(u => u.Patients).WithOne(i => i.User);
            modelBuilder.Entity<ApplicationUser>().HasMany(u => u.LabReports).WithOne(i => i.User);

            modelBuilder.Entity<Patient>().ToTable("Patients");
            modelBuilder.Entity<Patient>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Patient>().HasOne(i => i.User).WithMany(u => u.Patients);
            modelBuilder.Entity<Patient>().HasMany(p => p.LabReports).WithOne(l => l.Patient);

            modelBuilder.Entity<LabReport>().ToTable("LabReports");
            modelBuilder.Entity<LabReport>().Property(l => l.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<LabReport>().HasOne(u => u.User).WithMany(u => u.LabReports);
            modelBuilder.Entity<LabReport>().HasOne(l => l.Patient).WithMany(p => p.LabReports);


        }
        #endregion
    }
}
