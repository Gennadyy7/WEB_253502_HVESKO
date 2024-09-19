using Microsoft.AspNetCore.Mvc;

namespace WEB_253502_HVESKO.UI.Controllers
{
    public class Home : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
