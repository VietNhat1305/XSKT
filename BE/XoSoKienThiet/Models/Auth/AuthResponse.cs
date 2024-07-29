using XoSoKienThiet.Models.Core;
using XoSoKienThiet.ViewModels;

namespace XoSoKienThiet.Models.Auth
{
    public class AuthResponse
    {
        public UserLogin User { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsRequireVerify { get; set; }
        public bool IsRequireChangePassword { get; set; }
        
        
        public AuthResponse(UserLogin user, string accessToken, string refreshToken)
        {
            AccessToken = AccessToken;
            RefreshToken = refreshToken;
            User = user;
            UserId = user.Id;
            UserName = user.UserName;
        }

        public AuthResponse(UserLogin user)
        {
            this.User = user;
        }
    }

    public class UserLogin
    {
        public UserLogin() { }
        public UserLogin(User model , DateTime expiryDate )
        {
            this.Id = model.Id;
            this.UserName = model.UserName;
            this.FullName = model.FullName;
            this.Email = model.Email;
            this.ExpiryDate = expiryDate;
            this.Menu = model.Menu;
            this.Avatar = model.Avatar;
            this.ListAction = (model.UnitRole != null && model.UnitRole.ListAction != default) ? model.UnitRole.ListAction : null ;
        }
        public string Id { get; set; }
        public string UserName { get; set; }
        
        public string AccessToken { get; set; }
        
        public string RefreshToken { get; set; }
        
        public DateTime  ExpiryDate { get; set; }
        public string FullName { get; set; }
        
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    
        public IEnumerable<string> Permissions { get; set; }
        public string Avatar { get; set; }
        
        
        
        public  List<NavMenuVM> Menu { get; set; }
        
        public List<string> ListAction { get; set; } = new List<string>(); // Danh sách Menu mà quyền này có 
        
        
        public string Address { get; set;  }
    }

}