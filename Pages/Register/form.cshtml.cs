using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;


namespace wardalert.Pages.Register
{
    public class formModel : PageModel
    {
        private readonly string _connectionString = "Server=localhost;Database=project;User=root;Password=;";
        [BindProperty]
        public int trainingId { get; set; }
        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string Address { get; set; }
        [BindProperty]
        public string Phone { get; set; }
        [BindProperty]
        public string Gender { get; set; }
        public void OnGet(int trainingId)
        {
            this.trainingId = trainingId;
            
        }

        public IActionResult OnPost()
        {
            // Handle form submission
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "INSERT INTO Userlist (trainingId, name, address, phone, gender ) VALUES (@trainingId, @Name, @Address, @Phone , @Gender)";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@trainingId", trainingId);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Gender", Gender);
            command.ExecuteNonQuery();
            connection.Close();

            return RedirectToPage("/Trainings/Upcoming");

        }
    }
}
