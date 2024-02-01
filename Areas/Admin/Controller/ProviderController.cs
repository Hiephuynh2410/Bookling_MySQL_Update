using System.Text;
using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Booking.Areas.Admin
{
    [Area("Admin")]
    public class ProviderController : Controller
    {
        DlctContext db = new DlctContext();
        private readonly HttpClient _httpClient;
        public ProviderController()
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
        public IActionResult Edit(int providerId)
        {
           
            var provider = db.Providers.Find(providerId);
            if (provider == null)
            {
                return NotFound();
            }
            return View(provider);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int providerId, Provider updateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(updateModel);
            }
          
            var apiUrl = $"http://localhost:5196/api/ProviderApi/update/{providerId}";

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
        public async Task<IActionResult> Create(Provider registrationModel)
        {
            var apiUrl = "http://localhost:5196/api/ProviderApi/create";
            if (!ModelState.IsValid)
            {
                return View(registrationModel);
            }

            var json = JsonConvert.SerializeObject(registrationModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Provider created successfully" });
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("API Response Content: " + responseContent);

                var errorResponse = JsonConvert.DeserializeObject<object>(responseContent);
                
                string errorMessage = errorResponse?.ToString() ?? "An error occurred.";

               
                return Json(new { success = false, messag = "Failed to create Provider" });

            }
        }

        //View
        public async Task<IActionResult> Index()
        {

            var apiResponse = await _httpClient.GetAsync("http://localhost:5196/api/ProviderApi/");
            if (apiResponse.IsSuccessStatusCode)
            {
                var responseContent = await apiResponse.Content.ReadAsStringAsync();
                var providers = JsonConvert.DeserializeObject<List<Provider>>(responseContent);

                return View(providers);
            }
            else
            {
                var providersList = await db.Providers
                   .ToListAsync();
                return View(providersList);
            }
        }
    }
}
