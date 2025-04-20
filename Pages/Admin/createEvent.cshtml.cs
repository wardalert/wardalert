using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;

namespace wardalert.Pages.Admin
{
    public class createEventModel : PageModel
    {
        private readonly string _connectionString = "Server=localhost;Database=project;User=root;Password=;";

        [BindProperty]
        public int id { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Date is required")]
        public DateTime UploadedAt { get; set; } = DateTime.Today;

        public IActionResult OnGet(int? id)
        {
            if (id.HasValue)
            {
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();

                string query = "SELECT * FROM event WHERE id = @id";
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id.Value);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    this.id = reader.GetInt32("id");
                    Title = reader.GetString("Title");
                    Description = reader.GetString("Description");
                    UploadedAt = reader.GetDateTime("UploadedAt");
                }
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            if (id > 0) // Update existing event
            {
                string updateQuery = "UPDATE event SET Title=@Title, Description=@Description, UploadedAt=@UploadedAt WHERE id=@id";
                using var updateCommand = new MySqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@id", id);
                updateCommand.Parameters.AddWithValue("@Title", Title);
                updateCommand.Parameters.AddWithValue("@Description", Description);
                updateCommand.Parameters.AddWithValue("@UploadedAt", UploadedAt);
                updateCommand.ExecuteNonQuery();
            }
            else // Create new event
            {
                string insertQuery = "INSERT INTO event (Title, Description, UploadedAt) VALUES (@Title, @Description, @UploadedAt)";
                using var command = new MySqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@Title", Title);
                command.Parameters.AddWithValue("@Description", Description);
                command.Parameters.AddWithValue("@UploadedAt", UploadedAt);
                command.ExecuteNonQuery();
            }

            return RedirectToPage("/Events");
        }
    }
}