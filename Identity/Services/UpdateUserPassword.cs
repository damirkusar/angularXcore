using System.Threading.Tasks;
using Identity.Data.Models;
using Identity.Interface.Services;
using Identity.Interface.TransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Services
{
    public class UpdateUserPassword: IUpdateUserPassword
    {
        private readonly ILogger<UpdateUserPassword> logger;
        private readonly UserManager<ApplicationUser> userManager;

        public UpdateUserPassword(
            ILogger<UpdateUserPassword> logger,
            UserManager<ApplicationUser> userManager)
        {
            this.logger = logger;
            this.userManager = userManager;
        }

        public async Task<IdentityResult> UpdateAsync(string userId, UserPassword userPassword)
        {
            var applicationUser = await this.userManager.FindByIdAsync(userId);
            var result = await this.userManager.ChangePasswordAsync(applicationUser, userPassword.Password, userPassword.NewPassword);
            return result;
        }
    }
}