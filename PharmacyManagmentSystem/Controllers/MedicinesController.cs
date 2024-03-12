using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PharmacyManagmentSystem.Data;
using PharmacyManagmentSystem.Models;
using PharmacyManagmentSystem.ViewModel;

namespace PharmacyManagmentSystem.Controllers
{
    public class MedicinesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MedicinesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var query = _context.Medicines.Include(c => c.Category).Include(c => c.Company);
            var result = query.ToList();
            return View(result);
        }
        public IActionResult Create()
        {
            var category = _context.Categories.Select(c => new SelectListItem()
            {
                Text = c.CategoryName,
                Value = c.CategoryId.ToString()
            }).ToList();
            ViewBag.Categories = category;

            var company = _context.Companies.Select(c => new SelectListItem()
            {
                Text = c.CompanyName,
                Value = c.CompanyID.ToString()
            }).ToList();
            ViewBag.Companies = company;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MedicineViewModel viewModel,string Capacity, int CategoryID, int CompanyID, string TradeName, string GenericName)
        {
            bool medicineExsits = _context.Medicines.Any(m => m.Capacity == Capacity && m.CategoryID == CategoryID && m.CompanyID == CompanyID && m.TradeName == TradeName && m.GenericName == GenericName);
            if (ModelState.IsValid)
            {
                Medicine medicine = _mapper.Map<Medicine>(viewModel);
                if (medicineExsits)
                {
                    TempData["ExsitMessage"] = "Medicine already exists!";
                    return RedirectToAction("Index");
                }
                else
                {
                    _context.Medicines.Add(medicine);
                    _context.SaveChanges();
                    TempData["AddedMessage"] = "Medicine Added successfully!";
                    return RedirectToAction("Index");
                }
               
            }

            return View(viewModel);
        }
        public IActionResult Edit(int? id)
        {
            var category = _context.Categories.Select(c => new SelectListItem()
            {
                Text = c.CategoryName,
                Value = c.CategoryId.ToString()
            }).ToList();
            ViewBag.Categories = category;

            var company = _context.Companies.Select(c => new SelectListItem()
            {
                Text = c.CompanyName,
                Value = c.CompanyID.ToString()
            }).ToList();
            ViewBag.Companies = company;

            if (id == null)
            {
                return NotFound();
            }
            var medicine = _context.Medicines.FirstOrDefault(m => m.MedicineID == id);
            if (medicine == null)
            {
                return NotFound();
            }
            return View(medicine);
        }
        [HttpPost]
        public IActionResult Edit(Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                _context.Medicines.Update(medicine);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medicine);
        }

        public IActionResult Details(int? id)
        {
            var query = _context.Medicines.Include(c => c.Category).Include(c => c.Company);
            var result = query.ToList();
            var medicine = _context.Medicines.FirstOrDefault(m => m.MedicineID == id);
            if (medicine == null)
            {
                return NotFound();
            }
            return View(medicine);
        }
        public IActionResult Delete(int id)
        {
            var query = _context.Medicines.Include(c => c.Category).Include(c => c.Company);
            var result = query.ToList();
            var medicine = _context.Medicines.Find(id);
            if (medicine == null)
            {
                return NotFound();
            }

            return View(medicine);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmedDelete(int id)
        {
            var medicine = _context.Medicines.Find(id);
            if (medicine != null)
            {
                _context.Medicines.Remove(medicine);
            }
            _context.SaveChanges();
            TempData["DeletionMessage"] = "Medicine Deleted Successfully!";
            return RedirectToAction("Index");
        }

    }
}
