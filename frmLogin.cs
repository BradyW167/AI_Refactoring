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

namespace JumpGame
{
    public partial class frmLogin : Form
    {
        // Name of logged in user
        private string Username { get; set; }
        // Connection options
        private string ConnectionString { get; set; }
        // Connection to database
        private SqlConnection Connection { get; set; }
        // Setup for singular commands
        private SqlCommand Command { get; set; }
        // Read results (can be null because of the ?)
        private SqlDataReader? Reader { get; set; }

        public frmLogin()
        {
            InitializeComponent();

            Username = string.Empty;

            /* ---------------------- ASSUMES YOU HAVE AN EXISTING .mdf DATABASE FILE ---------------------- */
            string DBPath = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\..\JumpGameDB.mdf"));

            ConnectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;
                        AttachDbFilename={DBPath};
                        Integrated Security=True;
                        Connection Timeout=5";

            // Set default values for Sql properties
            Connection = new SqlConnection(ConnectionString);
            Command = new SqlCommand();
            Reader = null;

            // Clear error text
            lblErrorPassword.Text = "";
        }

        // Clears all user login info
        public void Logout()
        {
            Username = string.Empty;
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;
            lblErrorPassword.Text = string.Empty;
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

        // Queries the database for input username and password
        // Returns true if matching user found, false if not
        private bool LoginQuery(string username, string password)
        {
            if (!OpenConnection())
            {
                return false;
            }

            Command = Connection.CreateCommand();
            Command.CommandText = "Select * from Users WHERE username = @username AND password = @password";

            Command.Parameters.AddWithValue("username", username);
            Command.Parameters.AddWithValue("password", password);

            Reader = Command.ExecuteReader();

            // If reader has data (matching user and password were found)
            if (Reader.HasRows)
            {
                CloseConnection();
                return true;
            }
            // Else no matching data was found (invalid username or password)
            else
            {
                CloseConnection();
                return false;
            }
        }

        // Closes a SQL database connection
        private void CloseConnection()
        {
            Connection.Close();
        }

        // Validate login
        private void btnLogin_Click(object sender, EventArgs e)
        {
            /* 
             * Search for input username and password in database 
             * You should create a method using the DBController for this
             */
            bool validLogin = LoginQuery(txtUsername.Text, txtPassword.Text);

            // If the input credentials were valid...
            if (validLogin)
            {
                Username = txtUsername.Text;
                this.Hide();

                // Create and show the main game form
                frmMain frmMain = new frmMain(Username, ConnectionString);
                frmMain.Show();
            }
            else
            {
                lblErrorPassword.Text = "*";
            }
        }

        // Clears the text input fields
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
        }

        // Exits the program
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Triggers a btnLogin_Click event if user inputs Enter while focused in txtPassword
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }
    }
}
