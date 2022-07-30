using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("/api")]
    public class MailController : Controller
    {
        // MailService provide main logic while controller itself only manages results
        private readonly IMailService _mailService;

        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost("subscribe")]
        public ActionResult Subscribe([FromForm] string email)
        {
            try
            {
                _mailService.Subscribe(email);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
            return Ok();
        }

        [HttpPost("sendEmails")]
        public ActionResult SendEmails()
        {
            try
            {
                _mailService.SendEmails();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
            return Ok();
        }
    }
}
