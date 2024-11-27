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

namespace GroupBudget_Web.Controllers
{
    [Authorize]
    public class ProjectMembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectMembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjectMembers
        public async Task<IActionResult> Index(int projectId)
        {
            var applicationDbContext = _context.ProjectMembers
                                                .Where(pm => pm.ProjectId == projectId && pm.deleted > DateTime.Now)
                                                .Include(p => p.Member)
                                                .Include(p => p.Project);
            ViewData["ProjectId"] = projectId;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProjectMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectMember = await _context.ProjectMembers
                .Include(p => p.Member)
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectMember == null)
            {
                return NotFound();
            }

            return View(projectMember);
        }

        // GET: ProjectMembers/Create
        public IActionResult Create(int projectId)
        {
            GroupBudgetUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            ProjectMember pm = new ProjectMember{ DoneById = user.Id, ProjectId = projectId};
            ViewData["MemberId"] = new SelectList(_context.Users.Where(u => u.UserName != "?"), "Id", "UserName");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Description");
            return View(pm);
        }

        // POST: ProjectMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjectId,MemberId,BecameMemberDate,DoneById,Remark,deleted")] ProjectMember projectMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Users.Where(u=>u.UserName != "?"), "Id", "UserName", projectMember.MemberId);
            return View(projectMember);
        }

        // GET: ProjectMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectMember = await _context.ProjectMembers.FindAsync(id);
            if (projectMember == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_context.Users, "Id", "Id", projectMember.MemberId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Description", projectMember.ProjectId);
            return View(projectMember);
        }

        // POST: ProjectMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProjectId,MemberId,BecameMemberDate,DoneById,Remark,deleted")] ProjectMember projectMember)
        {
            if (id != projectMember.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectMemberExists(projectMember.Id))
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
            ViewData["MemberId"] = new SelectList(_context.Users, "Id", "Id", projectMember.MemberId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Description", projectMember.ProjectId);
            return View(projectMember);
        }

        // GET: ProjectMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectMember = await _context.ProjectMembers
                .Include(p => p.Member)
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectMember == null)
            {
                return NotFound();
            }

            return View(projectMember);
        }

        // POST: ProjectMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectMember = await _context.ProjectMembers.FindAsync(id);
            if (projectMember != null)
            {
                _context.ProjectMembers.Remove(projectMember);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectMemberExists(int id)
        {
            return _context.ProjectMembers.Any(e => e.Id == id);
        }
    }
}
