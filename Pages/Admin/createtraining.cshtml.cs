using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace wardalert.Pages.Admin
{
    public class createtrainingModel : PageModel
    {
        private readonly string _connectionString = "Server=localhost;Database=project;User=root;Password=;";

        [BindProperty]
        public int TrainingId { get; set; }

        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public string Description { get; set; }
        [BindProperty]
        public string Status { get; set; }
        [BindProperty]
        public DateTime Time { get; set; }
        [BindProperty]
        public DateTime StartDate { get; set; }
        [BindProperty]
        public DateTime EndDate { get; set; }


        public void OnPost()
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "INSERT INTO training (TrainingId,Title,Description,Status,Time,StartDate,EndDate) VALUES (@TrainingId,@Title, @Description,@Status,@Time,@StartDate,@EndDate)";

            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@TrainingId", TrainingId);
            command.Parameters.AddWithValue("@Title", Title);
            command.Parameters.AddWithValue("@Description", Description);
            command.Parameters.AddWithValue("@Status", Status);
            command.Parameters.AddWithValue("@Time", Time);
            command.Parameters.AddWithValue("@StartDate", StartDate);
            command.Parameters.AddWithValue("@EndDate", EndDate);



            command.ExecuteNonQuery();
            connection.Close();



        }


        public IActionResult OnGet()
        {
            var isLoggedIn = HttpContext.Session.GetString("Login");
            if (isLoggedIn == null || isLoggedIn == "false")
            {
                return RedirectToPage("/Login");
            }
            return Page();
        }

    }
}

