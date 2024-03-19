using Microsoft.AspNetCore.Mvc;
using PharmacyManagmentSystem.Data;
using PharmacyManagmentSystem.Models;

namespace PharmacyManagmentSystem.Controllers
{
    public class CurrenciesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CurrenciesController(ApplicationDbContext context) 
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Currencies.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Currency currency)
        {
            if(ModelState.IsValid)
            {
                _context.Currencies.Add(currency);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        
        public ActionResult Edit(int id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var currency = _context.Currencies.FirstOrDefault(c => c.CurrencyID == id);
            if(currency == null)
            {
                return NotFound(currency);
            }
            return View(currency);

        }
        [HttpPost]
        public IActionResult Edit(Currency currency)
        {
            if (ModelState.IsValid)
            {
                _context.Update(currency);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(currency);
        }
        
        public IActionResult Details(int id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var currency = _context.Currencies.FirstOrDefault(c => c.CurrencyID == id);
            return View(currency);
        }
        
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var currency = _context.Currencies.FirstOrDefault(c => c.CurrencyID == id);
            if (currency == null)
            {
                return NotFound();
            }
            return View(currency);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var currency = _context.Currencies.Find(id);
            if(currency != null)
            {
                _context.Currencies.Remove(currency);
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
