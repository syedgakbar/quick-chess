using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Chess
{
	/// <summary>
	/// Summary description for SelectPiece.
	/// </summary>
	public class SelectPiece : System.Windows.Forms.Form
	{
		public int SelectedIndex;		// stores selected object index

		public System.Windows.Forms.PictureBox Piece1;
		public System.Windows.Forms.PictureBox Piece2;
		public System.Windows.Forms.PictureBox Piece3;
		public System.Windows.Forms.PictureBox Piece4;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SelectPiece()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SelectPiece));
			this.Piece1 = new System.Windows.Forms.PictureBox();
			this.Piece3 = new System.Windows.Forms.PictureBox();
			this.Piece4 = new System.Windows.Forms.PictureBox();
			this.Piece2 = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// Piece1
			// 
			this.Piece1.BackColor = System.Drawing.Color.Transparent;
			this.Piece1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Piece1.Location = new System.Drawing.Point(17, 17);
			this.Piece1.Name = "Piece1";
			this.Piece1.Size = new System.Drawing.Size(55, 55);
			this.Piece1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.Piece1.TabIndex = 0;
			this.Piece1.TabStop = false;
			this.Piece1.Click += new System.EventHandler(this.Piece_Click);
			// 
			// Piece3
			// 
			this.Piece3.BackColor = System.Drawing.Color.Transparent;
			this.Piece3.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Piece3.Location = new System.Drawing.Point(17, 72);
			this.Piece3.Name = "Piece3";
			this.Piece3.Size = new System.Drawing.Size(55, 55);
			this.Piece3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.Piece3.TabIndex = 1;
			this.Piece3.TabStop = false;
			this.Piece3.Click += new System.EventHandler(this.Piece_Click);
			// 
			// Piece4
			// 
			this.Piece4.BackColor = System.Drawing.Color.Transparent;
			this.Piece4.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Piece4.Location = new System.Drawing.Point(72, 72);
			this.Piece4.Name = "Piece4";
			this.Piece4.Size = new System.Drawing.Size(55, 55);
			this.Piece4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.Piece4.TabIndex = 3;
			this.Piece4.TabStop = false;
			this.Piece4.Click += new System.EventHandler(this.Piece_Click);
			// 
			// Piece2
			// 
			this.Piece2.BackColor = System.Drawing.Color.Transparent;
			this.Piece2.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Piece2.Location = new System.Drawing.Point(72, 17);
			this.Piece2.Name = "Piece2";
			this.Piece2.Size = new System.Drawing.Size(55, 55);
			this.Piece2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.Piece2.TabIndex = 2;
			this.Piece2.TabStop = false;
			this.Piece2.Click += new System.EventHandler(this.Piece_Click);
			// 
			// SelectPiece
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(145, 142);
			this.ControlBox = false;
			this.Controls.Add(this.Piece4);
			this.Controls.Add(this.Piece2);
			this.Controls.Add(this.Piece3);
			this.Controls.Add(this.Piece1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "SelectPiece";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Piece";
			this.ResumeLayout(false);

		}
		#endregion

		private void Piece_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.Control PictureCont= (System.Windows.Forms.Control )sender;
			SelectedIndex = System.Convert.ToInt16(PictureCont.Name.Substring("Piece".Length));	// strip the piece word and get the index
			this.Close();	// unload the form
		}
	}
}
