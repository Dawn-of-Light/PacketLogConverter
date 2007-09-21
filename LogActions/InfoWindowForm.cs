using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace PacketLogConverter.LogActions
{
	public class InfoWindowForm : Form
	{
		protected RichTextBox infoRichTextBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		public InfoWindowForm()
		{
			InitializeComponent();
		}

		public RichTextBox InfoRichTextBox { get { return infoRichTextBox; } }

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
			this.infoRichTextBox = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// infoRichTextBox
			// 
			this.infoRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.infoRichTextBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.infoRichTextBox.Location = new System.Drawing.Point(0, 0);
			this.infoRichTextBox.Name = "infoRichTextBox";
			this.infoRichTextBox.ReadOnly = true;
			this.infoRichTextBox.Size = new System.Drawing.Size(294, 275);
			this.infoRichTextBox.TabIndex = 0;
			this.infoRichTextBox.Text = "";
			this.infoRichTextBox.WordWrap = false;
			this.infoRichTextBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.infoRichTextBox_MouseUp);
			// 
			// InfoWindowForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(294, 275);
			this.Controls.Add(this.infoRichTextBox);
			this.MinimumSize = new System.Drawing.Size(100, 100);
			this.Name = "InfoWindowForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "(right click to close)";
			this.TopMost = true;
			this.ResumeLayout(false);

		}
		#endregion


		/// <summary>
		/// Handles the MouseUp event of the infoRichTextBox control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
		private void infoRichTextBox_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				StopWindowThread();
			}
		}

		private Thread m_applicationThread;

		/// <summary>
		/// Starts the window thread.
		/// </summary>
		public void StartWindowThread()
		{
			lock (this)
			{
				if (m_applicationThread != null)
					return; // already started
				m_applicationThread = new Thread(new ThreadStart(AppThreadProc));
				m_applicationThread.Start();
			}
		}

		/// <summary>
		/// Stops the window thread.
		/// </summary>
		public void StopWindowThread()
		{
			lock (this)
			{
				if (m_applicationThread == null)
					return; // aready stopped
				
				// Process all window events before closing
				Application.ExitThread();

				m_applicationThread = null;
			}
		}

		/// <summary>
		/// Window event loop.
		/// </summary>
		private void AppThreadProc()
		{
			try
			{
				Application.Run(this);
			}
			catch (Exception e)
			{
				Log.Error("info window loop", e);
			}
		}
	}
}
