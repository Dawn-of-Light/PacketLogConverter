using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace PacketLogConverter
{
	/// <summary>
	/// Summary description for ProgressForm.
	/// </summary>
	public class ProgressForm : Form
	{
		private ProgressBar progressBar;
		private Label workProgressLabel;
		private Label descriptionLabel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		public ProgressForm()
		{
			InitializeComponent();
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
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.workProgressLabel = new System.Windows.Forms.Label();
			this.descriptionLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.progressBar.Location = new System.Drawing.Point(8, 48);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(376, 23);
			this.progressBar.TabIndex = 0;
			// 
			// workProgressLabel
			// 
			this.workProgressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.workProgressLabel.Location = new System.Drawing.Point(392, 48);
			this.workProgressLabel.Name = "workProgressLabel";
			this.workProgressLabel.Size = new System.Drawing.Size(120, 23);
			this.workProgressLabel.TabIndex = 2;
			this.workProgressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// descriptionLabel
			// 
			this.descriptionLabel.Location = new System.Drawing.Point(8, 8);
			this.descriptionLabel.Name = "descriptionLabel";
			this.descriptionLabel.Size = new System.Drawing.Size(504, 37);
			this.descriptionLabel.TabIndex = 3;
			this.descriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ProgressForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(522, 79);
			this.ControlBox = false;
			this.Controls.Add(this.descriptionLabel);
			this.Controls.Add(this.workProgressLabel);
			this.Controls.Add(this.progressBar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProgressForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Progress...";
			this.ResumeLayout(false);

		}
		#endregion

		public Label DescriptionLabel
		{
			get { return descriptionLabel; }
		}

		public Label WorkProgressLabel
		{
			get { return workProgressLabel; }
		}

		public ProgressBar ProgressBar
		{
			get { return progressBar; }
		}
	}
}
