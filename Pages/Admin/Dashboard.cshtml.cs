using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace wardalert.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        

        public IActionResult OnGet()
        {
            var isLoggedIn = HttpContext.Session.GetString("Login");
            if(isLoggedIn==null ||isLoggedIn=="false")
            {
                return RedirectToPage("/Login");
            }
            return Page();
        }
    }
}
 
