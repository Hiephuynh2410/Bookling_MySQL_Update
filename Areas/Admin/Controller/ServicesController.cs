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

        //DeleteMultiple
        [HttpPost]
        public async Task<IActionResult> DeleteServices([FromBody] List<int> servicesId)
        {
            var apiUrl = "http://localhost:5196/api/ServicesApi/deleteAll/";

            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(servicesId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "services deleted successfully" });
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
        public async Task<IActionResult> Delete(int serviceId)
        {
          
            var apiUrl = $"http://localhost:5196/api/ServicesApi/delete/{serviceId}";

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
                return Json(new { success = false, messag = "Failed to create services" });

            }
        }

        //edit
        [HttpGet]
        public IActionResult Edit(int serviceId)
        {
            var services = db.Services.Find(serviceId);
            var serviceType = db.Servicetypes.ToList();

            ViewBag.serviceType = new SelectList(serviceType, "ServiceTypeId", "Name");
            if (services == null)
            {
                return NotFound();
            }
            return View(services);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int serviceId, Service updateModel)
        {
            if (!ModelState.IsValid)
            {
                return View(updateModel);
            }
          
            var apiUrl = $"http://localhost:5196/api/ServicesApi/update/{serviceId}";

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
            var serviceTypes = db.Servicetypes.ToList();
            ViewBag.serviceTypes = new SelectList(serviceTypes, "ServiceTypeId", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Service registrationModel)
        {
            var apiUrl = "http://localhost:5196/api/ServicesApi/create";
            if (string.IsNullOrEmpty(registrationModel.Name) && string.IsNullOrEmpty(registrationModel.Price.ToString()))
            {
                ModelState.AddModelError("Name", "cannot be empty.");
                ModelState.AddModelError("Price", "cannot be empty.");
            }
            // if (registrationModel.Price <= 0)
            // {
            //     ModelState.AddModelError("Price", "Price must be greater than 0.");
            // }

            // if (string.IsNullOrEmpty(Request.Form["ServiceTypeId"]))
            // {
            //     ModelState.AddModelError("ServiceTypeId", "ServiceType is required.");
            // }
            if (ModelState.IsValid)
            {
                // int createdByUserId;
                // if (int.TryParse(HttpContext.Session.GetString("UserId"), out createdByUserId))
                // {
                //     registrationModel.CreatedBy = createdByUserId;
                // }
                registrationModel.Status = Request.Form["Status"] == "true";
                var json = JsonConvert.SerializeObject(registrationModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Service created successfully" });
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("API Response Content: " + responseContent);

                    dynamic errorResponse = JsonConvert.DeserializeObject(responseContent);

                    if (errorResponse != null)
                    {
                        if (!string.IsNullOrEmpty(errorResponse.Message))
                        {
                            ModelState.AddModelError("", errorResponse.Message);
                        }
                        if (errorResponse.Errors != null)
                        {
                            foreach (var error in errorResponse.Errors)
                            {
                                ModelState.AddModelError("", error.ToString());
                            }
                        }
                    }

                    var serviceTypes = db.Servicetypes.ToList();
                    ViewBag.serviceTypes = new SelectList(serviceTypes, "ServiceTypeId", "Name");

                    return Json(new { success = false, messag = "Failed to create Service" });

                }
            }
            else
            {
                var serviceTypes = db.Servicetypes.ToList();
                ViewBag.serviceTypes = new SelectList(serviceTypes, "ServiceTypeId", "Name");
                return View(registrationModel);
            }
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
                var servicesList = await db.Services
                   .Include(s => s.ServiceType)
                   .ToListAsync();
                return View(servicesList);
            }
        }
         
    }
}
