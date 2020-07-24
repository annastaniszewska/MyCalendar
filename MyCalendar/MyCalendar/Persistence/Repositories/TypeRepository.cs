using System.Collections.Generic;
using System.Linq;
using MyCalendar.Core.Repositories;
using Type = MyCalendar.Core.Models.Type;

namespace MyCalendar.Persistence.Repositories
{
    public class TypeRepository : ITypeRepository
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