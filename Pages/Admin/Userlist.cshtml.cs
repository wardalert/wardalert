using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using static wardalert.Pages.trainings.UpcomingModel;

namespace wardalert.Pages.Admin
{
    public class UserlistModel : PageModel
    {
        private readonly string _connectionString = "Server=185.176.40.25;Database=4583979_wardalert;User=4583979_wardalert;Password=qTuCSS#qcS9r4P2;";
        public List<Userlist> Userlists { get; set; } = new List<Userlist>();

        public IActionResult OnGet()
        {
            var isLoggedIn = HttpContext.Session.GetString("Login");
            if (isLoggedIn==null ||isLoggedIn=="false")
            {
                return RedirectToPage("/Login");

            }
            

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT id, Name,Address,Phone, Gender FROM Userlist";

            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Userlists.Add(new Userlist
                {

                    Name = reader.GetString("Name"),
                    Address = reader.GetString("Address"),
                    Phone = reader.GetString("Phone"),
                    Gender = reader.GetString("Gender"),

                });
            }
            connection.Close();
            return Page();
        }
        public class Userlist
        {
            public int id { get; set; }
            public string Name { get; set; }
            public string Address{ get; set; }
            public string Phone{ get; set; }

            public string Gender { get; set; }

        }

    }
    
}
