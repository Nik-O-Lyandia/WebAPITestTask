using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services;
using WebApp.Interfaces;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("/api")]
    public class BitcoinController : Controller
    {
        // BitcoinService and MailService provide main logic while controller itself only manages results
        private readonly IBitcoinService _bitcoinSrvice;
        private readonly IMailService _mailService;

        public BitcoinController(IBitcoinService bitcoinService, IMailService mailService)
        {
            _bitcoinSrvice = bitcoinService;
            _mailService = mailService;
        }

        [HttpGet("rate")]
        public ActionResult Rate()
        {
            try
            {
                return Ok(_bitcoinSrvice.Rate());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
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
