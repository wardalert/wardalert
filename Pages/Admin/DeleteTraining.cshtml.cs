using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace wardalert.Pages.Admin
{
    public class DeleteTrainingModel : PageModel
    {
        private readonly string _connectionString = "Server=localhost;Database=project;User=root;Password=;";

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("/Admin/Traininglist");
            }

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            // DELETE the training
            string deleteQuery = "DELETE FROM training WHERE TrainingId = @id";
            using var deleteCommand = new MySqlCommand(deleteQuery, connection);
            deleteCommand.Parameters.AddWithValue("@id", id);
            deleteCommand.ExecuteNonQuery();

            connection.Close();

            return RedirectToPage("/Admin/Traininglist"); // Redirect back to the list
        }
    }
}
