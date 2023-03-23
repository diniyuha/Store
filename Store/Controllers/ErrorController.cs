using Microsoft.AspNetCore.Mvc;

namespace Store.Controllers
{
    [Route("[controller]")]
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}