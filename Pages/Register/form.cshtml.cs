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
        public string Email { get; set; }
        [BindProperty]
        public string Phone { get; set; }
        public void OnGet()
        {
        }

        public void OnPost()
        {
            // Handle form submission
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "INSERT INTO Userlist (trainingId, Name, Address, Email, Phone) VALUES (@trainingId, @Name, @Address, @Email, @Phone)";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Training", trainingId);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.ExecuteNonQuery();
            connection.Close();


        }
    }
}
