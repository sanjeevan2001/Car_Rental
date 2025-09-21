using CarRental.Data;
using CarRental.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CarRental.Controllers
{
    public class AdminUserController : Controller
    {
        private readonly AppDbContext _dbContext;

        public AdminUserController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: AdminUser
        public async Task<IActionResult> Index()
        {
            var users = await _dbContext.Users.ToListAsync();
            return View(users);
        }

        // GET: AdminUser/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null) return NotFound();

            return View(user);
        }

        // GET: AdminUser/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: AdminUser/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserId,Username,Password")] User user)
        {
            if (id != user.UserId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Hash password if you want (recommended)
                    // user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                    _dbContext.Update(user);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        private bool UserExists(Guid id)
        {
            return _dbContext.Users.Any(e => e.UserId == id);
        }
    }
}
