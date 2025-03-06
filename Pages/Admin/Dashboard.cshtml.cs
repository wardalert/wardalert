using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace wardalert.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly string _connectionString = "Server=localhost;Database=project;User=root;Password=;";

        public int TotalTrainings { get; set; }
        public int TotalUsers { get; set; }
        public int ActiveTrainings { get; set; }
        public List<Training> RecentTrainings { get; set; } = new List<Training>();

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Login") != "true")
            {
                return RedirectToPage("/Login");
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Get Total Trainings
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM training", connection))
                {
                    TotalTrainings = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // Get Total Users
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM userlist", connection))
                {
                    TotalUsers = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // Get Active Trainings
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM training WHERE Status = 'Ongoing'", connection))
                {
                    ActiveTrainings = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // Get Recent Trainings
                using (var cmd = new MySqlCommand(
                    "SELECT Title, StartDate, Status FROM training ORDER BY StartDate DESC LIMIT 5", connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RecentTrainings.Add(new Training
                        {
                            Title = reader.GetString("Title"),
                            StartDate = reader.GetDateTime("StartDate"),
                            Status = reader.GetString("Status")
                        });
                    }
                }
            }

            return Page();
        }
    }

    public class Training
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; }
    }
}