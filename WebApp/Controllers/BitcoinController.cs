using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("subscribe")]
        public ActionResult Subscribe(string email)
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
                return NotFound(ex.Message);
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
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}
