using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VCTicketTrackerAPIClassLibrary.StorageDB;
using VCTicketTrackerAPIClassLibrary.Supporting;

namespace VarsityWILFinalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportController : ControllerBase
    {
        
        [HttpGet("FAQ/{page}")]
        [AllowAnonymous]
        public ActionResult<List<FAQ>> GetFaqa(int page)
        {
            List<FAQ> faqList =DatabaseAccesUtil.GetFAQ(page);
            if(faqList != null && faqList.Count > 0)
            {
                return Ok(faqList);
            }
            else
            {
                return NoContent();
            }
        }
        [HttpGet("FAQ/Search")]
        [AllowAnonymous]
        public ActionResult<List<FAQ>> GetFaq([FromQuery] string topic = "", [FromQuery] string answer = "", [FromQuery] string question = "", [FromQuery] byte rating = 0)
        {

            //List<FAQ> faqList =DatabaseAccesUtil.GetFAQ(10,page);
            List<FAQ> faqList = DatabaseAccesUtil.GetFAQAnswer(question,answer,topic,rating);
            if(faqList != null && faqList.Count > 0)
            {
                return Ok(faqList);
            }
            else
            {
                return NoContent();
            }
        }

        // POST api/<TicketController>
        [Route("FAQ/Add")]
        [HttpPost]
        //[Authorize(Roles = "ServerAdmin,Adminastrator,SupportMember")]
        [AllowAnonymous]
        public ActionResult AddFAQ([FromBody] FAQ faq)
        {
            long faqId = DatabaseAccesUtil.InsertFAQ(faq);
            if(faqId>=0)
            {
                return Ok();

            }
            else
            {
                return NoContent();
            }
        }

        // POST api/<TicketController>
        [Route("FAQ/Add/{faqId}")]
        [HttpPost]
        //[Authorize(Roles = "ServerAdmin,Adminastrator,SupportMember")]
        [AllowAnonymous]
        public ActionResult AddFAQ(long faqId, Collection<IFormFile> formFile)
        {
            //var faqId = DatabaseAccesUtil.InsertFAQ(faq);
            if(faqId > 0 && formFile != null)
            {
                string rootPath = @".\home\site\FaqDocuments";
                if(!Directory.Exists(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                }
                foreach(IFormFile item in formFile)
                {
                    string fiName = item.FileName;

                    if(item.Length > 0)
                    {
                        string filePath = Path.Combine(rootPath,faqId.ToString() ,Path.GetFileName(fiName));
                        if(!Path.Exists(filePath))
                        {
                            Directory.CreateDirectory(Path.Combine(rootPath, faqId.ToString()));
                        }

                        using(FileStream stream = System.IO.File.Create(filePath))
                        {
                            item.CopyTo(stream);
                        }
                    }
                    return Accepted();

                }
                return Ok();
            }
            else { return NoContent(); }
        }

        [HttpGet("DownloadFile/{faqId}/{fileName}")]
        [AllowAnonymous]
        public ActionResult<IFormFile> DownloadFaqFile(long faqId, string fileName)
        {
            string rootPath = @".\home\site\FaqDocuments";
            if(!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
            string filePath = Path.Combine(rootPath,faqId.ToString() ,Path.GetFileName(fileName));

            try
            {
                string ext = Path.GetExtension(filePath).Remove(0,1) ;

                byte[] fileContents = System.IO.File.ReadAllBytes(filePath);
                return File(fileContents, $"application/{ext}", filePath);
            }
            catch
            {
                return NoContent();
            }
        }
    }
}
