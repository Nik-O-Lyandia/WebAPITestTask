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
        // BitcoinService provide main logic while controller itself only manages results
        private readonly IBitcoinService _bitcoinSrvice;

        public BitcoinController(IBitcoinService bitcoinService)
        {
            _bitcoinSrvice = bitcoinService;
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
    }
}
