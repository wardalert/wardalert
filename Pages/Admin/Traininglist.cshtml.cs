using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class TrainingModel : PageModel
{
    private readonly string _connectionString = "Server=185.176.40.25;Database=4583979_wardalert;User=4583979_wardalert;Password=qTuCSS#qcS9r4P2;";

    public List<TrainingList> TrainingLists { get; set; } = new List<TrainingList>();

    public IActionResult OnGet()
    {
        var isLoggedIn = HttpContext.Session.GetString("Login");
        if (isLoggedIn == null || isLoggedIn == "false")
        {
            return RedirectToPage("/Login");
        }

        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        string query = "SELECT id, Title, StartDate FROM training";

        using var command = new MySqlCommand(query, connection);
        using var reader = command.ExecuteReader();


        while (reader.Read())
        {
            TrainingLists.Add(new TrainingList
            {
                Id = reader.GetInt32("Id"),
                Title = reader.GetString("Title"),
                StartDate = reader.GetDateTime("StartDate")
            });
        }

        connection.Close();
        return Page();
    }
}

public class TrainingList
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
}

