using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace wardalert.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly string _connectionString = "Server=localhost;Database=project;User=root;Password=;";

        [BindProperty]
        public string Trainingtitle { get; set; }
        [BindProperty]
        public string Trainingdescription { get; set; }
        [BindProperty]
        public string Date{ get; set; }
        [BindProperty]
        public string Time{ get; set; }

        public void OnPost()
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "INSERT INTO training (Trainingtitle,Trainingdescription,Date,Time) VALUES (@Trainingtitle, @Trainingdescription, @Date, @Time)";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Trainingtitle", Trainingtitle);
            command.Parameters.AddWithValue("@Trainingdescription", Trainingdescription);
            command.Parameters.AddWithValue("@Date", Date);
            command.Parameters.AddWithValue("@Time", Time);

            command.ExecuteNonQuery();
            connection.Close();



        }

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
 
