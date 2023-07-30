using RedBadgeProject.Models.User;
using RedBadgeProject.Services.User;
using RedBadgeProject.Models.AccountStatus;
using RedBadgeProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace RedBadgeProject.WebMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAccountStatusService _accountStatusService;

        public AccountController(IUserService userService, IAccountStatusService accountStatusService)
        {
            _userService = userService;
            _accountStatusService = accountStatusService;
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult UserPage()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegister model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.PasswordHash != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                return View(model);
            }

            var registerResult = await _userService.RegisterUserAsync(model);
            if (!registerResult)
            {
                return View(model);
            }

            var user = await _userService.GetUserByEmailAsync(model.Email);
            if (user != null)
            {
                var accountStatusModel = new AccountStatusModel
                {
                    UserId = user.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address?.Trim(),
                    Email = model.Email,
                    Subscription = "Free" 
                };

                await _accountStatusService.CreateAccountStatusAsync(accountStatusModel);
            }

            UserLogin loginModel = new UserLogin
            {
                Email = model.Email,
                Password = model.PasswordHash
            };

            await _userService.LoginAsync(loginModel);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLogin model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var loginResult = await _userService.LoginAsync(model);
            if (!loginResult)
            {
                ModelState.AddModelError(string.Empty, "Incorrect email or password.");
                return View(model);
            }

            return RedirectToAction("UserPage", "Account"); 
        }

        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}

