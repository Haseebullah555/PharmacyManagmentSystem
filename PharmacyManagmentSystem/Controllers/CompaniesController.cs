using Microsoft.AspNetCore.Mvc;
using PharmacyManagmentSystem.Data;
using PharmacyManagmentSystem.Models;

namespace PharmacyManagmentSystem.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View(_context.Companies.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Company company)
        {

            if(ModelState.IsValid)
            {
                _context.Add(company);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var company = _context.Companies.Find(id);
            if(company == null)
            {
                return NotFound();
            }
            return View(company);
        }
        [HttpPost]
        public IActionResult Edit(Company company)
        {
            if(ModelState.IsValid)
            {
                _context.Update(company);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(company);
        }
        public IActionResult Details(int id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var company = _context.Companies.FirstOrDefault(c => c.CompanyID == id);
            if(company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        public IActionResult Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var company = _context.Companies.FirstOrDefault(c => c.CompanyID == id);
            if(company == null)
            {
                return NotFound();
            }
            return View(company);

        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var company = _context.Companies.Find(id);
            if(company!= null)
            {
                _context.Companies.Remove(company);
            }
            _context.SaveChanges(true);
            return RedirectToAction("Index");
        }
    }
}
