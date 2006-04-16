using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PacketLogConverter
{
	/// <summary>
	/// Summary description for ErrorForm.
	/// </summary>
	public class ErrorForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button OkButton;
		public System.Windows.Forms.TextBox errorTextBox;
		public System.Windows.Forms.TextBox descriptionTextBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ErrorForm()
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
			this.OkButton = new System.Windows.Forms.Button();
			this.errorTextBox = new System.Windows.Forms.TextBox();
			this.descriptionTextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// OkButton
			// 
			this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkButton.Location = new System.Drawing.Point(436, 16);
			this.OkButton.Name = "OkButton";
			this.OkButton.TabIndex = 0;
			this.OkButton.Text = "Ok";
			// 
			// errorTextBox
			// 
			this.errorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.errorTextBox.Location = new System.Drawing.Point(8, 56);
			this.errorTextBox.Multiline = true;
			this.errorTextBox.Name = "errorTextBox";
			this.errorTextBox.ReadOnly = true;
			this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.errorTextBox.Size = new System.Drawing.Size(500, 208);
			this.errorTextBox.TabIndex = 1;
			this.errorTextBox.Text = "";
			this.errorTextBox.WordWrap = false;
			// 
			// descriptionTextBox
			// 
			this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.descriptionTextBox.AutoSize = false;
			this.descriptionTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.descriptionTextBox.HideSelection = false;
			this.descriptionTextBox.Location = new System.Drawing.Point(24, 16);
			this.descriptionTextBox.Multiline = true;
			this.descriptionTextBox.Name = "descriptionTextBox";
			this.descriptionTextBox.ReadOnly = true;
			this.descriptionTextBox.Size = new System.Drawing.Size(396, 32);
			this.descriptionTextBox.TabIndex = 2;
			this.descriptionTextBox.Text = "";
			// 
			// ErrorForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(520, 273);
			this.Controls.Add(this.descriptionTextBox);
			this.Controls.Add(this.errorTextBox);
			this.Controls.Add(this.OkButton);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(300, 300);
			this.Name = "ErrorForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Error";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
