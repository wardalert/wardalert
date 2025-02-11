using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace wardalert.Pages.trainings
{
    public class UpcomingModel : PageModel
    {
        private readonly string _connectionString = "Server=localhost;Database=project;User=root;Password=;";
        public List<Train> Trainings { get; set; } = new List<Train>();

        [BindProperty]
        public required string Training { get; set; }
        [BindProperty]
        public required string Name { get; set; }
        [BindProperty]
        public string Address { get; set; }
        [BindProperty]
        public required string Phone { get; set; }
        public void OnPost()
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "INSERT INTO Userlist (Training, Name, Address,Phone) VALUES (@Training,@Name, @Address,@Phone)";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Training", Training);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);


            command.ExecuteNonQuery();
            connection.Close();
        }

        public void OnGet()
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT id,Trainingtitle, Trainingdescription,Date,Time FROM training";

            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Trainings.Add(new Train
                {

                    Trainingtitle = reader.GetString("Trainingtitle"),
                    Trainingdescription = reader.GetString("Trainingdescription"),
                    Date = reader.GetString("Date"),
                    Time = reader.GetString("Time"),
                });
            }
            connection.Close();
        }

        public class Train
        {
            public string Trainingtitle { get; set; }
            public string Trainingdescription { get; set; }
            public string Date { get; set; }
            public string Time { get; set; }

        }
    }
}
