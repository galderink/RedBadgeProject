using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedBadgeProject.Models.Class;
using RedBadgeProject.Models.Messages;
using RedBadgeProject.Services.Class;

namespace RedBadgeProject.WebMvc.Controllers
{
    public class UserClassController : Controller
    {
        private readonly IUserClassService _userClassService;

        public UserClassController(IUserClassService userClassService)
        {
            _userClassService = userClassService;
        }

        [HttpPost]
public async Task<IActionResult> SendMessage(MessagesModel message)
{
    if (ModelState.IsValid)
    {
        message.SenderId = 1;
        await _userClassService.SendUserMessage(message);
        return RedirectToAction("Inbox");
    }
    return View(message);
}

        public async Task<IActionResult> UsersClass()
        {
            List<UserClassModel> userClasses = await _userClassService.GetAllUserClasses();
            return View("UsersClass", userClasses);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserClassModel userClass)
        {
            if (ModelState.IsValid)
            {
                bool isUserClassCreated = await _userClassService.CreateUserClass(userClass);

                if (isUserClassCreated)
                {
                    return RedirectToAction("UserClasses");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to create the user class.");
                    return View(userClass);
                }
            }

            return View(userClass);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool isUserClassDeleted = await _userClassService.DeleteUserClass(id);
            if (isUserClassDeleted)
            {
                return RedirectToAction("UserClasses");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(int userClassId, string fullName, string email, string selectedClass)
        {
            return RedirectToAction("UserPage", "Account");
        }
    }
}




