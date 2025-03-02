using Google.Protobuf.WellKnownTypes;
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
        public string Email { get; set; }
        [BindProperty]
        public string Gender { get; set; }
        [BindProperty]
        public IFormFile UploadedFile1 { get; set; }
        [BindProperty]

        public IFormFile UploadedFile2 { get; set; }
        // File property



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


            // **File Upload Handling**
            string filePath1 = null;
            if (UploadedFile1 != null && UploadedFile1.Length > 0)
            {
                try
                {
                    string uploadsFolder1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsFolder1))
                    {
                        Directory.CreateDirectory(uploadsFolder1);
                    }

                    // Generate a unique filename to avoid conflicts
                    string uniqueFileName1 = Guid.NewGuid().ToString() + Path.GetExtension(UploadedFile1.FileName);
                    string fileSavePath1 = Path.Combine(uploadsFolder1, uniqueFileName1);

                    using (var fileStream1 = new FileStream(fileSavePath1, FileMode.Create))
                    {
                        UploadedFile1.CopyTo(fileStream1);
                    }

                    // Save relative path instead of absolute path
                    filePath1 = "/uploads/" + uniqueFileName1;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("FileUploadError", "File upload failed: " + ex.Message);
                    return Page();
                }
            }

            string filePath2 = null;
            if (UploadedFile2 != null && UploadedFile2.Length > 0)
            {
                try
                {
                    string uploadsFolder2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsFolder2))
                    {
                        Directory.CreateDirectory(uploadsFolder2);
                    }

                    // Generate a unique filename to avoid conflicts
                    string uniqueFileName2 = Guid.NewGuid().ToString() + Path.GetExtension(UploadedFile2.FileName);
                    string fileSavePath2 = Path.Combine(uploadsFolder2, uniqueFileName2);

                    using (var fileStream2 = new FileStream(fileSavePath2, FileMode.Create))
                    {
                        UploadedFile2.CopyTo(fileStream2);
                    }

                    // Save relative path instead of absolute path
                    filePath2 = "/uploads/" + uniqueFileName2;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("FileUploadError", "File upload failed: " + ex.Message);
                    return Page();
                }
            }


            // Insert the new user if training is not full
            string query = "INSERT INTO Userlist (trainingId, name, address, phone,email, gender,imagePath1,imagePath2) VALUES (@trainingId, @Name, @Address, @Phone,@Email, @Gender,@ImagePath1,@ImagePath2)";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@trainingId", trainingId);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Gender", Gender);
            command.Parameters.AddWithValue("@ImagePath1", filePath1 ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@ImagePath2", filePath2 ?? (object)DBNull.Value);

            command.ExecuteNonQuery();

            connection.Close();

            return RedirectToPage("/Trainings/Upcoming"); // Redirect on success
        }
    }
}
