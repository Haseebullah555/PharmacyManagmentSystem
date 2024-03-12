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
        private ApplicationDbContext context;
        private IMapper mapper;

        public SalesController(ApplicationDbContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {

            var query = context.Sales.Include(m=>m.Medicine).Include(c=>c.Customer).Include(cr=>cr.Currency).ToList();
           
            return View(query);
        }
        [HttpGet]
        public IActionResult Create()
        {
            

            var medicine = context.Medicines.Select(x => new SelectListItem()
            {
                Text = x.TradeName+" | "+x.Capacity+" | "+x.Category.CategoryName,
                Value = x.MedicineID.ToString()

            }).ToList();
            ViewBag.medicine = medicine;




            var currency = context.Currencies.Select(x => new SelectListItem()
            {
                Text = x.CurrencyName,
                Value = x.CurrencyID.ToString()

            }).ToList();
            ViewBag.currency = currency;


            var customer = context.Customers.Select(x => new SelectListItem()
            {
                Text = x.CustomerName,
                Value = x.CustomerID.ToString()

            }).ToList();
            ViewBag.customer = customer;


            return View();
        }
        [HttpPost]
        public IActionResult Create(SaleViewModel viewModel)
        {
            Sale sale = mapper.Map<Sale>(viewModel);
            if (ModelState.IsValid)
            {
                context.Sales.Add(sale);
                context.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(viewModel);
        }
    }
}
