namespace PacketLogConverter.LogFilters.PacketPropertyValueFilter
{
	partial class VariableDetailsForm
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
			this.packetLabel = new System.Windows.Forms.Label();
			this.classValLabel = new System.Windows.Forms.Label();
			this.classPropertyLabel = new System.Windows.Forms.Label();
			this.classPropertyValLabel = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.nameLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// packetLabel
			// 
			this.packetLabel.AutoSize = true;
			this.packetLabel.Location = new System.Drawing.Point(12, 9);
			this.packetLabel.Name = "packetLabel";
			this.packetLabel.Size = new System.Drawing.Size(44, 13);
			this.packetLabel.TabIndex = 0;
			this.packetLabel.Text = "Packet:";
			// 
			// classValLabel
			// 
			this.classValLabel.AutoSize = true;
			this.classValLabel.Location = new System.Drawing.Point(102, 9);
			this.classValLabel.Name = "classValLabel";
			this.classValLabel.Size = new System.Drawing.Size(72, 13);
			this.classValLabel.TabIndex = 1;
			this.classValLabel.Text = "classValLabel";
			// 
			// classPropertyLabel
			// 
			this.classPropertyLabel.AutoSize = true;
			this.classPropertyLabel.Location = new System.Drawing.Point(12, 22);
			this.classPropertyLabel.Name = "classPropertyLabel";
			this.classPropertyLabel.Size = new System.Drawing.Size(76, 13);
			this.classPropertyLabel.TabIndex = 2;
			this.classPropertyLabel.Text = "Class property:";
			// 
			// classPropertyValLabel
			// 
			this.classPropertyValLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.classPropertyValLabel.AutoSize = true;
			this.classPropertyValLabel.Location = new System.Drawing.Point(102, 22);
			this.classPropertyValLabel.Name = "classPropertyValLabel";
			this.classPropertyValLabel.Size = new System.Drawing.Size(111, 13);
			this.classPropertyValLabel.TabIndex = 3;
			this.classPropertyValLabel.Text = "classPropertyValLabel";
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.Location = new System.Drawing.Point(105, 41);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(137, 20);
			this.textBox1.TabIndex = 4;
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Location = new System.Drawing.Point(12, 44);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(38, 13);
			this.nameLabel.TabIndex = 5;
			this.nameLabel.Text = "Name:";
			// 
			// VariableDetailsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(254, 72);
			this.Controls.Add(this.nameLabel);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.classPropertyValLabel);
			this.Controls.Add(this.classPropertyLabel);
			this.Controls.Add(this.classValLabel);
			this.Controls.Add(this.packetLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(215, 97);
			this.Name = "VariableDetailsForm";
			this.Text = "Variable Details";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label packetLabel;
		private System.Windows.Forms.Label classValLabel;
		private System.Windows.Forms.Label classPropertyLabel;
		private System.Windows.Forms.Label classPropertyValLabel;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label nameLabel;
	}
}