using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlayStudioHugoR.Models;
using PlayStudiosAssesmentFront.Controllers.Models;

namespace PlayStudiosAssesmentFront.Controllers
{
    public class HomeController : Controller
    {


        public IActionResult LoginError(string message)
        {
            TempData["ErrorLogin"] = message;
            return RedirectToPage("/Login");
        }
        // POST: HomeController/Create
        [HttpPost]
        public async Task<IActionResult> Login(string Username, string Password)
        {
            try
            {
                LoginModel user = new LoginModel { Username = Username, Password = Password};
                var client  = new HttpClient();
                var endpoint = "https://localhost:7155/User/Login";
                var response = await client.PostAsJsonAsync(endpoint, user);
                var message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("LoginError", new {message = message});
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // POST: HomeController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Register(UsersModel usersModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var client = new HttpClient();
                    var endpoint = "https://localhost:7155/User/Insert";
                    var response = await client.PostAsJsonAsync(endpoint, usersModel);
                    var message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return RedirectToPage("/Register");
                }
                return View(usersModel);
                //return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
