using Microsoft.AspNet.Identity.EntityFramework;
using MyCalendar.Core.Models;
using MyCalendar.Persistence.EntityConfigurations;
using System.Data.Entity;

namespace MyCalendar.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Type> Types { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<System.Type>();

            modelBuilder.Configurations.Add(new ApplicationUserConfiguration());
            modelBuilder.Configurations.Add(new EventConfiguration());
            modelBuilder.Configurations.Add(new TypeConfiguration());
            
            base.OnModelCreating(modelBuilder);
        }
    }
}