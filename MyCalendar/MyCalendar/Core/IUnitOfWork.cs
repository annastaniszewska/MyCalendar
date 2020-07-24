using MyCalendar.Core.Repositories;

namespace MyCalendar.Core
{
    public interface IUnitOfWork
    {
        ICycleEventRepository CycleEvents { get; }
        ITypeRepository Types { get; }
        void Complete();
    }
}