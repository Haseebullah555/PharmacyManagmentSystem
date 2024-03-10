using Microsoft.AspNetCore.Mvc;

namespace PharmacyManagmentSystem.Controllers
{
    public class SalesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
