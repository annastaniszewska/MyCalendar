using Microsoft.AspNet.Identity;
using MyCalendar.Core;
using System.Web.Http;

namespace MyCalendar.Controllers.Api
{
    [Authorize]
    public class CycleEventsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public CycleEventsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
