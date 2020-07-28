using System.Data.Entity;
using MyCalendar.Core.Models;

namespace MyCalendar.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<Event> Events { get; set; }
        DbSet<Type> Types { get; set; }
        IDbSet<ApplicationUser> Users { get; set; }
    }
}