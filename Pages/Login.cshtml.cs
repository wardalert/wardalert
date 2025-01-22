using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace wardalert.Pages.Admin
{
    public class LoginModel : PageModel
    {
        [BindProperty]

        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public bool IsLoggedIn { get; set; } = false;
        public string ErrorMessage;
        public IActionResult OnPost()
        {
            if (Email == "kri@gmail.com" && Password == "krisha123")
            {
                HttpContext.Session.SetString("Login", "true"); // Set session value
                return RedirectToPage("/Dashboard");
            }

            HttpContext.Session.SetString("Login", "false"); // Set session value
            return Page();
        }
        public IActionResult OnPostLogout()
        {
            // Clear session data when logging out
            HttpContext.Session.Clear();

            // Redirect to Admin or Login page after logout
            return RedirectToPage("/index");
        }
        public void OnGet()
        {
            // Retrieve login status from session
            //var loginStatus = HttpContext.Session.GetString("Login");
            //IsLoggedIn = loginStatus == "true"; // Set property for use in the view
        }
    }
}
