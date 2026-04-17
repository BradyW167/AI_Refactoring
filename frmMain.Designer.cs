using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.Xml.Linq;
using Font = System.Drawing.Font;

namespace JumpGame
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblScore = new Label();
            lblLogout = new Label();
            lblPlatform1 = new Label();
            lblPlatform5 = new Label();
            lblPlatform8 = new Label();
            lblPlatform7 = new Label();
            lblPlatform2 = new Label();
            lblPlatform4 = new Label();
            lblPlatform3 = new Label();
            lblPlatform6 = new Label();
            lblPlatform9 = new Label();
            picDoor = new PictureBox();
            picCoin20 = new PictureBox();
            picCoin18 = new PictureBox();
            picCoin19 = new PictureBox();
            tmrGame = new System.Windows.Forms.Timer(components);
            lblTime = new Label();
            picCoin1 = new PictureBox();
            picCoin2 = new PictureBox();
            picCoin3 = new PictureBox();
            picCoin4 = new PictureBox();
            picCoin5 = new PictureBox();
            picCoin6 = new PictureBox();
            picCoin7 = new PictureBox();
            picCoin8 = new PictureBox();
            picCoin9 = new PictureBox();
            picCoin10 = new PictureBox();
            picCoin11 = new PictureBox();
            picCoin12 = new PictureBox();
            picCoin13 = new PictureBox();
            picCoin14 = new PictureBox();
            picCoin15 = new PictureBox();
            picCoin17 = new PictureBox();
            picCoin16 = new PictureBox();
            picPlayer = new PictureBox();
            picEnemy1 = new PictureBox();
            picEnemy2 = new PictureBox();
            lblUsername = new Label();
            ((System.ComponentModel.ISupportInitialize)picDoor).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCoin20).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCoin18).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCoin19).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCoin1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCoin2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCoin3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCoin4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCoin5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCoin6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCoin7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCoin8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCoin9).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCoin10).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCoin11).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCoin12).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCoin13).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCoin14).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCoin15).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCoin17).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCoin16).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picPlayer).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picEnemy1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picEnemy2).BeginInit();
            SuspendLayout();
            // 
            // lblScore
            // 
            lblScore.AutoSize = true;
            lblScore.Font = new Font("Segoe UI", 15F);
            lblScore.Location = new Point(0, 0);
            lblScore.Name = "lblScore";
            lblScore.Size = new Size(65, 28);
            lblScore.TabIndex = 0;
            lblScore.Text = "Score:";
            // 
            // lblLogout
            // 
            lblLogout.AutoSize = true;
            lblLogout.Font = new Font("Segoe UI", 15F);
            lblLogout.Location = new Point(642, 25);
            lblLogout.Name = "lblLogout";
            lblLogout.Size = new Size(75, 28);
            lblLogout.TabIndex = 2;
            lblLogout.Text = "Logout";
            lblLogout.Click += lblLogout_Click;
            // 
            // lblPlatform1
            // 
            lblPlatform1.BackColor = Color.Black;
            lblPlatform1.Location = new Point(0, 570);
            lblPlatform1.Margin = new Padding(0);
            lblPlatform1.Name = "lblPlatform1";
            lblPlatform1.Size = new Size(175, 15);
            lblPlatform1.TabIndex = 3;
            lblPlatform1.Tag = "platform";
            // 
            // lblPlatform5
            // 
            lblPlatform5.BackColor = Color.Black;
            lblPlatform5.Location = new Point(382, 318);
            lblPlatform5.Margin = new Padding(0);
            lblPlatform5.Name = "lblPlatform5";
            lblPlatform5.Size = new Size(219, 15);
            lblPlatform5.TabIndex = 4;
            lblPlatform5.Tag = "platform";
            // 
            // lblPlatform8
            // 
            lblPlatform8.BackColor = Color.Black;
            lblPlatform8.Location = new Point(291, 99);
            lblPlatform8.Margin = new Padding(0);
            lblPlatform8.Name = "lblPlatform8";
            lblPlatform8.Size = new Size(150, 15);
            lblPlatform8.TabIndex = 5;
            lblPlatform8.Tag = "platform";
            // 
            // lblPlatform7
            // 
            lblPlatform7.BackColor = Color.Black;
            lblPlatform7.Location = new Point(101, 228);
            lblPlatform7.Margin = new Padding(0);
            lblPlatform7.Name = "lblPlatform7";
            lblPlatform7.Size = new Size(260, 15);
            lblPlatform7.TabIndex = 6;
            lblPlatform7.Tag = "platform";
            // 
            // lblPlatform2
            // 
            lblPlatform2.BackColor = Color.Black;
            lblPlatform2.Location = new Point(191, 509);
            lblPlatform2.Margin = new Padding(0);
            lblPlatform2.Name = "lblPlatform2";
            lblPlatform2.Size = new Size(250, 15);
            lblPlatform2.TabIndex = 7;
            lblPlatform2.Tag = "platform";
            // 
            // lblPlatform4
            // 
            lblPlatform4.BackColor = Color.Black;
            lblPlatform4.Location = new Point(60, 450);
            lblPlatform4.Margin = new Padding(0);
            lblPlatform4.Name = "lblPlatform4";
            lblPlatform4.Size = new Size(88, 15);
            lblPlatform4.TabIndex = 8;
            lblPlatform4.Tag = "movingPlatform";
            // 
            // lblPlatform3
            // 
            lblPlatform3.BackColor = Color.Black;
            lblPlatform3.Location = new Point(182, 395);
            lblPlatform3.Margin = new Padding(0);
            lblPlatform3.Name = "lblPlatform3";
            lblPlatform3.Size = new Size(300, 15);
            lblPlatform3.TabIndex = 9;
            lblPlatform3.Tag = "platform";
            // 
            // lblPlatform6
            // 
            lblPlatform6.BackColor = Color.Black;
            lblPlatform6.Location = new Point(510, 270);
            lblPlatform6.Margin = new Padding(0);
            lblPlatform6.Name = "lblPlatform6";
            lblPlatform6.Size = new Size(70, 15);
            lblPlatform6.TabIndex = 10;
            lblPlatform6.Tag = "movingPlatform";
            // 
            // lblPlatform9
            // 
            lblPlatform9.BackColor = Color.Black;
            lblPlatform9.Location = new Point(10, 152);
            lblPlatform9.Margin = new Padding(0);
            lblPlatform9.Name = "lblPlatform9";
            lblPlatform9.Size = new Size(175, 15);
            lblPlatform9.TabIndex = 11;
            lblPlatform9.Tag = "platform";
            // 
            // picDoor
            // 
            picDoor.Image = Properties.Resources.door;
            picDoor.Location = new Point(10, 99);
            picDoor.Margin = new Padding(3, 2, 3, 2);
            picDoor.Name = "picDoor";
            picDoor.Size = new Size(62, 53);
            picDoor.SizeMode = PictureBoxSizeMode.StretchImage;
            picDoor.TabIndex = 15;
            picDoor.TabStop = false;
            picDoor.Tag = "door";
            // 
            // picCoin20
            // 
            picCoin20.Image = Properties.Resources.coin;
            picCoin20.Location = new Point(305, 69);
            picCoin20.Margin = new Padding(0);
            picCoin20.Name = "picCoin20";
            picCoin20.Size = new Size(30, 30);
            picCoin20.SizeMode = PictureBoxSizeMode.StretchImage;
            picCoin20.TabIndex = 19;
            picCoin20.TabStop = false;
            picCoin20.Tag = "coin";
            // 
            // picCoin18
            // 
            picCoin18.Image = Properties.Resources.coin;
            picCoin18.Location = new Point(400, 69);
            picCoin18.Margin = new Padding(0);
            picCoin18.Name = "picCoin18";
            picCoin18.Size = new Size(30, 30);
            picCoin18.SizeMode = PictureBoxSizeMode.Zoom;
            picCoin18.TabIndex = 20;
            picCoin18.TabStop = false;
            picCoin18.Tag = "coin";
            // 
            // picCoin19
            // 
            picCoin19.Image = Properties.Resources.coin;
            picCoin19.Location = new Point(352, 69);
            picCoin19.Margin = new Padding(0);
            picCoin19.Name = "picCoin19";
            picCoin19.Size = new Size(30, 30);
            picCoin19.SizeMode = PictureBoxSizeMode.Zoom;
            picCoin19.TabIndex = 21;
            picCoin19.TabStop = false;
            picCoin19.Tag = "coin";
            // 
            // tmrGame
            // 
            tmrGame.Interval = 40;
            tmrGame.Tick += tmrGame_Tick;
            // 
            // lblTime
            // 
            lblTime.AutoSize = true;
            lblTime.Font = new Font("Segoe UI", 15F);
            lblTime.Location = new Point(0, 25);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(63, 28);
            lblTime.TabIndex = 22;
            lblTime.Text = "Time: ";
            // 
            // picCoin1
            // 
            picCoin1.Image = Properties.Resources.coin;
            picCoin1.Location = new Point(212, 479);
            picCoin1.Margin = new Padding(0);
            picCoin1.Name = "picCoin1";
            picCoin1.Size = new Size(30, 30);
            picCoin1.SizeMode = PictureBoxSizeMode.Zoom;
            picCoin1.TabIndex = 23;
            picCoin1.TabStop = false;
            picCoin1.Tag = "coin";
            // 
            // picCoin2
            // 
            picCoin2.Image = Properties.Resources.coin;
            picCoin2.Location = new Point(258, 479);
            picCoin2.Margin = new Padding(0);
            picCoin2.Name = "picCoin2";
            picCoin2.Size = new Size(30, 30);
            picCoin2.SizeMode = PictureBoxSizeMode.Zoom;
            picCoin2.TabIndex = 24;
            picCoin2.TabStop = false;
            picCoin2.Tag = "coin";
            // 
            // picCoin3
            // 
            picCoin3.Image = Properties.Resources.coin;
            picCoin3.Location = new Point(305, 479);
            picCoin3.Margin = new Padding(0);
            picCoin3.Name = "picCoin3";
            picCoin3.Size = new Size(30, 30);
            picCoin3.SizeMode = PictureBoxSizeMode.Zoom;
            picCoin3.TabIndex = 25;
            picCoin3.TabStop = false;
            picCoin3.Tag = "coin";
            // 
            // picCoin4
            // 
            picCoin4.Image = Properties.Resources.coin;
            picCoin4.Location = new Point(89, 420);
            picCoin4.Margin = new Padding(0);
            picCoin4.Name = "picCoin4";
            picCoin4.Size = new Size(30, 30);
            picCoin4.SizeMode = PictureBoxSizeMode.Zoom;
            picCoin4.TabIndex = 26;
            picCoin4.TabStop = false;
            picCoin4.Tag = "coin";
            // 
            // picCoin5
            // 
            picCoin5.Image = Properties.Resources.coin;
            picCoin5.Location = new Point(89, 350);
            picCoin5.Margin = new Padding(0);
            picCoin5.Name = "picCoin5";
            picCoin5.Size = new Size(30, 30);
            picCoin5.SizeMode = PictureBoxSizeMode.Zoom;
            picCoin5.TabIndex = 27;
            picCoin5.TabStop = false;
            picCoin5.Tag = "coin";
            // 
            // picCoin6
            // 
            picCoin6.Image = Properties.Resources.coin;
            picCoin6.Location = new Point(233, 365);
            picCoin6.Margin = new Padding(0);
            picCoin6.Name = "picCoin6";
            picCoin6.Size = new Size(30, 30);
            picCoin6.SizeMode = PictureBoxSizeMode.Zoom;
            picCoin6.TabIndex = 28;
            picCoin6.TabStop = false;
            picCoin6.Tag = "coin";
            // 
            // picCoin7
            // 
            picCoin7.Image = Properties.Resources.coin;
            picCoin7.Location = new Point(291, 365);
            picCoin7.Margin = new Padding(0);
            picCoin7.Name = "picCoin7";
            picCoin7.Size = new Size(30, 30);
            picCoin7.SizeMode = PictureBoxSizeMode.Zoom;
            picCoin7.TabIndex = 29;
            picCoin7.TabStop = false;
            picCoin7.Tag = "coin";
            // 
            // picCoin8
            // 
            picCoin8.Image = Properties.Resources.coin;
            picCoin8.Location = new Point(348, 365);
            picCoin8.Margin = new Padding(0);
            picCoin8.Name = "picCoin8";
            picCoin8.Size = new Size(30, 30);
            picCoin8.SizeMode = PictureBoxSizeMode.Zoom;
            picCoin8.TabIndex = 30;
            picCoin8.TabStop = false;
            picCoin8.Tag = "coin";
            // 
            // picCoin9
            // 
            picCoin9.Image = Properties.Resources.coin;
            picCoin9.Location = new Point(395, 288);
            picCoin9.Margin = new Padding(0);
            picCoin9.Name = "picCoin9";
            picCoin9.Size = new Size(30, 30);
            picCoin9.SizeMode = PictureBoxSizeMode.Zoom;
            picCoin9.TabIndex = 31;
            picCoin9.TabStop = false;
            picCoin9.Tag = "coin";
            // 
            // picCoin10
            // 
            picCoin10.Image = Properties.Resources.coin;
            picCoin10.Location = new Point(472, 288);
            picCoin10.Margin = new Padding(0);
            picCoin10.Name = "picCoin10";
            picCoin10.Size = new Size(30, 30);
            picCoin10.SizeMode = PictureBoxSizeMode.Zoom;
            picCoin10.TabIndex = 32;
            picCoin10.TabStop = false;
            picCoin10.Tag = "coin";
            // 
            // picCoin11
            // 
            picCoin11.Image = Properties.Resources.coin;
            picCoin11.Location = new Point(550, 288);
            picCoin11.Margin = new Padding(0);
            picCoin11.Name = "picCoin11";
            picCoin11.Size = new Size(30, 30);
            picCoin11.SizeMode = PictureBoxSizeMode.Zoom;
            picCoin11.TabIndex = 33;
            picCoin11.TabStop = false;
            picCoin11.Tag = "coin";
            // 
            // picCoin12
            // 
            picCoin12.Image = Properties.Resources.coin;
            picCoin12.Location = new Point(626, 165);
            picCoin12.Margin = new Padding(0);
            picCoin12.Name = "picCoin12";
            picCoin12.Size = new Size(30, 30);
            picCoin12.SizeMode = PictureBoxSizeMode.Zoom;
            picCoin12.TabIndex = 34;
            picCoin12.TabStop = false;
            picCoin12.Tag = "coin";
            // 
            // picCoin13
            // 
            picCoin13.Image = Properties.Resources.coin;
            picCoin13.Location = new Point(537, 38);
            picCoin13.Margin = new Padding(0);
            picCoin13.Name = "picCoin13";
            picCoin13.Size = new Size(30, 30);
            picCoin13.SizeMode = PictureBoxSizeMode.Zoom;
            picCoin13.TabIndex = 35;
            picCoin13.TabStop = false;
            picCoin13.Tag = "coin";
            // 
            // picCoin14
            // 
            picCoin14.Image = Properties.Resources.coin;
            picCoin14.Location = new Point(275, 198);
            picCoin14.Margin = new Padding(0);
            picCoin14.Name = "picCoin14";
            picCoin14.Size = new Size(30, 30);
            picCoin14.SizeMode = PictureBoxSizeMode.Zoom;
            picCoin14.TabIndex = 36;
            picCoin14.TabStop = false;
            picCoin14.Tag = "coin";
            // 
            // picCoin15
            // 
            picCoin15.Image = Properties.Resources.coin;
            picCoin15.Location = new Point(118, 198);
            picCoin15.Margin = new Padding(0);
            picCoin15.Name = "picCoin15";
            picCoin15.Size = new Size(30, 30);
            picCoin15.SizeMode = PictureBoxSizeMode.Zoom;
            picCoin15.TabIndex = 37;
            picCoin15.TabStop = false;
            picCoin15.Tag = "coin";
            // 
            // picCoin17
            // 
            picCoin17.Image = Properties.Resources.coin;
            picCoin17.Location = new Point(101, 122);
            picCoin17.Margin = new Padding(0);
            picCoin17.Name = "picCoin17";
            picCoin17.Size = new Size(30, 30);
            picCoin17.SizeMode = PictureBoxSizeMode.Zoom;
            picCoin17.TabIndex = 38;
            picCoin17.TabStop = false;
            picCoin17.Tag = "coin";
            // 
            // picCoin16
            // 
            picCoin16.Image = Properties.Resources.coin;
            picCoin16.Location = new Point(145, 122);
            picCoin16.Margin = new Padding(0);
            picCoin16.Name = "picCoin16";
            picCoin16.Size = new Size(30, 30);
            picCoin16.SizeMode = PictureBoxSizeMode.Zoom;
            picCoin16.TabIndex = 39;
            picCoin16.TabStop = false;
            picCoin16.Tag = "coin";
            // 
            // picPlayer
            // 
            picPlayer.Image = Properties.Resources.player;
            picPlayer.Location = new Point(10, 525);
            picPlayer.Margin = new Padding(0);
            picPlayer.Name = "picPlayer";
            picPlayer.Size = new Size(32, 42);
            picPlayer.SizeMode = PictureBoxSizeMode.AutoSize;
            picPlayer.TabIndex = 40;
            picPlayer.TabStop = false;
            // 
            // picEnemy1
            // 
            picEnemy1.Image = Properties.Resources.enemy;
            picEnemy1.Location = new Point(381, 355);
            picEnemy1.Margin = new Padding(0);
            picEnemy1.Name = "picEnemy1";
            picEnemy1.Size = new Size(40, 40);
            picEnemy1.SizeMode = PictureBoxSizeMode.AutoSize;
            picEnemy1.TabIndex = 41;
            picEnemy1.TabStop = false;
            // 
            // picEnemy2
            // 
            picEnemy2.Image = Properties.Resources.enemy;
            picEnemy2.Location = new Point(151, 188);
            picEnemy2.Margin = new Padding(0);
            picEnemy2.Name = "picEnemy2";
            picEnemy2.Size = new Size(40, 40);
            picEnemy2.SizeMode = PictureBoxSizeMode.AutoSize;
            picEnemy2.TabIndex = 42;
            picEnemy2.TabStop = false;
            // 
            // lblUsername
            // 
            lblUsername.Font = new Font("Segoe UI", 15F);
            lblUsername.ForeColor = SystemColors.Highlight;
            lblUsername.Location = new Point(510, 0);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(207, 28);
            lblUsername.TabIndex = 43;
            lblUsername.Text = "Username Goes Here";
            lblUsername.TextAlign = ContentAlignment.TopRight;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(719, 595);
            Controls.Add(lblUsername);
            Controls.Add(picEnemy2);
            Controls.Add(picEnemy1);
            Controls.Add(picPlayer);
            Controls.Add(picCoin16);
            Controls.Add(picCoin17);
            Controls.Add(picCoin15);
            Controls.Add(picCoin14);
            Controls.Add(picCoin13);
            Controls.Add(picCoin12);
            Controls.Add(picCoin11);
            Controls.Add(picCoin10);
            Controls.Add(picCoin9);
            Controls.Add(picCoin8);
            Controls.Add(picCoin7);
            Controls.Add(picCoin6);
            Controls.Add(picCoin5);
            Controls.Add(picCoin4);
            Controls.Add(picCoin3);
            Controls.Add(picCoin2);
            Controls.Add(picCoin1);
            Controls.Add(lblTime);
            Controls.Add(picCoin19);
            Controls.Add(picCoin18);
            Controls.Add(picCoin20);
            Controls.Add(picDoor);
            Controls.Add(lblPlatform9);
            Controls.Add(lblPlatform6);
            Controls.Add(lblPlatform3);
            Controls.Add(lblPlatform4);
            Controls.Add(lblPlatform2);
            Controls.Add(lblPlatform7);
            Controls.Add(lblPlatform8);
            Controls.Add(lblPlatform5);
            Controls.Add(lblPlatform1);
            Controls.Add(lblLogout);
            Controls.Add(lblScore);
            Margin = new Padding(3, 2, 3, 2);
            Name = "frmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "JumpGame";
            FormClosed += frmMain_FormClosed;
            KeyDown += KeyIsDown;
            KeyUp += KeyIsUp;
            ((System.ComponentModel.ISupportInitialize)picDoor).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCoin20).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCoin18).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCoin19).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCoin1).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCoin2).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCoin3).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCoin4).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCoin5).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCoin6).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCoin7).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCoin8).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCoin9).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCoin10).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCoin11).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCoin12).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCoin13).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCoin14).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCoin15).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCoin17).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCoin16).EndInit();
            ((System.ComponentModel.ISupportInitialize)picPlayer).EndInit();
            ((System.ComponentModel.ISupportInitialize)picEnemy1).EndInit();
            ((System.ComponentModel.ISupportInitialize)picEnemy2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblScore;
        private Label lblLogout;
        private Label lblPlatform1;
        private Label lblPlatform5;
        private Label lblPlatform8;
        private Label lblPlatform7;
        private Label lblPlatform2;
        private Label lblPlatform4;
        private Label lblPlatform3;
        private Label lblPlatform6;
        private Label lblPlatform9;
        private PictureBox picDoor;
        private PictureBox picCoin20;
        private PictureBox picCoin18;
        private PictureBox picCoin19;
        private System.Windows.Forms.Timer tmrGame;
        private Label lblTime;
        private PictureBox picCoin1;
        private PictureBox picCoin2;
        private PictureBox picCoin3;
        private PictureBox picCoin4;
        private PictureBox picCoin5;
        private PictureBox picCoin6;
        private PictureBox picCoin7;
        private PictureBox picCoin8;
        private PictureBox picCoin9;
        private PictureBox picCoin10;
        private PictureBox picCoin11;
        private PictureBox picCoin12;
        private PictureBox picCoin13;
        private PictureBox picCoin14;
        private PictureBox picCoin15;
        private PictureBox picCoin17;
        private PictureBox picCoin16;
        private PictureBox picPlayer;
        private PictureBox picEnemy1;
        private PictureBox picEnemy2;
        private Label lblUsername;
    }
}
