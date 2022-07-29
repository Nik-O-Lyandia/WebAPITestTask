using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("/api")]
    public class BitcoinController : Controller
    {
        // BitcoinService provides main logic while controller itself only manages results
        BitcoinService _bitcoinSrvice;

        public BitcoinController(IHttpClientFactory clientFactory, IConfiguration config)
        {
            // Instant transfer of interfaces to BitcoinService because of no need of stashing them in controller
            _bitcoinSrvice = new BitcoinService(clientFactory, config);
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
                _bitcoinSrvice.Subscribe(email);
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
                _bitcoinSrvice.SendEmails();
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
