using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Booking.Areas.Admin
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        DlctContext db = new DlctContext();
        private readonly HttpClient _httpClient;
        public ProductController()
        {
            _httpClient = new HttpClient();
        }

        //View
        public async Task<IActionResult> Index()
        {

            var apiResponse = await _httpClient.GetAsync("http://localhost:5196/api/ProductApi/");
            if (apiResponse.IsSuccessStatusCode)
            {
                var responseContent = await apiResponse.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<Product>>(responseContent);

                return View(products);
            }
            else
            {
                var productsList = await db.Products
                   .Include(s => s.ProductType)
                   .Include(s => s.Provider)
                   .ToListAsync();
                return View(productsList);
            }
        }
    }
}
