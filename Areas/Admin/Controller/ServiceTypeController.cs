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
                var Servicetype = JsonConvert.DeserializeObject<List<Servicetype>>(responseContent);

                return View(Servicetype);
            }
            else
            {
                var Servicetype = await db.Servicetypes
                   .ToListAsync();
                return View(Servicetype);
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
                return Json(new { success = true, message = "service type created successfully" });
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
         
         //Delete
        public async Task<IActionResult> Delete(int ServiceTypeId)
        {
            var apiUrl = $"http://localhost:5196/api/ServiceTypeApi/delete/{ServiceTypeId}";

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

                return Json(new { success = false, messag = "Failed to delete ServiceType" });

            }
        }

        //edit
        [HttpGet]
        public IActionResult Edit(int serviceTypeId)
        {
           
            var ServiceTypeId = db.Servicetypes.Find(serviceTypeId);

            if (ServiceTypeId == null)
            {
                return NotFound();
            }
            return View(ServiceTypeId);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int ServiceTypeId, Servicetype updateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(updateModel);
            }
          
            var apiUrl = $"http://localhost:5196/api/ServiceTypeApi/update/{ServiceTypeId}";

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
