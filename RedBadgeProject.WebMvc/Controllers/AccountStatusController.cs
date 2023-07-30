using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedBadgeProject.Models.AccountStatus;
using RedBadgeProject.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RedBadgeProject.WebMvc.Controllers
{
    public class AccountStatusController : Controller
    {
        private readonly IAccountStatusService _accountStatusService;

        public AccountStatusController(IAccountStatusService accountStatusService)
        {
            _accountStatusService = accountStatusService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AccountStatusDetail()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("AccountStatusDetail", "AccountStatus");
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var accountStatus = await _accountStatusService.GetAccountStatusByUserId(userId);

            if (accountStatus == null)
            {
                return NotFound();
            }

            return View(accountStatus);
        }
    }
}

