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
            if (isLoggedIn == null || isLoggedIn == "false")
            {
                return RedirectToPage("/Login");
            }

            if (id == null)
            {
                return RedirectToPage("/Admin/Traininglist");
            }

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT trainingId, Name, Address, Phone, Email, Gender,Status, imagePath1, imagePath2 FROM Userlist WHERE trainingId = @id";

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
                    Email = reader.GetString("Email"),
                    Gender = reader.GetString("Gender"),
                    Status = reader.GetString("Status"),
                    UploadedFile1 = reader.IsDBNull(reader.GetOrdinal("imagePath1")) ? string.Empty : reader.GetString("imagePath1"),
                    UploadedFile2 = reader.IsDBNull(reader.GetOrdinal("imagePath2")) ? string.Empty : reader.GetString("imagePath2")
                });
            }

            connection.Close();
            return Page();
        }
        public async Task<IActionResult> OnPostUpdateStatusAsync(int userId, string status)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = "UPDATE Userlist SET Status = @status WHERE id = @userId";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@status", status);
            command.Parameters.AddWithValue("@userId", userId);

            int rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected > 0)
            {
                return new JsonResult(new { success = true, message = "User status updated successfully!" });
            }
            else
            {
                return new JsonResult(new { success = false, message = "Failed to update status!" });
            }
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
            public string Status { get; set; }
            public string UploadedFile1 { get; set; }
            public string UploadedFile2 { get; set; }
        }
    }
}
