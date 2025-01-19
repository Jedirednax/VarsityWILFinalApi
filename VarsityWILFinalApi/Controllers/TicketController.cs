// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VCTicketTrackerAPIClassLibrary;
using VCTicketTrackerAPIClassLibrary.Handlers;
using VCTicketTrackerAPIClassLibrary.StorageDB;
using VCTicketTrackerAPIClassLibrary.Tickets;

namespace VarsityWILFinalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {

        private readonly ILogger<TicketController> _logger;
        public TicketController(ILogger<TicketController> logger)
        {
            _logger = logger;
        }

        // GET api/<TicketController>/0
        [HttpGet("UserTickets/{userId}")]
        [ProducesResponseType(typeof(Ticket), 200)]
        [AllowAnonymous]
        public IEnumerable<Ticket> GetAllTickets(long userId)
        {
            Console.WriteLine($"Get TIckets FOr: {userId}");
            List<Ticket> ticketList = DatabaseAccesUtil.GetUserTickets(userId);
            foreach(Ticket ticket in ticketList)
            {
                ticket.GetDetails();
            }
            return ticketList;
        }

        // POST api/<TicketController>/0
        [Route("GetCategory")]
        [ProducesResponseType(typeof(Category), 200)]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<List<Category>> GetCategory()
        {


            List<Category> cate = DatabaseAccesUtil.GetCategory();
            if(cate != null)
            {

                return cate;
            }
            else
            {
                return NoContent();
            }
        }
        // POST api/<TicketController>/0
        [Route("GetTicket/{ticketNum}")]
        [ProducesResponseType(typeof(Ticket), 200)]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<Ticket> GetTicket(long ticketNum)
        {
            Ticket ticket = DatabaseAccesUtil.GetTicketID(ticketNum);
            if(ticket != null)
            {
                ticket.GetDetails();
                return ticket;
            }
            else
            {
                return NoContent();
            }
        }


        // POST api/<TicketController>/0
        [Route("GetAll")]
        [ProducesResponseType(typeof(List<Ticket>), 200)]
        [HttpGet]
        //[Authorize(Roles = "ServerAdmin,Adminastrator,SupportMember")]
        [AllowAnonymous]
        public ActionResult<List<Ticket>> GetAllTicket()
        {
            List<Ticket> ticket = DatabaseAccesUtil.GetAllTickets();
            if(ticket != null)
            {

                foreach(Ticket t in ticket)
                {
                    t.GetDetails();
                }
                return ticket;
            }
            else
            {
                return NoContent();
            }
        }

        // POST api/<TicketController>/0
        [Route("StatusTickets/{status}")]
        [ProducesResponseType(typeof(List<Ticket>), 200)]
        [HttpGet]
        //[Authorize(Roles = "ServerAdmin,Adminastrator,SupportMember")]
        [AllowAnonymous]
        public ActionResult<List<Ticket>> GetTicketStatus(string status)
        {
            List<Ticket> ticket = DatabaseAccesUtil.GetStatusTickets(Enum.Parse<Status>(status));
            if(ticket != null)
            {

                foreach(Ticket t in ticket)
                {
                    t.GetDetails();
                }
                return ticket;
            }
            else
            {
                return NoContent();
            }
        }

        // POST api/<TicketController>/0
        [Route("DateRangeTickets/")]
        [ProducesResponseType(typeof(List<Ticket>), 200)]
        [HttpGet]
        //[Authorize(Roles = "ServerAdmin,Adminastrator,SupportMember")]
        [AllowAnonymous]
        public ActionResult<List<Ticket>> GetDateRange([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            List<Ticket> ticket = DatabaseAccesUtil.GetTicketCreatedBtw(start,end);
            if(ticket != null)
            {

                foreach(Ticket t in ticket)
                {
                    t.GetDetails();
                }
                return ticket;
            }
            else
            {
                return NoContent();
            }
        }

        // Get api/<TicketController>/0
        [Route("AssignedTickets/{userId}")]
        [ProducesResponseType(typeof(List<Ticket>), 200)]
        [HttpGet]
        //[Authorize(Roles = "ServerAdmin,Adminastrator,SupportMember")]
        [AllowAnonymous]
        public ActionResult<List<Ticket>> GetDateRange(long userId)
        {
            List<Ticket> ticket = DatabaseAccesUtil.GetAssginedTickets(userId);
            if(ticket != null)
            {

                foreach(Ticket t in ticket)
                {
                    t.GetDetails();
                }
                return ticket;
            }
            else
            {
                return NoContent();
            }
        }

        // POST api/<TicketController>
        [Route("Create")]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult AddTicket([FromBody] TicketCreateDto ticketDTO)
        {
            try
            {

                var ticket = new Ticket(ticketDTO);
                var res = ticket.Insert();
                if(res > 0)
                {
                    return Ok(res);
                }
                else
                {
                    return Problem();
                }
            }

            catch(Exception ex)
            {
                return Problem(ex.ToString());
            }
        }


        // POST api/<TicketController>
        [Route("Update")]
        [HttpPost]
        //[Authorize(Roles = "ServerAdmin,Adminastrator,SupportMember,Lecturer")]
        [AllowAnonymous]
        public ActionResult UpdateTicket([FromBody] TicketUpdate ticketUpdate)
        {
            try
            {

                Ticket ticket = DatabaseAccesUtil.GetTicketID(ticketUpdate.ticketID);
                if(ticket != null)
                {
                    if(ticket.Update(ticketUpdate))
                    {
                        return Accepted();
                    }
                    else
                    {
                        return Problem();
                    }
                }
                else
                {
                    return NoContent();
                }
            }
            catch(Exception ex)
            {
                return Problem(ex.ToString());
            }
        }

        // POST api/<TicketController>
        [Route("Assgin/{TicketId}")]
        [HttpPost]
        //[Authorize(Roles = "ServerAdmin,Adminastrator,SupportMember")]
        [AllowAnonymous]
        public ActionResult AssginTicket(int TicketId, [FromQuery] int UserId)
        {

            Ticket t = DatabaseAccesUtil.GetTicketID(TicketId);

            if(t.Assgin(UserId))
            {
                return Accepted();
            }
            else
            {
                return Problem();
            }
        }

        // DELETE api/<TicketController>
        [Route("Delete/{id}")]
        [HttpDelete]
        //[Authorize(Roles = "ServerAdmin,Adminastrator,SupportMember")]
        [AllowAnonymous]
        public ActionResult DeleteTicket(int id)
        {
            if(DatabaseAccesUtil.TicketDelete(id))
            {
                return NoContent();
            }
            else
            {
                return Problem();
            }
        }

        // DELETE api/<TicketController>
        [Route("Filter/{category}")]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<List<Ticket>> FilterTicket(string category)
        {
            var p = DatabaseAccesUtil.GetTicketCategory((int)Enum.Parse<Categories>(category));
            if(p != null)
            {
                return p;
            }
            else
            {
                return Problem();
            }
        }
        // DELETE api/<TicketController>
        [Route("Notification/{userId}")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Notification(long userId)
        {
            //var p = DatabaseAccesUtil.GetTicketCategory((int)Enum.Parse<Categories>(category));
            var user = DatabaseAccesUtil.GetUser(userId);
            NotificationHandler.SendPushNot("This is a Test", Status.WithStudent, user.fcmToken);
            return Ok();
        }



    }
}
//
