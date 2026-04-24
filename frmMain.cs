using JumpGame.Properties;
using System.Media;
using System.Runtime.InteropServices;

namespace JumpGame
{
    public partial class frmMain : Form
    {
        // Logged-in user
        private User User { get; set; }
        // Connection options (forwarded to frmScores)
        private string ConnectionString { get; set; }
        // Persistence layer
        private DatabaseManager Database { get; set; }

        // Length of a game tick in milliseconds
        private const int GameTickSpeed = 40;
        // Number of game ticks per second
        private const int TicksPerSecond = (int)(1000 / GameTickSpeed);

        // Owns movement, collisions, scoring, grounded tracking, and
        // victory/death detection. frmMain is the UI shell around it.
        private GameEngine engine;

        // Plays audio streams
        private SoundPlayer MusicPlayer;

        public frmMain(User user, string connectionString)
        {
            InitializeComponent();

            User = user;
            lblUsername.Text = User.Username;

            ConnectionString = connectionString;
            Database = new DatabaseManager(ConnectionString);

            MusicPlayer = new SoundPlayer();

            /* ---- YOU CAN CHANGE ALL THESE IN THE DESIGNER TAB --- */
            tmrGame.Interval = GameTickSpeed;

            picPlayer.Margin = new Padding(0);
            picEnemy1.Margin = new Padding(0);
            picEnemy2.Margin = new Padding(0);

            picPlayer.Tag = "player";
            picEnemy1.Tag = "enemy";
            picEnemy2.Tag = "enemy";
            /* ----------------------------------------------------- */

            engine = new GameEngine(
                this.Controls,
                picPlayer,
                picEnemy1, picEnemy2,
                lblPlatform4, lblPlatform6);
            engine.PlayerDied += PlayerDeath;
            engine.PlayerWon += Victory;

            // Reset incosistent variables for new game
            InitializeGame();
        }

        // Resets all necessary properties for a new game
        private void InitializeGame()
        {
            engine.Reset();

            PlayNewAudio(Resources.theme, true);

            tmrGame.Start();
        }

        // Plays input audio file from the SoundPlayer
        private void PlayNewAudio(UnmanagedMemoryStream newStream, bool loopMusic = false)
        {
            MusicPlayer.Stop();
            MusicPlayer.Stream = newStream;

            if (loopMusic)
                MusicPlayer.PlayLooping();
            else
                MusicPlayer.Play();
        }

        // User input handler for key presses
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) engine.Player.OnLeftDown();
            if (e.KeyCode == Keys.Right) engine.Player.OnRightDown();
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Up) engine.Player.OnJumpDown();
        }

        // User input handler for key releases
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) engine.Player.OnLeftUp();
            if (e.KeyCode == Keys.Right) engine.Player.OnRightUp();
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Up) engine.Player.OnJumpUp();
        }

        // Game tick function, runs every (GameTickSpeed) milliseconds
        private void tmrGame_Tick(object sender, EventArgs e)
        {
            lblTime.Text = "Time: " + (engine.Ticks / TicksPerSecond);
            lblScore.Text = "Score: " + engine.Score;

            engine.Tick(this.ClientSize.Width, this.ClientSize.Height);
        }

        // Ends game with victory
        private void Victory()
        {
            tmrGame.Stop();
            PlayNewAudio(Resources.smb_gameover);

            int TimeInSeconds = (int)(engine.Ticks / TicksPerSecond);

            Database.InsertScore(new Score(User.Username, engine.Score, TimeInSeconds, DateTime.Now));

            RestartPrompt(true);
        }

        // Ends game with player death
        private void PlayerDeath()
        {
            tmrGame.Stop();
            PlayNewAudio(Resources.smb_mariodie);

            RestartPrompt(false);
        }

        // Prompts player about their win or loss and asks to view scores or play again
        private void RestartPrompt(bool wonGame)
        {
            string message = string.Empty;
            string caption = "Play Again?";

            if (wonGame)
                message = $"You Won! Score:{engine.Score}\n\nView Scores?";
            else
                message = "You Died!\n\nPlay Again?";

            // If user hits yes to play again
            if (MessageBox.Show(message, caption,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (wonGame)
                {
                    this.Hide();
                    frmScores scoreForm = new frmScores(User, ConnectionString);
                    scoreForm.Show();
                    this.Dispose();
                }
                else
                {
                    InitializeGame();
                }
            }
            // Else user does want to view scores or play again
            else
            {
                Application.Exit();
            }
        }

        // When logout label is clicked
        private void lblLogout_Click(object sender, EventArgs e)
        {
           Logout();
        }

        // When form is closed (Windows X button)
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // Switches to login form
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

            this.MusicPlayer.Stop();
            this.Dispose();
        }
    }
}
