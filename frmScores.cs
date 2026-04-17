using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace JumpGame
{
    public partial class frmScores : Form
    {
        // Name of logged in user
        private string Username { get; set; }
        // Connection options
        private string ConnectionString { get; set; }
        // Connection to database
        private SqlConnection Connection { get; set; }
        // Helpful for large queries
        private SqlDataAdapter Adapter { get; set; }
        // Score data in formatted table
        public DataTable ScoreDataTable { get; set; }
        // Binding source for the data grid view
        public BindingSource BindingSource { get; set; }

        public frmScores(string username, string connectionString)
        {
            InitializeComponent();

            Username = username;

            ConnectionString = connectionString;
            Connection = new SqlConnection(ConnectionString);
            Adapter = new SqlDataAdapter();

            BindingSource = [];

            // Set your score data using this data table
            ScoreDataTable = GetScores();

            BindingSource.DataSource = ScoreDataTable;

            dgvScores.DataSource = BindingSource;

            // Sets the data grid to auto size all columns
            dgvScores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // Attempts to open a connection to a MySQL database.
        // Returns true on successful connection, false on failure
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

        /* 
         * Selects all scores
         * Returns a DataTable of scores on successful query
         * Returns an empty DataTable on failure
         */
        private DataTable GetScores()
        {
            if (!OpenConnection())
            {
                return new DataTable();
            }

            string query = "Select Username, Score, Time, Date FROM Scores " +
                           "ORDER BY score DESC ";

            // Prepare the select command
            Adapter.SelectCommand = new SqlCommand(query, Connection);

            DataTable dataTable = new DataTable();

            try
            {
                // Fill the DataTable with the results of the query
                Adapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetScores unexpected error: " + ex.Message);
                return dataTable;
            }
        }

        // Show the login form when the Window X button is hit
        private void frmScores_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnPlayAgain_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMain mainForm = new frmMain(Username, ConnectionString);

            mainForm.Show();

            this.Dispose();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Logout();
        }

        private void Logout()
        {
            this.Hide();

            // Get a reference to the login form (assuming it's still alive)
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is frmLogin loginForm)
                {
                    loginForm.Logout();
                    loginForm.Show();
                    break;
                }
            }

            this.Dispose();
        }
    }
}
