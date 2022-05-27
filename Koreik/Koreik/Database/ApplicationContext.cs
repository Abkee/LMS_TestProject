using Koreik.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Koreik.Database
{
    public class ApplicationContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<School> Schools { get; set; } = null!;
        public DbSet<Klass> Klasses { get; set; } = null!;
        public DbSet<Grade> Grades { get; set; } = null!;
        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Tutor> Tutors { get; set; } = null!;
        //public DbSet<Entiti> Entitis { get; set; } = null!;
        //public DbSet<Person> Persons { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }
    }
}
