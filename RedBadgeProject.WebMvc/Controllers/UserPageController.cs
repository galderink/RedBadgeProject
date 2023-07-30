using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedBadgeProject.Models.Class;
using RedBadgeProject.Services.Class;

namespace RedBadgeProject.WebMvc.Controllers
{
    public class UserPageController : Controller
    {
        private readonly IUserClassService _userClassService;

        public UserPageController(IUserClassService userClassService)
        {
            _userClassService = userClassService;
        }

        public async Task<IActionResult> UserPage()
        {
            List<AvailableClassModel> availableClasses = await GetAvailableClasses();
            return View(availableClasses);
        }

        private async Task<List<AvailableClassModel>> GetAvailableClasses()
        {
            List<UserClassModel> userClasses = await _userClassService.GetAllUserClasses();
            List<AvailableClassModel> availableClasses = ConvertToAvailableClassModels(userClasses);

            return availableClasses;
        }

        private List<AvailableClassModel> ConvertToAvailableClassModels(List<UserClassModel> userClasses)
        {
            List<AvailableClassModel> availableClasses = new List<AvailableClassModel>();
            foreach (var userClass in userClasses)
            {
                AvailableClassModel availableClass = new AvailableClassModel
                {
                    Id = userClass.Id,
                    Zoomba = userClass.Name,
                    Yoga = userClass.Email,
                };
                availableClasses.Add(availableClass);
            }

            return availableClasses;
        }
    }
}
