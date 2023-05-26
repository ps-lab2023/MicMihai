using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PhotoGalleryApp.Data;
using PhotoGalleryApp.Hubs;
using System.Security.Claims;

namespace PhotoGalleryApp.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<CommentHub> _hubContext;

        public NotificationsController(ApplicationDbContext context, IHubContext<CommentHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            var notifications = _context.Notifications.OrderByDescending(n => n.DateCreated).ToList();
            return View(notifications);
        }

        [HttpGet]
        public IActionResult UnviewedCount()
        {
            var count = _context.Notifications.Count(n => !n.IsViewed);
            return Ok(count);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAllAsViewed()
        {
            // Retrieve user's ID from the HttpContext
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var notifications = _context.Notifications.Where(n => n.UserId == userId && !n.IsViewed);
            foreach (var notification in notifications)
            {
                notification.IsViewed = true;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
