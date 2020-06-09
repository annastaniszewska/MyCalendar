using MyCalendar.Models;
using MyCalendar.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace MyCalendar.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController()
        {
            _context = new ApplicationDbContext();
        }
        
        public ActionResult Create()
        {
            var viewModel = new EventFormViewModel
            {
                Types = _context.Types.ToList()
            };

            return View(viewModel);
        }
    }
}