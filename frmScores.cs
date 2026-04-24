using System.Data;
using System.Windows.Forms;

namespace JumpGame
{
    public partial class frmScores : Form
    {
        // Logged-in user
        private User User { get; set; }
        // Connection options (forwarded back to frmMain on Play Again)
        private string ConnectionString { get; set; }
        // Persistence layer
        private DatabaseManager Database { get; set; }
        // Score data in formatted table
        public DataTable ScoreDataTable { get; set; }
        // Binding source for the data grid view
        public BindingSource BindingSource { get; set; }

        public frmScores(User user, string connectionString)
        {
            InitializeComponent();

            User = user;

            ConnectionString = connectionString;
            Database = new DatabaseManager(ConnectionString);

            BindingSource = [];

            // Set your score data using this data table
            ScoreDataTable = Database.GetScores();

            BindingSource.DataSource = ScoreDataTable;

            dgvScores.DataSource = BindingSource;

            // Sets the data grid to auto size all columns
            dgvScores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // Show the login form when the Window X button is hit
        private void frmScores_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnPlayAgain_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMain mainForm = new frmMain(User, ConnectionString);

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
