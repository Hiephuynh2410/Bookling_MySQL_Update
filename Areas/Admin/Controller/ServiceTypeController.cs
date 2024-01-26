using System.Text;
using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Booking.Areas.Admin
{
    [Area("Admin")]
    public class ServiceTypeController : Controller
    {
        DlctContext db = new DlctContext();
        private readonly HttpClient _httpClient;
       
        public ServiceTypeController()
        {
            _httpClient = new HttpClient();
        }

         //View
        public async Task<IActionResult> Index()
        {

            var apiResponse = await _httpClient.GetAsync("http://localhost:5196/api/ServiceTypeApi/");
            if (apiResponse.IsSuccessStatusCode)
            {
                var responseContent = await apiResponse.Content.ReadAsStringAsync();
                var servicetypes = JsonConvert.DeserializeObject<List<Servicetype>>(responseContent);

                return View(servicetypes);
            }
            else
            {
                var servicetypesList = await db.Servicetypes
                   .ToListAsync();
                return View(servicetypesList);
            }
        }

         //Delete
        public async Task<IActionResult> Delete(int serviceTypeId)
        {
            var apiUrl = $"http://localhost:5196/api/ServiceTypeApi/delete/{serviceTypeId}";

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

        //Delte All
        [HttpPost]
        public async Task<IActionResult> DeleteServiceType([FromBody] List<int> serviceTypeId)
        {
            var apiUrl = "http://localhost:5196/api/ServiceTypeApi/deleteAll/";

            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(serviceTypeId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "serviceType deleted successfully" });
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
    
        //create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Servicetype registrationModel)
        {
            var apiUrl = "http://localhost:5196/api/ServiceTypeApi/create";
            if (!ModelState.IsValid)
            {
                return View(registrationModel);
            }

            var json = JsonConvert.SerializeObject(registrationModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Service type created successfully" });
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
   
        //edit
        [HttpGet]
        public IActionResult Edit(int serviceTypeId)
        {
           
            var serviceType = db.Servicetypes.Find(serviceTypeId);

            if (serviceType == null)
            {
                return NotFound();
            }
            return View(serviceType);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int serviceTypeId, Servicetype updateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(updateModel);
            }
          
            var apiUrl = $"http://localhost:5196/api/ServiceTypeApi/update/{serviceTypeId}";

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
    }
}
