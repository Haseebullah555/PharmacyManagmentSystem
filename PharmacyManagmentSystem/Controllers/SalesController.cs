using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using PharmacyManagmentSystem.Data;
using PharmacyManagmentSystem.Models;
using PharmacyManagmentSystem.ViewModel;
using System.Text.RegularExpressions;

namespace PharmacyManagmentSystem.Controllers
{
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SalesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index()
        {

            var query = _context.Sales.Include(m => m.Medicine).Include(c => c.Customer).Include(cr => cr.Currency).ToList();
            return View(query);
        }
        public IActionResult Create()
        {
            var query = _context.Purchases
                .Include(p => p.Medicine)
                    .ThenInclude(m => m.Company)
                .Include(p => p.Currency)
                .GroupBy(p => new { p.Medicine.TradeName, p.Medicine.Capacity, p.Medicine.Company.CompanyName, p.CurrencyID })
                .Select(g => new
                {
                    TradeName = g.Key.TradeName,
                    Capacity = g.Key.Capacity,
                    CompanyName = g.Key.CompanyName,
                    Amount = g.Sum(p => p.Amount),
                    UnitPrice = g.FirstOrDefault(p => p.PurchaseDate == g.Max(d => d.PurchaseDate)).UnitPrice,
                    SalePrice = g.FirstOrDefault(p => p.PurchaseDate == g.Max(d => d.PurchaseDate)).SalePrice,
                    ExpiryDate = g.FirstOrDefault(p => p.ExpiryDate == g.Min(d => d.ExpiryDate)).ExpiryDate,
                    PurchaseDate = g.Max(p => p.PurchaseDate),
                    CurrencyName = g.FirstOrDefault(p => p.PurchaseDate == g.Max(d => d.PurchaseDate)).Currency.CurrencyName
                })
                .ToList();

            var selectableMedicines = query.Select(item => new SelectListItem
            {
                Text = $"{item.TradeName} | {item.Capacity} | {item.CompanyName} | {item.CurrencyName}",
                Value = $"{item.TradeName}_{item.Capacity}_{item.CompanyName}_{item.CurrencyName}_{item.Amount}_{item.SalePrice}"
            })
                .ToList();
            ViewBag.medicines = selectableMedicines;

            var currency = _context.Currencies
                .Select(c => new SelectListItem()
                {
                    Text = c.CurrencyName,
                    Value = c.CurrencyID.ToString()
                })
                .ToList();
            ViewBag.currency = currency;

            var customer = _context.Customers
                .Select(c => new SelectListItem()
                {
                    Text = c.CustomerName,
                    Value = c.CustomerID.ToString()
                })
                .ToList();
            ViewBag.customer = customer;
            return View();
        }
        [HttpPost]
        public IActionResult Create(SaleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //Sale sale = _mapper.Map<Sale>(viewModel);

               Sale sale = new Sale
               {
                  SaleAmount = viewModel.SaleAmount,
                  SaleDate = viewModel.SaleDate,
                  SaleID = viewModel.SaleID,
                  Paid  = viewModel.Paid,
                  UnitPrice = viewModel.UnitPrice,
                  Unpaid    = viewModel.Unpaid,
                  MedicineID = viewModel.MedicineID,
                  CurrencyID = viewModel.CurrencyID,
                  CustomerID    = viewModel.CustomerID,
                  TotalPrice = viewModel.TotalPrice,
               };
                _context.Sales.Add(sale);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }
        public IActionResult Details(int? id)
        {
            var query = _context.Sales.Include(m => m.Medicine).Include(s => s.Customer).Include(c => c.Currency).ToList();
            if (id == null)
            {
                return NotFound();
            }
            var purchase = _context.Sales.FirstOrDefault(p => p.SaleID == id);
            if (purchase == null)
            {
                return NotFound();
            }
            return View(purchase);
        }
    }
}
