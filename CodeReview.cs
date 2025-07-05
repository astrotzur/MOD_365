using Microsoft.AspNetCore.Mvc;
using System.Net.Http;                   // needed for HttpClient
using System.Threading.Tasks;            // needed for async/await

namespace MyApp.Controllers
{
    [Route("api/[controller]")]
    public class SampleController : Controller
    {
        [HttpGet]
        public async Task<string> Get() // changed to async Task<string> to fix deadlock/blocking
        {
            using (var client = new HttpClient()) // add using to dispose properly
            {
                // changed .Result to await to avoid thread blocking/deadlock
                var response = await client.GetAsync("https://graph.microsoft.com/v1.0/sites/root");
                return await response.Content.ReadAsStringAsync();
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] dynamic data)
        {
            // Save to SharePoint
            return Ok();
        }
    }
}
