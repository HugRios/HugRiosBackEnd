using FrontEndAssesment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FrontEndAssesment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UrlSettings _urlSettings;
        public HomeController(ILogger<HomeController> logger, UrlSettings urlSettings)
        {
            _logger = logger;
            _urlSettings = urlSettings;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult RecoverPassword()
        {
            return View();
        }

        public IActionResult NewPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    LoginModel user = new LoginModel { Username = loginModel.Username, Password = loginModel.Password };
                    var client = new HttpClient();
                    
                    var endpoint = _urlSettings.Login;
                    var response = await client.PostAsJsonAsync(endpoint, user);
                    var message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    if (!response.IsSuccessStatusCode)
                    {
                        return View();
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(UsersModel usersModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var client = new HttpClient();
                    var endpoint = _urlSettings.Insert;
                    var response = await client.PostAsJsonAsync(endpoint, usersModel);
                    var message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    if (!response.IsSuccessStatusCode)
                    {
                        return RedirectToPage("/Register");
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }


        [HttpPost]
        public async Task<IActionResult> RecoverPassword(UsersModel usersModel)
        {
            try
            {
                if (usersModel.email != null)
                {
                    var client = new HttpClient();
                    var endpoint = _urlSettings.Recover;
                    var response = await client.PostAsJsonAsync(endpoint, usersModel.email);
                    var message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    if (!response.IsSuccessStatusCode)
                    {
                        TempData["ErrorLogin"] = message;
                       View();
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }


        [HttpPost]
        public async Task<IActionResult> NewPassword(UsersModel usersModel)
        {
            try
            {
                usersModel.email = "huguitorude@gmail.com";
                usersModel.full_name = "";
                if (usersModel.password != null)
                {
                    var client = new HttpClient();
                    var endpoint = _urlSettings.ChangePass;
                    var response = await client.PostAsJsonAsync(endpoint, usersModel);
                    var message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    if (!response.IsSuccessStatusCode)
                    {
                        TempData["MessageChangePass"] = message;
                        View();
                    }
                    TempData["MessageChangePass"] = "Password has change. Try Login";
                    return View("Login");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}