using FrontEndAssesment.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

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

        public IActionResult NewPassword(string user)
        {
            TempData["User"] = user;
            return View();
        }

        public IActionResult EmailAlert()
        {
            return View();
        }

        public IActionResult UserLogged()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Usuario autenticado
                return View();
            }
            else
            {
                // Usuario no autenticado
                return Unauthorized("Usuario no autenticado");
            }
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
                        TempData["MessageError"] = message;
                        return View();
                    }
                    else
                    {

                        var claims = new[]
                                                    {
                                        new Claim("name", loginModel.Username)
                                            };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(identity));
                        return View("UserLogged");
                    }
                }
                return View();
            }
            catch(Exception ex)
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
                        TempData["MessageRegister"] = message;
                        return View();
                    }
                }
                TempData["MessageRegister"] = "User has registered";
                return View("Login");
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
                        return View();
                    }
                    else
                    {
                        return View("EmailAlert");
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