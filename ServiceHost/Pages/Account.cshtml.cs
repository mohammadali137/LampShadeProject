using AccountManagement.Application.Contracts.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ServiceHost.Pages
{
    public class AccountModel : PageModel
    {
        [TempData]
        public string LoginMessage { get; set; }

        [TempData]
        public string RegisterMessage { get; set; }


        private readonly IAccountApplication _accountApplication;

        public AccountModel(IAccountApplication accountApplication)
        {
            _accountApplication = accountApplication;
        }

        public void OnGet()
        {


        }

        public IActionResult OnPostLogin(Login command)
        {
            var result = _accountApplication.Login(command);
            if (result.IsSuccedded)
            {
                TempData["SuccessMessage"] = "عملیات با موفقیت انجام شد";
                return RedirectToPage("/Index");
            }
            TempData["WarningMessage"] = "نام کاربری یا کلمه رمز اشتباه است";

            LoginMessage = result.Message;
            return Page();
        }

        public IActionResult OnGetLogout()
        {
            _accountApplication.Logout();
            return RedirectToPage("/Index");
        }

        public IActionResult OnPostRegister(RegisterAccount command)
        {
            if (ModelState.IsValid)
            {
                if (command.Password != command.rePassword)
                {
                    RegisterMessage = "پسورد و تکرار آن با هم مطابقت ندارند";
                    TempData["WarningMessage"] = "پسورد و تکرار آن با هم مطابقت ندارند";
                    return Page();

                }
                var result = _accountApplication.Register(command);
                if (result.IsSuccedded)
                {
                    TempData["SuccessMessage"] = "ثبت نام شما با موفقیت انجام شد لطفا وارد سایت شوید";
                    RegisterMessage = "ثبت نام شما با موفقیت انجام شد لطفا وارد سایت شوید";

                    return Page();
                }
                TempData["WarningMessage"] = "امکان ثبت رکورد تکراری وجود ندارد. لطفا مجدد تلاش بفرمایید";
                RegisterMessage = "امکان ثبت رکورد تکراری وجود ندارد. لطفا مجدد تلاش بفرمایید";
            }
            return Page();
        }
    }
}
