using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace wardalert.Pages
{
    public class EventsModel : PageModel
    {
        private readonly string _connectionString = "Server=localhost;Database=project;User=root;Password=;AllowZeroDateTime=True;ConvertZeroDateTime=True;";
        public List<EventCard> Events { get; set; } = new List<EventCard>();

        public void OnGet()
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT  id , Title, Description , UploadedAt FROM event ORDER BY UploadedAt DESC";
            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Events.Add(new EventCard
                {
                    id = reader.GetInt32("id"),
                    Title = reader.GetString("Title"),
                    Description = reader.GetString("Description"),
                    UploadedAt = reader.GetDateTime("UploadedAt")
                });
            }
            connection.Close();
        }

        public class EventCard
        {
            public int id { get; set; }
            public string Title { get; set; }         
            public string Description { get; set; }
            public DateTime UploadedAt { get; set; }
        }
       
    }
}
