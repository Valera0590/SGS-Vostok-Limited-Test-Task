using Microsoft.EntityFrameworkCore;
namespace ManageStaff.Model.Data
{
    public class ApplicationContext: DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Workshop> Workshops { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ManageStaff;Trusted_Connection=True;");
        }
    }
}
