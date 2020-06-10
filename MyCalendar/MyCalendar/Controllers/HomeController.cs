using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using MyCalendar.Models;
using System.Linq;
using System.Web.Mvc;

namespace MyCalendar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var latestEvent = _context.Events
                .Include(e => e.User)
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.StartDate)
                .FirstOrDefault();

            return View(latestEvent);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}