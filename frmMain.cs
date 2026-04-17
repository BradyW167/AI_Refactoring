using JumpGame.Properties;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Media;

namespace JumpGame
{
    public partial class frmMain : Form
    {
        // Name of logged in user
        private string Username {  get; set; }
        // Connection options
        private string ConnectionString { get; set; }
        // Connection to database
        private SqlConnection Connection { get; set; }
        // Setup for singular commands
        private SqlCommand Command { get; set; }

        // Moving platform height limits
        private const int Platform1Top = 350;
        private const int Platform2Top = 160;
        private const int Platform1Bottom = 470;
        private const int Platform2Bottom = 280;

        // Length of a game tick in milliseconds
        private const int GameTickSpeed = 40;
        // Number of game ticks per second
        private const int TicksPerSecond = (int)(1000 / GameTickSpeed);

        // State of groundedness
        private bool Grounded { get; set; }
        private bool LandedOnPlatform { get; set; }

        // Number of score points
        private int Score { get; set; }
        // Number of milliseconds passed since game start
        private int Time { get; set; }

        // Plays audio streams
        private SoundPlayer MusicPlayer;

        // Game entities
        private Player player;
        private Enemy enemy1;
        private Enemy enemy2;
        private MovingPlatform platform1;
        private MovingPlatform platform2;

        public frmMain(string username, string connectionString)
        {
            InitializeComponent();

            Username = username;
            lblUsername.Text = Username;

            ConnectionString = connectionString;
            Connection = new SqlConnection(ConnectionString);
            Command = Connection.CreateCommand();

            MusicPlayer = new SoundPlayer();

            player = new Player(picPlayer);
            enemy1 = new Enemy(picEnemy1, 6);
            enemy2 = new Enemy(picEnemy2, 8);
            platform1 = new MovingPlatform(lblPlatform4, 3, Platform1Top, Platform1Bottom);
            platform2 = new MovingPlatform(lblPlatform6, 7, Platform2Top, Platform2Bottom);

            /* ---- YOU CAN CHANGE ALL THESE IN THE DESIGNER TAB --- */
            tmrGame.Interval = GameTickSpeed;

            picPlayer.Margin = new Padding(0);
            picEnemy1.Margin = new Padding(0);
            picEnemy2.Margin = new Padding(0);

            picPlayer.Tag = "player";
            picEnemy1.Tag = "enemy";
            picEnemy2.Tag = "enemy";
            /* ----------------------------------------------------- */

            // Reset incosistent variables for new game
            InitializeGame();
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

        // Inserts input data into the Scores table
        private bool InsertScore(string username, int score, int time, DateTime date)
        {
            if (!OpenConnection())
            {
                return false;
            }

            // Parameterized query string (prevents SQL code injection)
            string query = "INSERT INTO Scores (username,score,time,date) VALUES (@username,@score,@time,@date)";

            Command = Connection.CreateCommand();
            Command.CommandText = query;

            // Fill parameters with actual values
            Command.Parameters.AddWithValue("@username", username);
            Command.Parameters.AddWithValue("@score", score);
            Command.Parameters.AddWithValue("@time", time);
            Command.Parameters.AddWithValue("@date", date);

            // Execute command and store returned data
            int result = Command.ExecuteNonQuery();

            // If columns were modified, return true
            return result > 0;
        }

        // Resets all necessary properties for a new game
        private void InitializeGame()
        {
            ResetCharacters();
            ResetCoins();

            Score = 0;
            Time = 0;

            PlayNewAudio(Resources.theme, true);

            tmrGame.Start();
        }

        // Resets character objects
        private void ResetCharacters()
        {
            ResetPlayer();
            ResetEnemies();
        }

        // Make all collected coins visible again
        private void ResetCoins()
        {
            foreach (Control control in this.Controls)
            {
                if (control.Tag != null)
                {
                    if (control is PictureBox && (string)control.Tag == "coin")
                    {
                        control.Visible = true;
                    }
                }
            }
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

        // Repositions player and resets movement properties
        private void ResetPlayer()
        {
            player.Reset();
            Grounded = false;
        }

        // Repositons enemies to a random starting platform
        private void ResetEnemies()
        {
            enemy1.Reset(GetRandomPlatform());
            enemy2.Reset(GetRandomPlatform());
        }

        // Returns a random platform as a Label
        private Label GetRandomPlatform()
        {
            Random rand = new Random();

            // Define the possible platform name numbers
            int[] platformNumbers = { 2, 3, 5, 7, 8 };

            // Select a random index from 0 to platformNumbers.Length
            int randIndex = rand.Next(platformNumbers.Length);

            // Get the platform name using the random index
            string randomPlatformName = $"lblPlatform{platformNumbers[randIndex]}";

            if (this.Controls[randomPlatformName] is Label randomPlatform)
                return randomPlatform;
            else
                return new Label();
        }

        // User input handler for key presses
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            player.HandleKeyDown(e.KeyCode, Grounded);
        }

        // User input handler for key releases
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            player.HandleKeyUp(e.KeyCode);
        }

