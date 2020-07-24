using Microsoft.AspNet.Identity;
using MyCalendar.Persistence;
using System.Web.Http;
using MyCalendar.Core.Models;

namespace MyCalendar.Controllers.Api
{
    [Authorize]
    public class CycleEventsController : ApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly UnitOfWork _unitOfWork;

        public CycleEventsController()
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var cycleEvent = _unitOfWork.CycleEvents.GetCycleEvent(id);

            if (cycleEvent == null || cycleEvent.IsCanceled)
            {
                return NotFound();
            }

            if (cycleEvent.UserId != User.Identity.GetUserId())
            {
                return Unauthorized();
            }
            
            cycleEvent.Cancel();
            
            _unitOfWork.Complete();

            return Ok();
        }
    }
}
