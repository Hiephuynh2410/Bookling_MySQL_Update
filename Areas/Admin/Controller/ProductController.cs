using System.Text;
using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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


        //Delete
        public async Task<IActionResult> Delete(int productId)
        {
          
            var apiUrl = $"http://localhost:5196/api/ProductApi/delete/{productId}";

            var response = await _httpClient.DeleteAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("API Response Content: " + responseContent);

                var errorResponse = JsonConvert.DeserializeObject<object>(responseContent);

                string errorMessage = errorResponse?.ToString() ?? "An error occurred.";
                return Json(new { success = false, messag = "Failed to create product" });

            }
        }

       
        [HttpPost]
        public async Task<IActionResult> DeleteProducts([FromBody] List<int> productIds)
        {
            var apiUrl = "http://localhost:5196/api/ProductApi/deleteAll";

            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(productIds);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Products deleted successfully" });
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("API Response Content: " + responseContent);

                    var errorResponse = JsonConvert.DeserializeObject<object>(responseContent);
                    string errorMessage = errorResponse?.ToString() ?? "An error occurred.";

                    return Json(new { success = false, message = errorMessage });
                }
            }
        }

        //edit
        [HttpGet]
        public IActionResult Edit(int productId)
        {
           
            var product = db.Products.Find(productId);
            var ProductType = db.Producttypes.ToList();
            var providers = db.Providers.ToList();

            ViewBag.ProductType = new SelectList(ProductType, "ProductTypeId", "Name");
            ViewBag.providers = new SelectList(providers, "ProviderId", "Name");
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int productId, Product updateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(updateModel);
            }
          
            var apiUrl = $"http://localhost:5196/api/ProductApi/update/{productId}";

            var json = JsonConvert.SerializeObject(updateModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("API Response Content: " + responseContent);

                var errorResponse = JsonConvert.DeserializeObject<object>(responseContent);

                string errorMessage = errorResponse?.ToString() ?? "An error occurred.";

                return View(updateModel);
            }
        }     

        //create
        public IActionResult Create()
        {
         
            var ProductType = db.Producttypes.ToList();
            var providers = db.Providers.ToList();

            ViewBag.ProductType = new SelectList(ProductType, "ProductTypeId", "Name");
            ViewBag.providers = new SelectList(providers, "ProviderId", "Name");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product registrationModel)
        {
            var apiUrl = "http://localhost:5196/api/ProductApi/create";
            if (!ModelState.IsValid)
            {
                var ProductType = db.Producttypes.ToList();
                var providers = db.Providers.ToList();
                ViewBag.ProductType = new SelectList(ProductType, "ProductTypeId", "Name");
                ViewBag.providers = new SelectList(providers, "ProviderId", "Name");

                return View(registrationModel);
            }

            var json = JsonConvert.SerializeObject(registrationModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Product created successfully" });
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("API Response Content: " + responseContent);

                var errorResponse = JsonConvert.DeserializeObject<object>(responseContent);
                
                string errorMessage = errorResponse?.ToString() ?? "An error occurred.";

                var ProductType = db.Producttypes.ToList();
                var providers = db.Providers.ToList();
                ViewBag.ProductType = new SelectList(ProductType, "ProductTypeId", "Name");
                ViewBag.providers = new SelectList(providers, "ProviderId", "Name");
                return Json(new { success = false, messag = "Failed to create product" });

            }
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
