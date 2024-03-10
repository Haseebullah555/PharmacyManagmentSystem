using Microsoft.AspNetCore.Mvc;
using PharmacyManagmentSystem.Data;
using PharmacyManagmentSystem.Models;

namespace PharmacyManagmentSystem.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SuppliersController(ApplicationDbContext context) 
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Suppliers.ToList());
        }
    
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Supplier supplier)
        {
            if(ModelState.IsValid)
            {
                _context.Suppliers.Add(supplier);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var supplier = _context.Suppliers.FirstOrDefault(s=>s.SupplierID == id);
            if(supplier == null)
            {
                return NotFound();
            }
            return View();
        }

        [HttpPost]
        public IActionResult Edit(Supplier supplier) 
        {
           
            if(ModelState.IsValid)
            {
                _context.Suppliers.Update(supplier);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplier);
        }
    
        public IActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var supplier = _context.Suppliers.FirstOrDefault(s=>s.SupplierID==id);
            if(supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }
        
        public IActionResult Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var supplier = _context.Suppliers.FirstOrDefault(s=>s.SupplierID==id);
            if(supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmedDelete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var supplier = _context.Suppliers.Find(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    
    
    
    }
}
