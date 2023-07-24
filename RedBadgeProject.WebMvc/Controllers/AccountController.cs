using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedBadgeProject.Models.User;
using RedBadgeProject.Services.User;

namespace RedBadgeProject.WebMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegister model)
        {
            if(!ModelState.IsValid)
            {
                foreach(var e in ModelState.Values)
                Console.WriteLine(e.AttemptedValue);
                return View(model);
            }

            var registerResult = await _userService.RegisterUserAsync(model);
            if (registerResult == false)
            {
                return View(model);
            }

            UserLogin loginModel = new()
            {
                Email = model.Email,
                Password = model.Password
            };

            await _userService.LoginAsync(loginModel);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLogin model)
        {
            var loginResult = await _userService.LoginAsync(model);
            if (loginResult == false)
            {
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}