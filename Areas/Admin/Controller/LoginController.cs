using System.Text;
using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Booking.Areas.Admin
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        DlctContext db = new DlctContext();
        private readonly HttpClient _httpClient;
        public LoginController()
        {
            _httpClient = new HttpClient();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Staff loginModel)
        {
            var apiUrl = "http://localhost:5196/api/LoginApi/login"; 

            var json = JsonConvert.SerializeObject(loginModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home"); 
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<dynamic>(responseContent);
                string errorMessage = error.message;

                ModelState.AddModelError(string.Empty, errorMessage);
                return View("Login");
            }
        }
    }
}
