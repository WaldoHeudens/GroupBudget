using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupBudget_Web.Data;
using GroupBudget_Web.Models;
using Microsoft.AspNetCore.Authorization;
using GroupBudget_Web.Data.Migrations;
using GroupBudget_Web.Services;

namespace GroupBudget_Web.Controllers
{
    [Authorize (Roles = "SystemAdmin")]
    public class ParametersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GroupBudgetUser _user;

        public ParametersController(ApplicationDbContext context, IMyUser user)
        {
            _context = context;
            _user = user.User;
        }

        // GET: Parameters
        public async Task<IActionResult> Index()
        {
            return View(await _context.Parameters.ToListAsync());
        }


        //// GET: Parameters/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var parameter = await _context.Parameters.FindAsync(id);
        //    if (parameter == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(parameter);
        //}

        //// POST: Parameters/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("Name,Value,Description,UserId,LastChanged,Obsolete,Destination")] Parameter parameter)
        //{
        //    if (id != parameter.Name)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {   
        //            parameter.LastChanged = DateTime.Now;
        //            parameter.UserId = _user.Id;
        //            _context.Update(parameter);
        //            await _context.SaveChangesAsync();
        //            Parameter.Parameters[parameter.Name] = parameter;
        //            if (parameter.Destination == "Mail")
        //                Parameter.ConfigureMail();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ParameterExists(parameter.Name))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(parameter);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _EditPartial(string name, string value)
        {
            Parameter parameter = _context.Parameters.First(p => p.Name == name);
            parameter.Value = value;
            parameter.LastChanged = DateTime.Now;
            parameter.UserId = _user.Id;
            _context.Update(parameter);
            Parameter.Parameters[parameter.Name] = parameter;
            if (parameter.Destination == "Mail")
                Parameter.ConfigureMail();
            await _context.SaveChangesAsync();
            return PartialView("_EditPartial", parameter);
        }

        //private bool ParameterExists(string id)
        //{
        //    return _context.Parameters.Any(e => e.Name == id);
        //}
    }
}
