using Microsoft.AspNetCore.Mvc;
using PharmacyManagmentSystem.Data;
using PharmacyManagmentSystem.Models;

namespace PharmacyManagmentSystem.Controllers
{
    public class CompaniesController : Controller
    {
        private ApplicationDbContext db;

        public CompaniesController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {

            return View(db.Companies.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
       public IActionResult Create(Company cm)
        {

            if(ModelState.IsValid)
            {
                db.Companies.Add(cm);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View();
        }

    }
}
