using MyCalendar.Models;
using MyCalendar.Repositories;

namespace MyCalendar.Persistence
{
    public class UnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public CycleEventRepository CycleEvents { get; private set; }
        public TypeRepository Types { get; private set; }

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