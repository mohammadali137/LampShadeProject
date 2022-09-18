using _0_Framework.Domain;
using AccountManagement.Domain.RoleAgg;
using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Domain.AccountAgg
{
    public class Account : EntityBase
    {
        public string Fullname { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Mobile { get; private set; }
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; private set; }
        public long RoleId { get; private set; }
        public Role Role { get; private set; }
        public string ProfilePhoto { get; private set; }

        public Account(string fullname, string username, string password, string mobile,
            long roleId, string profilePhoto,string email)
        {
            Fullname = fullname;
            Username = username;
            Password = password;
            Mobile = mobile;
            RoleId = roleId;
            Email = email;
            if (roleId == 0)
                RoleId = 2;
            
            ProfilePhoto = profilePhoto;
        }

        public void Edit(string fullname, string username, string mobile,
            long roleId, string profilePhoto,string email)
        {
            Fullname = fullname;
            Username = username;
            Mobile = mobile;
            RoleId = roleId;
            Email = email;
            if (!string.IsNullOrWhiteSpace(profilePhoto))
                ProfilePhoto = profilePhoto;
        }

        public void ChangePassword(string password)
        {
            Password = password;
        }
    }
}
