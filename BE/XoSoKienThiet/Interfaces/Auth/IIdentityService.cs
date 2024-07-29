using XoSoKienThiet.Models.Auth;
using XoSoKienThiet.Models.Core;

namespace XoSoKienThiet.Interfaces.Auth
{
    public interface IIdentityService
    {
        Task<User> Authenticate(AuthRequest model);
        Task<dynamic> LoginAsync(AuthRequest model);
   
    }
}