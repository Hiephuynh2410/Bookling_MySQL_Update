using System.Text;
using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Booking.Areas.Admin
{
    [Area("Admin")]
    public class ServicesController : Controller
    {
        DlctContext db = new DlctContext();
        private readonly HttpClient _httpClient;
        public ServicesController()
        {
            _httpClient = new HttpClient();
        }
        //View
        public async Task<IActionResult> Index()
        {

            var apiResponse = await _httpClient.GetAsync("http://localhost:5196/api/ServicesApi/");
            if (apiResponse.IsSuccessStatusCode)
            {
                var responseContent = await apiResponse.Content.ReadAsStringAsync();
                var services = JsonConvert.DeserializeObject<List<Service>>(responseContent);
                return View(services);
            }
            else
            {
                var ServicesList = await db.Services
                   .Include(s => s.ServiceType)
                   .ToListAsync();
                return View(ServicesList);
            }
        }
    }
}
