using MyCalendar.Core;
using MyCalendar.Core.Models;
using MyCalendar.Core.Repositories;
using MyCalendar.Persistence.Repositories;

namespace MyCalendar.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public ICycleEventRepository CycleEvents { get; private set; }
        public ITypeRepository Types { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            CycleEvents = new CycleEventRepository(_context);
            Types = new TypeRepository(_context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}