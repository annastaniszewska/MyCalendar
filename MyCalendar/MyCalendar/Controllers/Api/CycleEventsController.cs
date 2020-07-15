using Microsoft.AspNet.Identity;
using MyCalendar.Models;
using System.Linq;
using System.Web.Http;

namespace MyCalendar.Controllers.Api
{
    [Authorize]
    public class CycleEventsController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public CycleEventsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var userId = User.Identity.GetUserId();
            var cycleEvent = _context.Events.Single(c => c.Id == id && c.UserId == userId);

            if (cycleEvent.IsCanceled)
            {
                return NotFound();
            }

            cycleEvent.Cancel();
            
            _context.SaveChanges();

            return Ok();
        }
    }
}
