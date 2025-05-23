using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using wardalert.Services;


namespace wardalert.Pages.Admin
{
    public class TrainingModel : PageModel
    {
        private readonly string _connectionString = "Server=localhost;Database=project;User=root;Password=;";
        private readonly TrainingService _trainingService;

        public TrainingModel(TrainingService trainingService)
        {
            _trainingService = trainingService;
        }

        public List<TrainingList> TrainingLists { get; set; } = new List<TrainingList>();

        public IActionResult OnGet()
        {
            var isLoggedIn = HttpContext.Session.GetString("Login");
            if (isLoggedIn == null || isLoggedIn == "false")
            {
                return RedirectToPage("/Login");
            }
            _trainingService.UpdateTrainingStatus();

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT TrainingId, Title, StartDate, EndDate, Status, Capacity  FROM training";

            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();


            while (reader.Read())
            {
                TrainingLists.Add(new TrainingList
                {
                    TrainingId = reader.GetInt32("TrainingId"),
                    Title = reader.GetString("Title"),
                    StartDate = reader.GetDateTime("StartDate"),
                    EndDate = reader.GetDateTime("EndDate"),
                    Status = reader.GetString("Status"),// Fetch status directly from DB
                    Capacity=reader.GetInt32("Capacity")
                });
            }

            connection.Close();
            return Page();
        }



    }

    public class TrainingList
    {
        public int TrainingId { get; set; }
        public required string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        
        public int Capacity { get; set; }// Ensure this is fetched and used
    }





}