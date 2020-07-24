using MyCalendar.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace MyCalendar.Persistence.EntityConfigurations
{
    public class ApplicationUserConfiguration : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfiguration()
        {
            Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}