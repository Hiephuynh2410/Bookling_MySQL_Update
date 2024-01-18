using Microsoft.AspNetCore.Mvc;

namespace Booking.Areas.Admin
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index() 
        {
            return View();
        }
    }
}
