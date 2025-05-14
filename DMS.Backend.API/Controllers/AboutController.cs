using Microsoft.AspNetCore.Mvc;

namespace DMS.Backend.API.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}