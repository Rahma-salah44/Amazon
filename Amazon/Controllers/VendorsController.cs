#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Amazon.Data;
using Amazon.Models;
using Amazon.Data.Services;

namespace Amazon.Controllers
{
    public class VendorsController : Controller
    {
        // private readonly AmazonDbContext _context;
        private readonly IVendorServices _service;
        public VendorsController(IVendorServices service)//AmazonDbContext context)
        {
            _service = service;
            // _context = context;
        }

        // GET: Vendors

        public async Task<IActionResult> index()
        {
            return View(await _service.GetAllAsync());
        }


        public async Task<IActionResult> search(string searchString)
        {
            var allVendors = await _service.GetAllAsync();//_context.Vendor.ToListAsync();

            if (!string.IsNullOrEmpty(searchString))
            {


                var filteredResultNew = allVendors.Where(n => string.Equals(n.Name, searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();

                return View("Index", filteredResultNew);
            }
            return View(allVendors);


        }

        //// GET: Vendors/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await _service.GetByIdAsync(id);//_context.Vendor
                                                         //.FirstOrDefaultAsync(m => m.Id == id);
            if (vendor == null)
            {
                return NotFound();
            }

            return View(vendor);
        }

        // GET: Vendors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vendors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Location,Phone")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(vendor);
                //await _context.SaveChangesAsync();
                _service.AddAsync(vendor);
                return RedirectToAction(nameof(Index));
            }
            return View(vendor);
        }

        // GET: Vendors/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await _service.GetByIdAsync(id);//_context.Vendor.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }
            return View(vendor);
        }

        // POST: Vendors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location,Phone")] Vendor vendor)
        {
            if (id != vendor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _service.UpdateAsync(vendor.Id, vendor);
                    //_context.Update(vendor);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Vendor vendor = await _service.GetByIdAsync(id);
                    if (!VendorExists(vendor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vendor);
        }

        // GET: Vendors/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await _service.GetByIdAsync(id);//_context.Vendor
                                                         //.FirstOrDefaultAsync(m => m.Id == id);
            if (vendor == null)
            {
                return NotFound();
            }

            return View(vendor);
        }

        // POST: Vendors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var vendor = await _service.GetByIdAsync(id);//_context.Vendor.FindAsync(id);
            await _service.DeleteAsync(id);
            //_context.Vendor.Remove(vendor);
            //await _service.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendorExists(int id)
        {
            var lst = _service.GetByIdAsync(id);

            if (lst != null)
                return true;

            return false;
            //_context.Vendor.Any(e => e.Id == id);
        }
    }
}