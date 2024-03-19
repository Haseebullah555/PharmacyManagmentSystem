using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PharmacyManagmentSystem.Data;
using PharmacyManagmentSystem.Models;
using PharmacyManagmentSystem.ViewModel;

namespace PharmacyManagmentSystem.Controllers
{
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SalesController(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index()
        {

            var query = _context.Sales.Include(m=>m.Medicine).Include(c=>c.Customer).Include(cr=>cr.Currency).ToList();
            return View(query);
        }
        //private List<SelectListItem> GetMedicines()
        //{
        //    var medicineList = new List<SelectListItem>();
        //    List<Medicine> medicines = _context.Medicines.ToList();
        //    medicineList = medicines.Select(m=> new SelectListItem()
        //    {
        //        Text = m.TradeName + " | " + m.Capacity + " | " + m.Category.CategoryName + " | " + m.Company.CompanyName,
        //        Value = m.MedicineID.ToString()
        //    }).ToList();
        //    var defaultItem = new SelectListItem()
        //    {
        //        Text = "Select Medicine",
        //        Value = ""
        //    };
        //    medicineList.Insert(0, defaultItem);
        //    return medicineList;
        //}
        //private List<SelectListItem> GetAmount(int medicineId = 1)
        //{
        //    List<SelectListItem> AmountList = _context.Purchases.Where(a => a.MedicineID == medicineId).OrderBy(a => a.Amount).Select(a => new SelectListItem()
        //    {
        //        Text=a.Amount.ToString(),
        //        Value = a.Amount.ToString()
        //    }).ToList();
        //    var defaultItem = new SelectListItem()
        //    {
        //        Text = "Select Amount",
        //        Value = ""
        //    };
        //    AmountList.Insert(0,defaultItem);
        //    return AmountList;
        //}
        public IActionResult Create()
        {
            //Sale sale = new Sale();
            //ViewBag.medicineId = GetMedicines();
            //ViewBag.amount = GetAmount();
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

            var selectedMedicines = query.Select(item => new SelectListItem
            {
                Text = $"{item.TradeName} | {item.Capacity} | {item.CompanyName} | {item.CurrencyName}",
                Value = $"{item.TradeName}_{item.Capacity}_{item.CompanyName}"
            })
            .ToList();

ViewBag.medicines = selectedMedicines;

            //var defaultItem = new SelectListItem()
            //{
            //   Text = "--Select a Medicine",
            //    Value = ""
            //};
            //query.Insert(0, defaultItem);

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

            var amount = _context.Purchases
                .GroupBy(p => new { p.Medicine.TradeName, p.Medicine.Capacity, p.Medicine.Company.CompanyName, p.CurrencyID })
                .Select(g => new
                {
                    Amount = g.Sum(p => p.Amount)
                })
                .ToList();

            ViewBag.Amount = amount.Select(a => new SelectListItem
            {
                Text = a.Amount.ToString(),
                Value = a.Amount.ToString()
            }).ToList();

            //var salePriceQuery = _context.Purchases
            //    .GroupBy(p => new { p.Medicine.TradeName, p.Medicine.Capacity, p.Medicine.Company.CompanyName, p.CurrencyID })
            //    .Select(g => new
            //    {
            //        SalePrice = g.FirstOrDefault(p => p.PurchaseDate == g.Max(d => d.PurchaseDate)).SalePrice
            //    })
            //    .ToList();
            //ViewBag.SalePrice = salePriceQuery.Select(s => s.SalePrice).ToList();
            //var query = _context.Purchases
            //    .GroupBy(p => new { p.Medicine.TradeName, p.Medicine.Capacity, p.Medicine.Company.CompanyName, p.CurrencyID })
            //    .Select(g => new
            //    {
            //        Amount = g.Sum(p => p.Amount),
            //        SalePrice = g.FirstOrDefault(p => p.PurchaseDate == g.Max(d => d.PurchaseDate)).SalePrice
            //    })
            //    .ToList();
            //var amountList = query.Select(q => q.Amount).ToList();
            //var salePriceList = query.Select(q => q.SalePrice).ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(SaleViewModel viewModel)
        {
            var amount = _context.Purchases
            .Where(p => p.MedicineID == viewModel.MedicineID)
            .Sum(p => p.Amount);

            // Assign the amount to the Sale object
            viewModel.purchase.Amount = amount;
            Sale sale = _mapper.Map<Sale>(viewModel);
            if (ModelState.IsValid)
            {
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
        [HttpPost]
        [HttpPost]
        public IActionResult GetAmount(int medicineID)
        {
            var amount = _context.Purchases
                .Where(p => p.MedicineID.ToString() == medicineID.ToString())
                .Sum(p => p.Amount);

            return Json(amount);
        }
    }
}
