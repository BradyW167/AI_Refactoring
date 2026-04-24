using System.Windows.Forms;

namespace JumpGame
{
    public partial class frmLogin : Form
    {
        // Connection options
        private string ConnectionString { get; set; }
        // Persistence layer
        private DatabaseManager Database { get; set; }

        public frmLogin()
        {
            InitializeComponent();

            /* ---------------------- ASSUMES YOU HAVE AN EXISTING .mdf DATABASE FILE ---------------------- */
            string DBPath = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\..\JumpGameDB.mdf"));

            ConnectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;
                        AttachDbFilename={DBPath};
                        Integrated Security=True;
                        Connection Timeout=5";

            Database = new DatabaseManager(ConnectionString);

            // Clear error text
            lblErrorPassword.Text = "";
        }

        // Clears all user login info
        public void Logout()
        {
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;
            lblErrorPassword.Text = string.Empty;
        }

        // Validate login
        private void btnLogin_Click(object sender, EventArgs e)
        {
            User attempt = new User(txtUsername.Text, txtPassword.Text);

            if (Database.ValidateLogin(attempt))
            {
                this.Hide();

                // Create and show the main game form
                frmMain frmMain = new frmMain(attempt, ConnectionString);
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
