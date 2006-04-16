using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PacketLogConverter
{
	/// <summary>
	/// Summary description for ReportForm.
	/// </summary>
	public class ReportForm : System.Windows.Forms.Form
	{
		public System.Windows.Forms.Button buttonClose;
		public System.Windows.Forms.Button buttonReset;
		public System.Windows.Forms.RichTextBox Data;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ReportForm()
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
			this.buttonClose = new System.Windows.Forms.Button();
			this.buttonReset = new System.Windows.Forms.Button();
			this.Data = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// buttonClose
			// 
			this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonClose.Location = new System.Drawing.Point(216, 248);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.TabIndex = 0;
			this.buttonClose.Text = "Close";
			// 
			// buttonReset
			// 
			this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonReset.Location = new System.Drawing.Point(136, 248);
			this.buttonReset.Name = "buttonReset";
			this.buttonReset.TabIndex = 1;
			this.buttonReset.Text = "Reset";
			// 
			// Data
			// 
			this.Data.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Data.Location = new System.Drawing.Point(8, 8);
			this.Data.Name = "Data";
			this.Data.ReadOnly = true;
			this.Data.Size = new System.Drawing.Size(280, 232);
			this.Data.TabIndex = 2;
			this.Data.Text = "";
			// 
			// ReportForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.Data);
			this.Controls.Add(this.buttonReset);
			this.Controls.Add(this.buttonClose);
			this.MinimumSize = new System.Drawing.Size(300, 300);
			this.Name = "ReportForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Report";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
