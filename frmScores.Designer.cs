namespace JumpGame
{
    partial class frmScores
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgvScores = new DataGridView();
            lblScores = new Label();
            btnExit = new Button();
            btnPlayAgain = new Button();
            btnLogout = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvScores).BeginInit();
            SuspendLayout();
            // 
            // dgvScores
            // 
            dgvScores.AllowUserToAddRows = false;
            dgvScores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvScores.Location = new Point(10, 45);
            dgvScores.Margin = new Padding(3, 2, 3, 2);
            dgvScores.Name = "dgvScores";
            dgvScores.ReadOnly = true;
            dgvScores.RowHeadersWidth = 51;
            dgvScores.Size = new Size(679, 236);
            dgvScores.TabIndex = 0;
            // 
            // lblScores
            // 
            lblScores.AutoSize = true;
            lblScores.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            lblScores.Location = new Point(270, 7);
            lblScores.Name = "lblScores";
            lblScores.Size = new Size(149, 32);
            lblScores.TabIndex = 1;
            lblScores.Text = "High Scores";
            // 
            // btnExit
            // 
            btnExit.Font = new Font("Segoe UI", 12F);
            btnExit.Location = new Point(298, 299);
            btnExit.Margin = new Padding(3, 2, 3, 2);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(105, 30);
            btnExit.TabIndex = 2;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // btnPlayAgain
            // 
            btnPlayAgain.Font = new Font("Segoe UI", 12F);
            btnPlayAgain.Location = new Point(187, 299);
            btnPlayAgain.Margin = new Padding(3, 2, 3, 2);
            btnPlayAgain.Name = "btnPlayAgain";
            btnPlayAgain.Size = new Size(105, 30);
            btnPlayAgain.TabIndex = 8;
            btnPlayAgain.Text = "Play Again";
            btnPlayAgain.UseVisualStyleBackColor = true;
            btnPlayAgain.Click += btnPlayAgain_Click;
            // 
            // btnLogout
            // 
            btnLogout.Font = new Font("Segoe UI", 12F);
            btnLogout.Location = new Point(409, 299);
            btnLogout.Margin = new Padding(3, 2, 3, 2);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(105, 30);
            btnLogout.TabIndex = 9;
            btnLogout.Text = "Logout";
            btnLogout.UseVisualStyleBackColor = true;
            btnLogout.Click += btnLogout_Click;
            // 
            // frmScores
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 338);
            Controls.Add(btnLogout);
            Controls.Add(btnPlayAgain);
            Controls.Add(btnExit);
            Controls.Add(lblScores);
            Controls.Add(dgvScores);
            Margin = new Padding(3, 2, 3, 2);
            Name = "frmScores";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Scores";
            FormClosed += frmScores_FormClosed;
            ((System.ComponentModel.ISupportInitialize)dgvScores).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvScores;
        private Label lblScores;
        private Button btnExit;
        private Button btnPlayAgain;
        private Button btnLogout;
    }
}