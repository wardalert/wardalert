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
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            // Get the current number of users registered for the training
            string countQuery = "SELECT COUNT(*) FROM Userlist WHERE trainingId = @trainingId";
            using var countCommand = new MySqlCommand(countQuery, connection);
            countCommand.Parameters.AddWithValue("@trainingId", trainingId);
            int userCount = Convert.ToInt32(countCommand.ExecuteScalar());

            // Get the training capacity
            string capacityQuery = "SELECT Capacity FROM Training WHERE TrainingId = @trainingId";
            using var capacityCommand = new MySqlCommand(capacityQuery, connection);
            capacityCommand.Parameters.AddWithValue("@trainingId", trainingId);
            object capacityResult = capacityCommand.ExecuteScalar();

            if (capacityResult == null)
            {
                ModelState.AddModelError("TrainingError", "Training not found.");
                return Page();
            }

            int trainingCapacity = Convert.ToInt32(capacityResult);

            // Check if the training is full
            if (userCount >= trainingCapacity)
            {
                ModelState.AddModelError("TrainingError", "This training is already full. Registration is not allowed.");
                return Page(); // Return the form with an error message
            }

            // Insert the new user if training is not full
            string query = "INSERT INTO Userlist (trainingId, name, address, phone, gender) VALUES (@trainingId, @Name, @Address, @Phone, @Gender)";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@trainingId", trainingId);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Gender", Gender);
            command.ExecuteNonQuery();

            connection.Close();

            return RedirectToPage("/Trainings/Upcoming"); // Redirect on success
        }
    }
}
