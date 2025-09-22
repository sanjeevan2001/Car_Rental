using CarRental.Data;
using CarRental.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class AdminFeedbackController : Controller
    {
        private readonly AppDbContext _context;

        public AdminFeedbackController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var feedbacks = _context.Feedbacks.OrderByDescending(f => f.CreatedAt).ToList();
            return View(feedbacks);
        }

        public IActionResult Details(Guid id)
        {
            var feedback = _context.Feedbacks.FirstOrDefault(f => f.Id == id);
            if (feedback == null) return NotFound();
            return View(feedback);
        }

        public IActionResult Delete(Guid id)
        {
            var feedback = _context.Feedbacks.FirstOrDefault(f => f.Id == id);
            if (feedback == null) return NotFound();
            return View(feedback);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var feedback = _context.Feedbacks.FirstOrDefault(f => f.Id == id);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
