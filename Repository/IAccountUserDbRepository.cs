using Microsoft.AspNetCore.Identity;
using SMS_WebApplication.ViewModel;

namespace SMS_WebApplication.Repository
{
    public interface IAccountUserDbRepository
    {
        Task<bool> SignUpUserAsync(RegisterUserViewModel user);
        Task<string> SignInUserAsync(LoginUserViewModel loginUserViewModel);
        Task<string> GetApplicationUserId(string token);
    }
}
