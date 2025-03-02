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

        public IActionResult OnGet(int? id) //query param
        {
            var isLoggedIn = HttpContext.Session.GetString("Login");
            if (isLoggedIn==null ||isLoggedIn=="false")
            {
                return RedirectToPage("/Login");

            }

            if (id == null)
            {
                return RedirectToPage("/Admin/Traininglist");
            }



            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT trainingId , Name,Address,Phone, Gender FROM Userlist WHERE trainingId = @id";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Userlists.Add(new Userlist
                {
                    TrainingId = reader.GetInt32("trainingId"),
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
            public int TrainingId { get; set; }
            public required string Name { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }

            public string Email { get; set; }

            public string Gender { get; set; }
            public IFormFile UploadedFile1 { get; set; }

            public IFormFile UploadedFile2 { get; set; }

        }

    }
    
}