        // Game tick function, runs every (GameTickSpeed) milliseconds
        private void tmrGame_Tick(object sender, EventArgs e)
        {
            UpdateTime();
            UpdateScore();

            MoveImages();

            // Reset landed state
            LandedOnPlatform = false;

            // Loop through all controls in this game form
            foreach (Control control in this.Controls)
            {
                // Check control.Tag type (controls with tags are game objects with collision)
                if (control.Tag is string tagValue)
                {
                    CheckCollision(control, tagValue);
                }
            }

            // After all other checks, update grounded boolean
            // Needed for moving platforms
            if (!LandedOnPlatform)
            {
                Grounded = false;
            }
        }

        private void UpdateTime()
        {
            lblTime.Text = "Time: " + (Time / TicksPerSecond);
            Time += 1;
        }

        private void UpdateScore()
        {
            lblScore.Text = "Score: " + Score;
        }

        // Runs movement handling for all moving images
        private void MoveImages()
        {
            if (player.Update(Grounded, this.ClientSize.Width, this.ClientSize.Height))
                PlayerDeath();

            enemy1.Update();
            enemy2.Update();
            platform1.Update();
            platform2.Update();
        }

        private void CheckCollision(Control control, string tagValue)
        {
            if (tagValue == "enemy")
            {
                EnemyCollisionCheck(control);
            }

            if (tagValue == "coin")
            {
                CoinCollisionCheck(control);
            }

            if (tagValue == "platform" || tagValue == "movingPlatform")
            {
                PlatformCollisionCheck(control);
            }
            if (tagValue == "door")
            {
                DoorCollisionCheck(control);
            }
        }

        private void EnemyCollisionCheck(Control enemy)
        {
            if (picPlayer.Bounds.IntersectsWith(enemy.Bounds))
            {
                PlayerDeath();
            }
        }

        private void CoinCollisionCheck(Control coin)
        {
            if (picPlayer.Bounds.IntersectsWith(coin.Bounds) && coin.Visible)
            {
                coin.Visible = false;
                Score += 1;
            }
        }

        // Returns true when player is grounded on top of a platform, false when not
        private void PlatformCollisionCheck(Control platform)
        {
            if (picPlayer.Bounds.IntersectsWith(platform.Bounds))
            {
                if (picPlayer.Bounds.IntersectsWith(platform.Bounds))
                {
                    Rectangle plat = platform.Bounds;

                    // Additional pixels added to collision checks
                    // Prevents player image from bouncing while on platforms
                    const int SafetyThreshold = 5;

                    // Landing on top of platform
                    if (picPlayer.Bottom >= plat.Top - 5 &&
                        picPlayer.Top < plat.Top &&
                        player.JumpSpeed >= 0)
                    {
                        LandedOnPlatform = true;
                        Grounded = true;
                        player.JumpSpeed = 0;
                        picPlayer.Top = plat.Top - picPlayer.Height + 1;
                    }
                    // Hitting platform from the sides
                    else if (picPlayer.Right > plat.Left && picPlayer.Left < plat.Left && picPlayer.Bottom > plat.Top + SafetyThreshold)
                    {
                        picPlayer.Left = plat.Left - picPlayer.Width;
                        player.GoRight = false;
                    }
                    else if (picPlayer.Left < plat.Right && picPlayer.Right > plat.Right && picPlayer.Bottom > plat.Top + SafetyThreshold)
                    {
                        picPlayer.Left = plat.Right;
                        player.GoLeft = false;
                    }
                    // Hitting platform from below
                    else if (picPlayer.Top < plat.Bottom && picPlayer.Bottom > plat.Bottom)
                    {
                        player.JumpSpeed = 5; // small bounce back
                        picPlayer.Top = plat.Bottom;
                    }
                }
            }
        }

        private void DoorCollisionCheck(Control door)
        {
            if (picPlayer.Bounds.IntersectsWith(door.Bounds))
            {
                Victory();
            }
        }

        // Ends game with victory
        private void Victory()
        {
            tmrGame.Stop();
            PlayNewAudio(Resources.smb_gameover);

            int TimeInSeconds = (int)(Time / TicksPerSecond);

            InsertScore(Username, Score, TimeInSeconds, DateTime.Now);

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
                message = $"You Won! Score:{Score}\n\nView Scores?";
            else
                message = "You Died!\n\nPlay Again?";

            // If user hits yes to play again
            if (MessageBox.Show(message, caption,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (wonGame)
                {
                    this.Hide();
                    frmScores scoreForm = new frmScores(Username, ConnectionString);
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
