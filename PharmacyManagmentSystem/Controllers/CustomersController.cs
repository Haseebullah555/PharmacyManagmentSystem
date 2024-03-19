using Microsoft.AspNetCore.Mvc;
using PharmacyManagmentSystem.Data;
using PharmacyManagmentSystem.Models;

namespace PharmacyManagmentSystem.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Customers.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string CustomerName, string phoneNo,string Address, Customer customer)
        {
            bool customerExists = _context.Customers.Any(c=> c.CustomerName == CustomerName && c.PhoneNo==phoneNo && c.Address == Address);
            if(ModelState.IsValid)
            {
                if (customerExists)
                {
                    TempData["SuccessMessage"] = "Customer already exists!";
                    return View();

                }
                else
                {
                    _context.Customers.Add(customer);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Customer Added successfully!";
                    return RedirectToAction("Index");
                }
            }
            return View(customer);

        }
        
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerID == id);
            if(customer == null)
            {
                return NotFound(customer);
            }
            return View(customer);
        }
        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            if(ModelState.IsValid)
            {
                _context.Customers.Update(customer);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Customer Modified successfully!";
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        public IActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerID == id);
            return View(customer);
        }
        
        public IActionResult Delete(int id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var customer = _context.Customers.FirstOrDefault(c=> c.CustomerID == id);
            return View(customer);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var customer = _context.Customers.Find(id);
            if(customer != null)
            {
                _context.Customers.Remove(customer);
            }
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Customer Deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
