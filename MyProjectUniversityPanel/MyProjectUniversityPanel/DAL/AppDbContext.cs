using MyProjectUniversityPanel.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyProjectUniversityPanel.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Department> Departments { get; set; }
        //public DbSet<Teacher> Teachers { get; set; }
        //public DbSet<Designation> Designations { get; set; }
        public DbSet<HasSuperAdmin> HasSuperAdmins { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<HasSuperAdmin>().HasData(new HasSuperAdmin { Id = 1, HasSuperadmin = false });
            builder.Entity<Gender>().HasData(new Gender { Id = 1, Type = "Male" });
            builder.Entity<Gender>().HasData(new Gender { Id = 2, Type = "Female" });
            builder.Entity<Gender>().HasData(new Gender { Id = 3, Type = "Other" });
        }
    }
}
