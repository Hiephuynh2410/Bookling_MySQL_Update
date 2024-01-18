using Microsoft.AspNetCore.Mvc;

namespace Booking.Areas.Admin
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index() 
        {
            return  View();
        }
    }
}
