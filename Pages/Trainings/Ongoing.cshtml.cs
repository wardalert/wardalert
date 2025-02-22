using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace wardalert.Pages.trainings
{
    public class OngoingModel : PageModel
    {
        private readonly string _connectionString = "Server=localhost;Database=project;User=root;Password=;AllowZeroDateTime=True;ConvertZeroDateTime=True;";
        public List<TrainingCard> Trainings { get; set; } = new List<TrainingCard>();

        public void OnGet()
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT TrainingId, Title, Time, StartDate, EndDate, Description FROM training WHERE Status = 'Ongoing'";
            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Trainings.Add(new TrainingCard
                {
                    TrainingId = reader.GetInt32("TrainingId"),
                    Title = reader.GetString("Title"),
                    Time = reader.IsDBNull(reader.GetOrdinal("Time")) ? (DateTime?)null : reader.GetDateTime("Time"),
                    StartDate = reader.IsDBNull(reader.GetOrdinal("StartDate")) ? (DateTime?)null : reader.GetDateTime("StartDate"),
                    EndDate = reader.IsDBNull(reader.GetOrdinal("EndDate")) ? (DateTime?)null : reader.GetDateTime("EndDate"),
                    Description = reader.GetString("Description"),
                });
            }
            connection.Close();
        }

        public class TrainingCard
        {
            public int TrainingId { get; set; }
            public string Title { get; set; }
            public DateTime? Time { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string Description { get; set; }
        }
    }
}
