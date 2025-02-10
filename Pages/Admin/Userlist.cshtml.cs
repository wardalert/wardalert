using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using static wardalert.Pages.trainings.UpcomingModel;

namespace wardalert.Pages.Admin
{
    public class UserlistModel : PageModel
    {
        private readonly string _connectionString = "Server=localhost;Database=project;User=root;Password=;";
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

            string query = "SELECT id,Training, Name,Address,Phone FROM Userlist";

            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Userlists.Add(new Userlist
                {

                    Training= reader.GetString("Training"),
                    Name = reader.GetString("Name"),
                    Address = reader.GetString("Address"),
                    Phone = reader.GetString("Phone"),
                });
            }
            connection.Close();
            return Page();
        }
        public class Userlist
        {
            public string Training { get; set; }
            public string Name { get; set; }
            public string Address{ get; set; }
            public string Phone{ get; set; }
        }

    }
    
}
