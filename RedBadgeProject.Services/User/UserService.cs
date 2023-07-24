using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RedBadgeProject.Data;
using RedBadgeProject.Data.Entities;
using RedBadgeProject.Models.User;

namespace RedBadgeProject.Services.User
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _ctx;
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;



        public UserService(
            AppDbContext ctx,
            UserManager<UserEntity> userManager,
            SignInManager<UserEntity> signInManager)
            {
                _ctx = ctx;
                _userManager = userManager;
                _signInManager = signInManager;
            }

            public async Task<bool>RegisterUserAsync(UserRegister model)
            {
                if (await UserExistsAsync(model.Email, model.Email))
                return false;

                UserEntity user = new()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Address = model.Address,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };

                var createResult = await _userManager.CreateAsync(user, model.Password);
                return createResult.Succeeded;
            }

            public async Task<bool> LoginAsync(UserLogin model)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is null)
                return false;

                var isValidPassword = await _userManager.CheckPasswordAsync(user,model.Password);
                if (isValidPassword == false)
                return false;

                await _signInManager.SignInAsync(user, true);
                return true;
            }

            public async Task LogoutAsync() => _signInManager.SignOutAsync();

            private async Task<bool> UserExistsAsync(string email, string username)
            {
                var normalizedEmail = _userManager.NormalizeEmail(email);
                var normalizedUsername = _userManager.NormalizeName(username);

                return await _ctx.Users.AnyAsync(u =>
                u.NormalizedEmail == normalizedEmail ||
                u.NormalizedUserName == normalizedUsername
                );
            }

        Task<bool> IUserService.LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegisterUserAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> LoginAsync()
        {
            throw new NotImplementedException();
        }
    }
}
