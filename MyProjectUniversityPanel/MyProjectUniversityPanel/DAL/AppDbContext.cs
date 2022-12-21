using MyProjectUniversityPanel.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace MyProjectUniversityPanel.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<HasSuperAdmin> HasSuperAdmins { get; set; }
        public DbSet<DepartmentDetail> DepartmentDetails { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Kassa> Kassas { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Outcome> Outcomes { get; set; }
        public DbSet<Designation> Designations { get; set; }

        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<StudentGroup> StudentGroups { get; set; }
        public DbSet<TeacherGroups> TeacherGroups { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<HasSuperAdmin>().HasData(new HasSuperAdmin { Id = 1, HasSuperadmin = false });
            builder.Entity<Gender>().HasData(new Gender { Id = 1, Type = "Male" });
            builder.Entity<Gender>().HasData(new Gender { Id = 2, Type = "Female" });
            builder.Entity<Gender>().HasData(new Gender { Id = 3, Type = "Other" });
            builder.Entity<Department>().HasData(new Department { Id = 1, Name = "Default" , IsDeactive=false});
            builder.Entity<Kassa>().HasData(new Kassa { Id = 1, Balance = 0,LastModifiedBy="",LastModifiedMoney=0,LastModifiedFor="",LastModifiedTime= DateTime.UtcNow.AddHours(4),IsDeactive=false,AppUserId= "a4dab9a1-cbf9-4795-a071-b4255ede23d9" });

        }
    }
}
