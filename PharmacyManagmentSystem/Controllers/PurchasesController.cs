using Microsoft.AspNetCore.Mvc;

namespace PharmacyManagmentSystem.Controllers
{
    public class PurchasesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
