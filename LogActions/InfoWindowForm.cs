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
			this.infoRichTextBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.infoRichTextBox.Location = new System.Drawing.Point(0, 0);
			this.infoRichTextBox.Name = "infoRichTextBox";
			this.infoRichTextBox.ReadOnly = true;
			this.infoRichTextBox.Size = new System.Drawing.Size(294, 275);
			this.infoRichTextBox.TabIndex = 0;
			this.infoRichTextBox.Text = "";
			this.infoRichTextBox.WordWrap = false;
			this.infoRichTextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.infoRichTextBox_MouseDown);
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

		private void infoRichTextBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				StopWindowThread();
				infoRichTextBox.Clear();
			}
		}

		private Thread m_applicationThread;

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

		public void StopWindowThread()
		{
			lock (this)
			{
				if (m_applicationThread == null)
					return; // aready stopped
				Close();
				m_applicationThread = null;
			}
		}

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
