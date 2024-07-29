using XoSoKienThiet.Models.Core;
using XoSoKienThiet.Models.PagingParam;
using XoSoKienThiet.ViewModels;

namespace XoSoKienThiet.Interfaces.Core;

public interface IUserService
{
    
    Task<User> Create(User model);
    Task<User> Update(User model);
    
    Task<User> GetByUserName(string userName);  
    Task<User> ChangePassword(UserVM model);
    Task<User> ChangeProfile(User model);
    Task<dynamic> GetPaging(PagingParam pagingParam);

}