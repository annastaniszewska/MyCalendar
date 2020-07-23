using MyCalendar.Models;
using System.Collections.Generic;
using System.Linq;
using Type = MyCalendar.Models.Type;

namespace MyCalendar.Repositories
{
    public class TypeRepository
    {
        private readonly ApplicationDbContext _context;

        public TypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Type> GetEventTypes()
        {
            return _context.Types.ToList();
        }
    }
}