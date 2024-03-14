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

        public IActionResult Index()
        {
            var query = _context.Purchases.Include(m => m.Medicine).Include(s => s.Supplier).Include(c => c.Currency).ToList();
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
            bool showablePurchase = _context.Purchases.Any(p => p.MedicineID == MedicineID && p.CurrencyID == CurrencyID);

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
                if (showablePurchase)
                {
                    // Retrieve the existing purchases with the same MedicineID and CurrencyID
                    var existingPurchases = _context.Purchases
                        .Where(p => p.MedicineID == MedicineID && p.CurrencyID == CurrencyID)
                        .ToList();

                    // Calculate the summation of amounts for the existing purchases
                    int totalAmount = existingPurchases.Sum(p => p.Amount);

                    // Add the new purchase with the updated amount
                    viewModel.Amount += totalAmount;
                }

                Purchase purchase = _mapper.Map<Purchase>(viewModel);
                _context.Purchases.Add(purchase);
                _context.SaveChanges();

                TempData["AddedMessage"] = "Purchase added successfully!";
            }

            var viewModelForIndex = new PurchasesViewModel
            {
                PurchaseDate = viewModel.PurchaseDate,
                Amount = viewModel.Amount,
                UnitPrice = viewModel.UnitPrice,
                SalePrice = viewModel.SalePrice
            };

            // Pass the view model to the index view
            return RedirectToAction("Index", viewModelForIndex);
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
