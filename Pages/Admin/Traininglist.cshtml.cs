using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace wardalert.Pages.Admin
{
    public class TrainingModel : PageModel
    {
        private readonly string _connectionString = "Server=localhost;Database=project;User=root;Password=;";

        public List<TrainingList> TrainingLists { get; set; } = new List<TrainingList>();

        public IActionResult OnGet()
        {
            var isLoggedIn = HttpContext.Session.GetString("Login");
            if (isLoggedIn == null || isLoggedIn == "false")
            {
                return RedirectToPage("/Login");
            }

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT TrainingId, Title, StartDate, EndDate, Status  FROM training";

            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();


            while (reader.Read())
            {
                TrainingLists.Add(new TrainingList
                {
                    TrainingId = reader.GetInt32("TrainingId"),
                    Title = reader.GetString("Title"),
                    StartDate = reader.GetDateTime("StartDate"),
                    EndDate = reader.GetDateTime("EndDate"),
                    Status = reader.GetString("Status") // Fetch status directly from DB
                });
            }

            connection.Close();
            return Page();
        }
    }

    public class TrainingList
    {
        public int TrainingId { get; set; }
        public required string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } // Ensure this is fetched and used
    }



}