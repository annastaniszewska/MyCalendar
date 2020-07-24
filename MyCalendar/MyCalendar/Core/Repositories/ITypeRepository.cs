using System.Collections.Generic;
using MyCalendar.Core.Models;

namespace MyCalendar.Core.Repositories
{
    public interface ITypeRepository
    {
        IEnumerable<Type> GetEventTypes();
    }
}