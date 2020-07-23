using Microsoft.AspNet.Identity;
using MyCalendar.Models;
using MyCalendar.Repositories;
using System.Web.Http;

namespace MyCalendar.Controllers.Api
{
    [Authorize]
    public class CycleEventsController : ApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly CycleEventRepository _cycleEventRepository;

        public CycleEventsController()
        {
            _context = new ApplicationDbContext();
            _cycleEventRepository = new CycleEventRepository(_context);
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var cycleEvent = _cycleEventRepository.GetCycleEvent(id);

            if (cycleEvent == null || cycleEvent.IsCanceled)
            {
                return NotFound();
            }

            if (cycleEvent.UserId != User.Identity.GetUserId())
            {
                return Unauthorized();
            }
            
            cycleEvent.Cancel();
            
            _context.SaveChanges();

            return Ok();
        }
    }
}
