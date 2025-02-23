using MySql.Data.MySqlClient;

namespace wardalert.Services
{
    public class TrainingService
    {
        private readonly string _connectionString = "Server=localhost;Database=project;User=root;Password=;";

        public void UpdateTrainingStatus()
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "SELECT TrainingId, StartDate, EndDate FROM training";
            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            var updates = new List<(int, string)>();

            while (reader.Read())
            {
                int trainingId = reader.GetInt32("TrainingId");
                DateTime startDate = reader.GetDateTime("StartDate");
                DateTime endDate = reader.GetDateTime("EndDate");

                string newStatus = ComputeTrainingStatus(startDate, endDate);
                updates.Add((trainingId, newStatus));
            }

            reader.Close();

            foreach (var (trainingId, newStatus) in updates)
            {
                string updateQuery = "UPDATE training SET Status = @Status WHERE TrainingId = @TrainingId";
                using var updateCommand = new MySqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@Status", newStatus);
                updateCommand.Parameters.AddWithValue("@TrainingId", trainingId);
                updateCommand.ExecuteNonQuery();
            }

            connection.Close();
        }

        public string ComputeTrainingStatus(DateTime startDate, DateTime endDate)
        {
            DateTime now = DateTime.Now;
            if (now < startDate) return "Upcoming";
            if (now >= startDate && now <= endDate) return "Ongoing";
            return "Expired";
        }
         

    }
}
