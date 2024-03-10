using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PharmacyManagmentSystem.Data;
using PharmacyManagmentSystem.Models;

namespace PharmacyManagmentSystem.Controllers
{
    public class MedicinesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MedicinesController(ApplicationDbContext context) 
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Medicines.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Medicine medicine)
        {
            if (!ModelState.IsValid)
            {
                _context.Medicines.Add(medicine);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateDropdownLists();
            return View(medicine);
        }
        public IActionResult Edit(int? id)
        {
            if(id == null)
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
            if(ModelState.IsValid)
            {
                _context.Medicines.Update(medicine);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medicine);
        }
        
        public IActionResult Details(int? id)
        {
            var  medicine = _context.Medicines.FirstOrDefault(m=>m.MedicineID==id);
            if (medicine == null)
            {
                return NotFound();
            }
            return View(medicine);
        }
        public IActionResult Delete(int id)
        {
            var medicine = _context.Medicines.Find(id);
            if(medicine == null)
            {
                return NotFound();
            }

            return View(medicine);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmedDelete(int id)
        {
            var medicine = _context.Medicines.Find(id);
            if(medicine != null)
            {
                _context.Medicines.Remove(medicine);
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        private void PopulateDropdownLists()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Companies = _context.Companies.ToList();

            // Check if the lists are null before creating the SelectList objects
            if (ViewBag.Categories != null)
            {
                ViewBag.CategoryID = new SelectList(ViewBag.Categories, "CategoryId", "CategoryName");
            }
            else
            {
                ViewBag.CategoryID = new SelectList(Enumerable.Empty<SelectListItem>(), "Value", "Text");
            }

            if (ViewBag.Companies != null)
            {
                ViewBag.CompanyID = new SelectList(ViewBag.Companies, "CompanyID", "CompanyName");
            }
            else
            {
                ViewBag.CompanyID = new SelectList(Enumerable.Empty<SelectListItem>(), "Value", "Text");
            }
        }

    }
}
