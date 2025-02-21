using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace wardalert.Pages.Admin
{
    public class createtrainingModel : PageModel
    {
        private readonly string _connectionString = "Server=localhost;Database=project;User=root;Password=;";
        public bool IsEdit { get; set; } = false;

        [BindProperty]
        public int TrainingId { get; set; }

        [BindProperty]
        public required string Title { get; set; }
        [BindProperty]
        public required string Description { get; set; }
        [BindProperty]
        public required string Status { get; set; }
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

            if (TrainingId > 0) //update/edit existing training
            {
                string updateQuery = "UPDATE training SET Title=@Title, Description=@Description, Status=@Status, Time=@Time, StartDate=@StartDate, EndDate=@EndDate WHERE TrainingId=@TrainingId";
                using var updateCommand = new MySqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@TrainingId", TrainingId);
                updateCommand.Parameters.AddWithValue("@Title", Title);
                updateCommand.Parameters.AddWithValue("@Description", Description);
                updateCommand.Parameters.AddWithValue("@Status", Status);
                updateCommand.Parameters.AddWithValue("@Time", Time);
                updateCommand.Parameters.AddWithValue("@StartDate", StartDate);
                updateCommand.Parameters.AddWithValue("@EndDate", EndDate);
                updateCommand.ExecuteNonQuery();
            }
            else
            {

            string query = "INSERT INTO training (Title,Description,Status,Time,StartDate,EndDate) VALUES (@Title, @Description,@Status,@Time,@StartDate,@EndDate)";

            using var command = new MySqlCommand(query, connection);

           /* command.Parameters.AddWithValue("@TrainingId", TrainingId);*/
            command.Parameters.AddWithValue("@Title", Title);
            command.Parameters.AddWithValue("@Description", Description);
            command.Parameters.AddWithValue("@Status", Status);
            command.Parameters.AddWithValue("@Time", Time);
            command.Parameters.AddWithValue("@StartDate", StartDate);
            command.Parameters.AddWithValue("@EndDate", EndDate);



            command.ExecuteNonQuery();
            connection.Close();

            Response.Redirect("/Admin/TrainingList");
            }   
        }


        public IActionResult OnGet(int? id)
        {
            var isLoggedIn = HttpContext.Session.GetString("Login");
            if (isLoggedIn == null || isLoggedIn == "false")
            {
                return RedirectToPage("/Login");
            }

            if(id.HasValue)
            {
                IsEdit = true;
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();

                string query = "SELECT * FROM training WHERE TrainingId = @TrainingId";
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@TrainingId", id.Value);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    TrainingId = reader.GetInt32("TrainingId");
                    Title = reader.GetString("Title");
                    Description = reader.GetString("Description");
                    Status = reader.GetString("Status");
                    // Handling possible NULL or invalid MySQL DateTime values
                    Time = !reader.IsDBNull(reader.GetOrdinal("Time")) ? reader.GetDateTime("Time") : DateTime.MinValue;
                    StartDate = !reader.IsDBNull(reader.GetOrdinal("StartDate")) ? reader.GetDateTime("StartDate") : DateTime.MinValue;
                    EndDate = !reader.IsDBNull(reader.GetOrdinal("EndDate")) ? reader.GetDateTime("EndDate") : DateTime.MinValue;
                }
                else
                {
                    return RedirectToPage("/Admin/TrainingList"); // Redirect if training not found
                }


            }

            return Page();
        }

    }
}

