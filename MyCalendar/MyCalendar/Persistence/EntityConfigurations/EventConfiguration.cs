using MyCalendar.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace MyCalendar.Persistence.EntityConfigurations
{
    public class EventConfiguration : EntityTypeConfiguration<Event>
    {
        public EventConfiguration()
        {
            Property(e => e.UserId)
                .IsRequired();

            Property(e => e.TypeId)
                .IsRequired();
        }
    }
}