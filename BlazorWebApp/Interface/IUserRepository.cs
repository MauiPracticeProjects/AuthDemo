using BlazorWebApp.Models;

namespace BlazorWebApp.Interface
{
    public interface IUserRepository
    {
        Task<ApiResponse> UserRegistration(UserModel model);

        Task<ApiResponse> LoginUser(UserLoginModel model);
    }
}
