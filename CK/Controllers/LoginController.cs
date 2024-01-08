using CK.Model;
using CK.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace CK.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly CkhelperdbContext _dbContext;
        private static readonly List<Helpersuser> Users = new List<Helpersuser>();
        public LoginController(ILogger<LoginController> logger, CkhelperdbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                // Clear existing session when displaying the login page
                HttpContext.Session.Clear();

                return RedirectToAction("Index", "Home");
            }

            return View();
        }


        //[HttpPost]
        //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        //public async Task<IActionResult> Login(VMLogin modellogin)
        //{
        //    if (HttpContext.Request.Query.ContainsKey("preventBack"))
        //    {
        //        // Clear authentication cookies if any (extra measure)
        //        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //        // Clear session on login
        //        HttpContext.Session.Clear();
        //    }

        //    if (string.IsNullOrWhiteSpace(modellogin.username) || string.IsNullOrWhiteSpace(modellogin.Password))
        //    {
        //        TempData["ValidateMessage"] = "Username and password are required.";
        //        return View("Login");
        //    }

        //    var authenticatedUser = _dbContext.Helpersusers
        //        .FirstOrDefault(u => u.Username == modellogin.username && u.Password == modellogin.Password);

        //    if (authenticatedUser != null)
        //    {
        //        List<Claim> claims = new List<Claim>
        //{
        //    new Claim(ClaimTypes.NameIdentifier, authenticatedUser.Username),
        //    new Claim("OtherProperties", "example role")
        //};

        //        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //        AuthenticationProperties properties = new AuthenticationProperties
        //        {
        //            AllowRefresh = true,
        //        };

        //        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

        //        // Store username in session for future use
        //        HttpContext.Session.SetString("Username", authenticatedUser.Username);

        //        return RedirectToAction("Index", "Home");
        //    }

        //    if (HttpContext.Session.GetString("LoggedOut") == "true")
        //    {
        //        TempData["PreventBack"] = true;
        //        HttpContext.Session.SetString("LoggedOut", "false"); // Reset the session variable
        //    }

        //    return View();
        //}
        [HttpPost]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Login(VMLogin modellogin)
        {
            if (HttpContext.Request.Query.ContainsKey("preventBack"))
            {
                // Clear authentication cookies if any (extra measure)
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                // Clear session on login
                HttpContext.Session.Clear();
            }

            if (string.IsNullOrWhiteSpace(modellogin.username) || string.IsNullOrWhiteSpace(modellogin.Password))
            {
                TempData["ValidateMessage"] = "Username and password are required.";
                return View("Login");
            }

            var authenticatedUser = _dbContext.Helpersusers
                .FirstOrDefault(u => u.Username == modellogin.username && u.Password == modellogin.Password);

            if (authenticatedUser != null)
            {
                // Create claims for the authenticated user
                List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, authenticatedUser.Username),
                new Claim("OtherProperties", "example role")
            };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                };

                // Sign in the user
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

                // Store username in session for future use
                HttpContext.Session.SetString("Username", authenticatedUser.Username);

                return RedirectToAction("Index", "Home");
            }

            if (HttpContext.Session.GetString("LoggedOut") == "true")
            {
                TempData["PreventBack"] = true;
                HttpContext.Session.SetString("LoggedOut", "false"); // Reset the session variable
            }

            return View();
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

