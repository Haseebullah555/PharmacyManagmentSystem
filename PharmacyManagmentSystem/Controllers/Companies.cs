using Microsoft.AspNetCore.Mvc;

namespace PharmacyManagmentSystem.Controllers
{
    public class Companies : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
