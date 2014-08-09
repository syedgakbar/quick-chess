using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Chess
{
	/// <summary>
	/// Summary description for NewGame.
	/// </summary>
	public class NewGame : System.Windows.Forms.Form
	{
        // Public variables

        public string ResourceFolderPath;      // This string gcontains the resoruce folder path, where all the external resources are stored
        public bool bStartGame;		        // True when we need to start the game

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnCancel;

		public System.Windows.Forms.PictureBox BlackPlayerImage;
		public System.Windows.Forms.RadioButton PlayersCvC;
		public System.Windows.Forms.RadioButton PlayersHvC;
		public System.Windows.Forms.RadioButton PlayesrHvH;
		public System.Windows.Forms.RadioButton PlayerLevel3;
		public System.Windows.Forms.RadioButton PlayerLevel2;
		public System.Windows.Forms.RadioButton PlayerLevel1;
		public System.Windows.Forms.TextBox BlackPlayerName;
		public System.Windows.Forms.TextBox WhitePlayerName;
		public System.Windows.Forms.PictureBox WhitePlayerImage;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NewGame()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewGame));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PlayersCvC = new System.Windows.Forms.RadioButton();
            this.PlayersHvC = new System.Windows.Forms.RadioButton();
            this.PlayesrHvH = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.PlayerLevel3 = new System.Windows.Forms.RadioButton();
            this.PlayerLevel2 = new System.Windows.Forms.RadioButton();
            this.PlayerLevel1 = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.BlackPlayerName = new System.Windows.Forms.TextBox();
            this.BlackPlayerImage = new System.Windows.Forms.PictureBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.WhitePlayerName = new System.Windows.Forms.TextBox();
            this.WhitePlayerImage = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BlackPlayerImage)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WhitePlayerImage)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.PlayersCvC);
            this.groupBox1.Controls.Add(this.PlayersHvC);
            this.groupBox1.Controls.Add(this.PlayesrHvH);
            this.groupBox1.Location = new System.Drawing.Point(24, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(160, 96);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Player Options";
            // 
            // PlayersCvC
            // 
            this.PlayersCvC.Location = new System.Drawing.Point(14, 70);
            this.PlayersCvC.Name = "PlayersCvC";
            this.PlayersCvC.Size = new System.Drawing.Size(146, 16);
            this.PlayersCvC.TabIndex = 2;
            this.PlayersCvC.Text = "Computer Vs. Computer";
            this.PlayersCvC.CheckedChanged += new System.EventHandler(this.PlayesrType_CheckedChanged);
            // 
            // PlayersHvC
            // 
            this.PlayersHvC.Location = new System.Drawing.Point(14, 47);
            this.PlayersHvC.Name = "PlayersHvC";
            this.PlayersHvC.Size = new System.Drawing.Size(136, 20);
            this.PlayersHvC.TabIndex = 1;
            this.PlayersHvC.Text = "Human Vs. Computer";
            this.PlayersHvC.CheckedChanged += new System.EventHandler(this.PlayesrType_CheckedChanged);
            // 
            // PlayesrHvH
            // 
            this.PlayesrHvH.Checked = true;
            this.PlayesrHvH.Location = new System.Drawing.Point(14, 24);
            this.PlayesrHvH.Name = "PlayesrHvH";
            this.PlayesrHvH.Size = new System.Drawing.Size(136, 20);
            this.PlayesrHvH.TabIndex = 0;
            this.PlayesrHvH.TabStop = true;
            this.PlayesrHvH.Text = "Human Vs. Human";
            this.PlayesrHvH.CheckedChanged += new System.EventHandler(this.PlayesrType_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.PlayerLevel3);
            this.groupBox2.Controls.Add(this.PlayerLevel2);
            this.groupBox2.Controls.Add(this.PlayerLevel1);
            this.groupBox2.Location = new System.Drawing.Point(24, 125);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(160, 91);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Player Level";
            // 
            // PlayerLevel3
            // 
            this.PlayerLevel3.Location = new System.Drawing.Point(13, 64);
            this.PlayerLevel3.Name = "PlayerLevel3";
            this.PlayerLevel3.Size = new System.Drawing.Size(139, 20);
            this.PlayerLevel3.TabIndex = 2;
            this.PlayerLevel3.Text = "Chess Master";
            // 
            // PlayerLevel2
            // 
            this.PlayerLevel2.Location = new System.Drawing.Point(13, 44);
            this.PlayerLevel2.Name = "PlayerLevel2";
            this.PlayerLevel2.Size = new System.Drawing.Size(139, 20);
            this.PlayerLevel2.TabIndex = 1;
            this.PlayerLevel2.Text = "Intermediate";
            // 
            // PlayerLevel1
            // 
            this.PlayerLevel1.Checked = true;
            this.PlayerLevel1.Location = new System.Drawing.Point(13, 24);
            this.PlayerLevel1.Name = "PlayerLevel1";
            this.PlayerLevel1.Size = new System.Drawing.Size(139, 20);
            this.PlayerLevel1.TabIndex = 0;
            this.PlayerLevel1.TabStop = true;
            this.PlayerLevel1.Text = "Beginner";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.BlackPlayerName);
            this.groupBox3.Controls.Add(this.BlackPlayerImage);
            this.groupBox3.Location = new System.Drawing.Point(192, 24);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(156, 95);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Black Player";
            // 
            // BlackPlayerName
            // 
            this.BlackPlayerName.Location = new System.Drawing.Point(67, 40);
            this.BlackPlayerName.Name = "BlackPlayerName";
            this.BlackPlayerName.Size = new System.Drawing.Size(80, 20);
            this.BlackPlayerName.TabIndex = 0;
            this.BlackPlayerName.Text = "Black Player";
            this.BlackPlayerName.Click += new System.EventHandler(this.PlayerName_Focus);
            this.BlackPlayerName.Enter += new System.EventHandler(this.PlayerName_Focus);
            // 
            // BlackPlayerImage
            // 
            this.BlackPlayerImage.BackColor = System.Drawing.Color.Transparent;
            this.BlackPlayerImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.BlackPlayerImage.Location = new System.Drawing.Point(14, 24);
            this.BlackPlayerImage.Name = "BlackPlayerImage";
            this.BlackPlayerImage.Size = new System.Drawing.Size(45, 50);
            this.BlackPlayerImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.BlackPlayerImage.TabIndex = 1;
            this.BlackPlayerImage.TabStop = false;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(277, 230);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(71, 24);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "&Start";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(197, 230);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(71, 24);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Transparent;
            this.groupBox4.Controls.Add(this.WhitePlayerName);
            this.groupBox4.Controls.Add(this.WhitePlayerImage);
            this.groupBox4.Location = new System.Drawing.Point(192, 124);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(156, 92);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "White Player";
            // 
            // WhitePlayerName
            // 
            this.WhitePlayerName.Location = new System.Drawing.Point(67, 40);
            this.WhitePlayerName.Name = "WhitePlayerName";
            this.WhitePlayerName.Size = new System.Drawing.Size(80, 20);
            this.WhitePlayerName.TabIndex = 0;
            this.WhitePlayerName.Text = "White Player";
            this.WhitePlayerName.Click += new System.EventHandler(this.PlayerName_Focus);
            this.WhitePlayerName.Enter += new System.EventHandler(this.PlayerName_Focus);
            // 
            // WhitePlayerImage
            // 
            this.WhitePlayerImage.BackColor = System.Drawing.Color.Transparent;
            this.WhitePlayerImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.WhitePlayerImage.Location = new System.Drawing.Point(14, 24);
            this.WhitePlayerImage.Name = "WhitePlayerImage";
            this.WhitePlayerImage.Size = new System.Drawing.Size(45, 50);
            this.WhitePlayerImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.WhitePlayerImage.TabIndex = 1;
            this.WhitePlayerImage.TabStop = false;
            // 
            // NewGame
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(374, 284);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewGame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Game";
            this.Load += new System.EventHandler(this.NewGame_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BlackPlayerImage)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WhitePlayerImage)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void btnStart_Click(object sender, System.EventArgs e)
		{
			bStartGame=true;
			this.Close();	// close the form
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			bStartGame=false;
			this.Close();	// close the form
		}

        private void PlayesrType_CheckedChanged(object sender, EventArgs e)
        {
            // Check the selected player types
            if (PlayesrHvH.Checked)
            {
                BlackPlayerImage.Image = System.Drawing.Image.FromFile(ResourceFolderPath + "User.jpg");
                WhitePlayerImage.Image = System.Drawing.Image.FromFile(ResourceFolderPath + "User_2.jpg");
            }
            else if (PlayersHvC.Checked)
            {
                BlackPlayerImage.Image = System.Drawing.Image.FromFile(ResourceFolderPath + "laptop.jpg");
                WhitePlayerImage.Image = System.Drawing.Image.FromFile(ResourceFolderPath + "User_2.jpg");
            }
            else if (PlayersCvC.Checked)
            {
                BlackPlayerImage.Image = System.Drawing.Image.FromFile(ResourceFolderPath + "laptop.jpg");
                WhitePlayerImage.Image = System.Drawing.Image.FromFile(ResourceFolderPath + "laptop_2.png");
            }
        }

        private void NewGame_Load(object sender, EventArgs e)
        {
            // Setup the Player vs Player images by default
            PlayesrType_CheckedChanged(null, null);
        }

        private void PlayerName_Focus(object sender, EventArgs e)
        {
            (sender as TextBox).Select(0, (sender as TextBox).Text.Length);
        }
	}
}
