using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace JumpGame
{
    // Owns the SqlConnection and all raw SQL for the game. Forms use this
    // instead of issuing queries themselves.
    public class DatabaseManager
    {
        private string ConnectionString { get; set; }
        private SqlConnection Connection { get; set; }

        public DatabaseManager(string connectionString)
        {
            ConnectionString = connectionString;
            Connection = new SqlConnection(ConnectionString);
        }

        // Attempts to open a connection to the database.
        // Returns true on successful connection, false on failure.
        private bool OpenConnection()
        {
            try
            {
                if (Connection != null)
                {
                    // Check if it's marked open but is actually unusable
                    if (Connection.State == ConnectionState.Open)
                    {
                        return true;
                    }
                    else
                    {
                        Connection.Dispose();
                        Connection = new SqlConnection(ConnectionString);
                    }

                    Connection.Open();
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Returning false, Unexpected error: " + ex.Message);
                return false;
            }
        }

        private void CloseConnection()
        {
            Connection.Close();
        }

        // Returns true if the given credentials match a row in Users.
        public bool ValidateLogin(User user)
        {
            if (!OpenConnection())
            {
                return false;
            }

            SqlCommand command = Connection.CreateCommand();
            command.CommandText = "Select * from Users WHERE username = @username AND password = @password";
            command.Parameters.AddWithValue("username", user.Username);
            command.Parameters.AddWithValue("password", user.Password);

            SqlDataReader reader = command.ExecuteReader();
            bool found = reader.HasRows;

            CloseConnection();
            return found;
        }

        // Inserts a new score into the Scores table.
        // Returns true if a row was inserted.
        public bool InsertScore(Score score)
        {
            if (!OpenConnection())
            {
                return false;
            }

            // Parameterized query string (prevents SQL code injection)
            string query = "INSERT INTO Scores (username,score,time,date) VALUES (@username,@score,@time,@date)";

            SqlCommand command = Connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@username", score.Username);
            command.Parameters.AddWithValue("@score", score.Points);
            command.Parameters.AddWithValue("@time", score.Time);
            command.Parameters.AddWithValue("@date", score.Date);

            int result = command.ExecuteNonQuery();
            return result > 0;
        }

        // Returns all scores ordered by score descending.
        // Returns an empty DataTable on failure.
        public DataTable GetScores()
        {
            DataTable dataTable = new DataTable();

            if (!OpenConnection())
            {
                return dataTable;
            }

            string query = "Select Username, Score, Time, Date FROM Scores " +
                           "ORDER BY score DESC ";

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(query, Connection);

            try
            {
                adapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetScores unexpected error: " + ex.Message);
                return dataTable;
            }
        }
    }
}
