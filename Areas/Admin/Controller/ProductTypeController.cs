using System.Text;
using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Booking.Areas.Admin
{
    [Area("Admin")]
    public class ProductTypeController : Controller
    {
        DlctContext db = new DlctContext();
        private readonly HttpClient _httpClient;
       
        public ProductTypeController()
        {
            _httpClient = new HttpClient();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductTypes([FromBody] List<int> productTypeId)
        {
            var apiUrl = "http://localhost:5196/api/ProductTypeApi/deleteAll/";

            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(productTypeId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "ProductType deleted successfully" });
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

        //Delete
        public async Task<IActionResult> Delete(int productTypeId)
        {
            var apiUrl = $"http://localhost:5196/api/ProductTypeApi/delete/{productTypeId}";

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

                return Json(new { success = false, messag = "Failed to delete productTypes" });

            }
        }
        
        //edit
        [HttpGet]
        public IActionResult Edit(int productTypeId)
        {
           
            var productType = db.Producttypes.Find(productTypeId);

            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int productTypeId, Producttype updateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(updateModel);
            }
          
            var apiUrl = $"http://localhost:5196/api/ProductTypeApi/update/{productTypeId}";

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
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Producttype registrationModel)
        {
            var apiUrl = "http://localhost:5196/api/ProductTypeApi/create";
            if (!ModelState.IsValid)
            {
                return View(registrationModel);
            }

            var json = JsonConvert.SerializeObject(registrationModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Product type created successfully" });
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("API Response Content: " + responseContent);

                var errorResponse = JsonConvert.DeserializeObject<object>(responseContent);
                
                string errorMessage = errorResponse?.ToString() ?? "An error occurred.";

                return Json(new { success = false, messag = "Failed to create product type" });

            }
        }

        //View
        public async Task<IActionResult> Index()
        {

            var apiResponse = await _httpClient.GetAsync("http://localhost:5196/api/ProductTypeApi/");
            if (apiResponse.IsSuccessStatusCode)
            {
                var responseContent = await apiResponse.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<Producttype>>(responseContent);

                return View(products);
            }
            else
            {
                var productsList = await db.Producttypes
                   .ToListAsync();
                return View(productsList);
            }
        }
    }
}
