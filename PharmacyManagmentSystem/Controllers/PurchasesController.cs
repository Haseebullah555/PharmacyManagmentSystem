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
        public IActionResult Create(decimal Amount, decimal UnitPrice)
        {
            var medicine = _context.Medicines.Select(m => new SelectListItem()
            {
                Text = m.TradeName +" | "+ m.Capacity + " | " + m.Category.CategoryName,
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
            ViewBag.TotalPrice =  Amount * UnitPrice;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PurchasesViewModel viewModel)
        {
            Purchase purchase= _mapper.Map<Purchase>(viewModel);
            if(ModelState.IsValid)
            {
                _context.Purchases.Add(purchase);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }
    }
}
