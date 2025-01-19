using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VCTicketTrackerAPIClassLibrary;
using VCTicketTrackerAPIClassLibrary.StorageDB;
using VCTicketTrackerAPIClassLibrary.Tickets;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VarsityWILFinalApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        

        private readonly ILogger<DocumentController> _logger;
        public DocumentController(ILogger<DocumentController> logger)
        {
            _logger = logger;
        }

        [HttpPost("UploadFiles/{ticketId}")]
        [AllowAnonymous]
        public IActionResult UploadFile(long ticketId, Collection<IFormFile> formFile)
        {
            string rootPath = @".\home\site\ticketDocuments";
            if(!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
            foreach(IFormFile item in formFile)
            {
                string fiName = item.FileName;

                if(item.Length > 0)
                {
                    string filePath = Path.Combine(rootPath,ticketId.ToString() );
                    if(!Path.Exists(filePath))
                    {
                        Directory.CreateDirectory(Path.Combine(rootPath, ticketId.ToString()));
                    }
                    filePath = Path.Combine(rootPath, ticketId.ToString(), fiName);
                    using(FileStream stream = System.IO.File.Create(filePath))
                    {
                        Ticket ticket = DatabaseAccesUtil.GetTicketID(ticketId);
                        TicketUpdate update = new TicketUpdate(ticketId, ticket.TicketCreatedBy,(Status)ticket.TicketStatus, "Document","Added docs");
                        ticket.AddDocument(update, fiName);
                        item.CopyTo(stream);
                    }
                }
            }
            return Ok();
        }

        // TODO Add FIle Path for documnet store
        [HttpGet("DownloadFile/{ticketId}/{FileName}")]
        [AllowAnonymous]
        public ActionResult<IFormFile> DownloadFile(long ticketId, string FileName)
        {
            try
            {


                string rootPath = @".\home\site\ticketDocuments";
                string filePath = Path.Combine(rootPath,ticketId.ToString() ,FileName);
                string ext = Path.GetExtension(filePath);


                byte[] fileContents = System.IO.File.ReadAllBytes(filePath);
                return File(fileContents, $"application/{ext}", filePath);
            }
            catch
            {
                return NoContent();
            }
        }



        [HttpGet("DownloadTimeTable/{CourseCode}")]
        [AllowAnonymous]
        public ActionResult<IFormFile> DownloadTimeTable(string CourseCode)
        {
            //string rootPath = "C:\\Users\\jedix\\Desktop";
            string rootPath = @".\home\site\timeTables";

            if(CourseCode == "timetable.pdf")
            {
                string filePath = Path.Combine(rootPath,CourseCode);

                try
                {
                    byte[] fileContents = System.IO.File.ReadAllBytes(filePath);
                    string ext = Path.GetExtension(filePath);
                    return File(fileContents, $"application/{ext}", Path.GetFileName(filePath));
                }
                catch
                {
                    return NoContent();
                }
            }
            else
            {
                return NoContent();
            }
        }

    }
}
