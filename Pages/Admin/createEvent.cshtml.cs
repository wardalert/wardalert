using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using wardalert.Services;

namespace wardalert.Pages.Admin
{
    public class createEventModel : PageModel
    {
        private readonly string _connectionString = "Server=localhost;Database=project;User=root;Password=;";
        private readonly IConfiguration _configuration;

        public createEventModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [BindProperty]
        public int id { get; set; }

        [BindProperty]
        public required string Title { get; set; }
        [BindProperty]
        public required string Description { get; set; }
        [BindProperty]
        public DateTime UploadedAt { get; set; }
        public void OnGet()
        {
        }
        public void OnPost()
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            if (id > 0) 
            {
                string updateQuery = "UPDATE event SET id=@id, Title=@Title, Description=@Description, UploadedAt WHERE id=@id";
                using var updateCommand = new MySqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@id", id);
                updateCommand.Parameters.AddWithValue("@Title", Title);
                updateCommand.Parameters.AddWithValue("@Description", Description);
                updateCommand.Parameters.AddWithValue("@UploadedAt", UploadedAt);
              

                updateCommand.ExecuteNonQuery();

            }
            else
            {

                string query = "INSERT INTO training (Title,Description,UploadedAt) VALUES (@Title, @Description,@UploadedAt)";

                using var command = new MySqlCommand(query, connection);

                /* command.Parameters.AddWithValue("@TrainingId", TrainingId);*/
                command.Parameters.AddWithValue("@Title", Title);
                command.Parameters.AddWithValue("@Description", Description);
                command.Parameters.AddWithValue("@UploadedAt", UploadedAt);


                command.ExecuteNonQuery();
                connection.Close();

            }
        }
    }
}
