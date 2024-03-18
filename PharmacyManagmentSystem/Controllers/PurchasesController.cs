using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PharmacyManagmentSystem.Data;
using PharmacyManagmentSystem.Models;
using PharmacyManagmentSystem.ViewModel;

namespace PharmacyManagmentSystem.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public PurchasesController(ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public IActionResult Index(int id)
        {
            var query = _context.Purchases.Include(m => m.Medicine).ThenInclude(m=>m.Company).Include(s => s.Supplier).Include(c => c.Currency).ToList();
            return View(query);
        }

        // List the purchases as a group with same Medicine name, Medicine Type, Medicine Company, Medicine Capacity, as well as same Currency,if the Currency is different it list it separatly
        // The medicine Expiry Date is the nearest expiry Date of the medicine in stock
        public IActionResult Stock()
        {
            var query = _context.Purchases
            .Include(p => p.Medicine)
            .ThenInclude(m => m.Company)
            .Include(p => p.Supplier)
            .Include(p => p.Currency)
            .GroupBy(p => new { p.Medicine.TradeName, p.Medicine.Capacity, p.Medicine.Company.CompanyName, p.CurrencyID })
            .Select(g => new Purchase
            {
                Medicine = new Medicine
                {
                    TradeName = g.Key.TradeName,
                    Capacity = g.Key.Capacity,
                    Company = new Company
                    {
                        CompanyName = g.Key.CompanyName
                    }
                },
                Amount = g.Sum(p => p.Amount),
                UnitPrice = g.FirstOrDefault(p => p.PurchaseDate == g.Max(d => d.PurchaseDate)).UnitPrice,
                SalePrice = g.FirstOrDefault(p => p.PurchaseDate == g.Max(d => d.PurchaseDate)).SalePrice,
                ExpiryDate = g.FirstOrDefault(p => p.ExpiryDate == g.Min(d => d.ExpiryDate)).ExpiryDate,
                PurchaseDate = g.Max(p => p.PurchaseDate),
                Currency = new Currency
                {
                    CurrencyName = g.FirstOrDefault(p => p.PurchaseDate == g.Max(d => d.PurchaseDate)).Currency.CurrencyName
                }
            })
            .ToList();
            return View(query);
        }
        public IActionResult Create()
        {
            var medicine = _context.Medicines.Select(m => new SelectListItem()
            {
                Text = m.TradeName +" | "+ m.Capacity + " | " + m.Category.CategoryName+" | "+ m.Company.CompanyName,
                Value = m.MedicineID.ToString()
            }).ToList();
            ViewBag.Medicines = medicine;

            var supplier = _context.Suppliers.Select(s => new SelectListItem()
            {
                Text = s.SupplierName,
                Value = s.SupplierID.ToString()
            }).ToList();
            ViewBag.Suppliers = supplier;

            var currency = _context.Currencies.Select(c => new SelectListItem()
            {
                Text = c.CurrencyName,
                Value = c.CurrencyID.ToString()
            }).ToList();
            ViewBag.Currencies = currency;
            //Calculating the total price of the purchased Item
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PurchasesViewModel viewModel, int MedicineID, int CurrencyID, DateTime PurchaseDate)
        {
            bool purchaseExists = _context.Purchases.Any(p => p.MedicineID == MedicineID && p.CurrencyID == CurrencyID && p.PurchaseDate == PurchaseDate);
            if (purchaseExists)
            {
                if (ModelState.IsValid)
                {
                    TempData["AddedMessage"] = "A purchase with the same Medicine, Currency, and Purchase Date already exists.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                Purchase purchase = _mapper.Map<Purchase>(viewModel);
                _context.Purchases.Add(purchase);
                _context.SaveChanges();
                TempData["AddedMessage"] = "Purchase added successfully!";
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }
        public IActionResult Edit(int? id)
        {
            var medicine = _context.Medicines.Select(m => new SelectListItem()
            {
                Text = m.TradeName + " | " + m.Capacity + " | " + m.Category.CategoryName,
                Value = m.MedicineID.ToString()
            }).ToList();
            ViewBag.Medicines = medicine;

            var supplier = _context.Suppliers.Select(s => new SelectListItem()
            {
                Text = s.SupplierName,
                Value = s.SupplierID.ToString()
            }).ToList();
            ViewBag.Suppliers = supplier;

            var currency = _context.Currencies.Select(c => new SelectListItem()
            {
                Text = c.CurrencyName,
                Value = c.CurrencyID.ToString()
            }).ToList();
            ViewBag.Currencies = currency;
            
            if (id == null)
            {
                return NotFound();
            }
            var purchase = _context.Purchases.FirstOrDefault(p=>p.PurchaseID==id);
            if(purchase == null)
            {
                return NotFound();
            }
            return View(purchase);
        }
        [HttpPost]
        public IActionResult Edit(PurchasesViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Purchase purchase = _mapper.Map<Purchase>(viewModel);
                _context.Purchases.Update(purchase);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }
        public IActionResult Details(int? id)
        {
            var query = _context.Purchases.Include(m => m.Medicine).Include(s => s.Supplier).Include(c => c.Currency).ToList();
            if (id == null)
            {
                return NotFound();
            }
            var purchase = _context.Purchases.FirstOrDefault(p => p.PurchaseID == id);
            if(purchase == null)
            {
                return NotFound();
            }
            return View(purchase);
        }
        public IActionResult Delete(int? id)
        {
            var query = _context.Purchases.Include(m => m.Medicine).Include(s => s.Supplier).Include(c => c.Currency).ToList();
            if (id == null)
            {
                return NotFound();
            }
            var purchase = _context.Purchases.FirstOrDefault(p => p.PurchaseID == id);
            if (purchase == null)
            {
                return NotFound();
            }
            return View(purchase);
            
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id) 
        {
            var purchase = _context.Purchases.FirstOrDefault(p=>p.PurchaseID == id);
            if (purchase != null)
            {
                _context.Purchases.Remove(purchase);
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
