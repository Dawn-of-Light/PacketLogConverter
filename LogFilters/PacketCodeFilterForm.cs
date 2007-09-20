using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using PacketLogConverter.LogActions;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// Filters the log by packet codes
	/// </summary>
	[LogFilter("Packet code filter...", Shortcut.CtrlP, Priority=1000)]
	public class PacketCodeFilterForm : Form, ILogFilter
	{
		private GroupBox stocGroupBox;
		private GroupBox ctosGroupBox;
		private Button cancelButton;
		private Button stocSelectAllButton;
		private Button ctosSelectAllButton;
		private CheckedListBox stocCheckedListBox;
		private Button stocClearAllButton;
		private Button ctosClearAllButton;
		private CheckedListBox ctosCheckedListBox;
		private Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;
		private Button acceptButton;
		private Button ignoreAllButton;
		private Button allowAllbutton;
		private Button invertButton;
		private Button selectAllKnownButton;
		private ComboBox templateComboBox;
		private System.Windows.Forms.Button listSelectedButton;

		/// <summary>
		/// Saves the changes count
		/// </summary>
		private int m_changes;

		public PacketCodeFilterForm()
		{
			InitializeComponent();

			stocCheckedListBox.Items.Clear();
			ctosCheckedListBox.Items.Clear();
	
			for (int i = 0; i < Packet.MAX_CODE; i++)
			{
				string description;
				string delim = " - ";
				if (PacketManager.GetPacketTypesCount(i, ePacketDirection.ServerToClient) > 0)
					delim = ">>>";
				description = DefaultPacketDescriptions.GetDescription(i, ePacketDirection.ServerToClient);
				stocCheckedListBox.Items.Add("0x" + i.ToString("X2") + delim + description, false);

				delim = " - ";
				if (PacketManager.GetPacketTypesCount(i, ePacketDirection.ClientToServer) > 0)
					delim = ">>>";
				description = DefaultPacketDescriptions.GetDescription(i, ePacketDirection.ClientToServer);
				ctosCheckedListBox.Items.Add("0x" + i.ToString("X2") + delim + description, false);
			}

			templateComboBox.Items.Clear();
			templateComboBox.Items.Add("(select template)");
			templateComboBox.Items.Add(new KeepPacketsTemplate());
			templateComboBox.Items.Add(new SiegePacketsTemplate());
			templateComboBox.Items.Add(new HousePacketsTemplate());
			templateComboBox.Items.Add(new TextMessagePacketsTemplate());
			templateComboBox.Items.Add(new RegionChangePacketsTemplate());
			templateComboBox.Items.Add(new PetPacketsTemplate());
			templateComboBox.Items.Add(new BoatsPacketsTemplate());
			templateComboBox.Items.Add(new UnknownPacketsTemplate());
			templateComboBox.SelectedIndex = 0;

			this.stocCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.resetTemplate_Event);
			this.ctosCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.resetTemplate_Event);
		}

		#region templates

		private void resetTemplate_Event(object sender, ItemCheckEventArgs e)
		{
			templateComboBox.SelectedIndex = 0;
		}

		private abstract class FilterTemplate
		{
			public virtual void ActivateTemplate(CheckedListBox stoc, CheckedListBox ctos)
			{
				SetListBox(stoc, false);
				SetListBox(ctos, false);
			}
		}

		private class KeepPacketsTemplate : FilterTemplate
		{
			public override void ActivateTemplate(CheckedListBox stoc, CheckedListBox ctos)
			{
				base.ActivateTemplate(stoc, ctos);
				ctos.SetItemChecked(0x64, true);
				ctos.SetItemChecked(0x66, true);
				ctos.SetItemChecked(0x6F, true);
				stoc.SetItemChecked(0x49, true);
				stoc.SetItemChecked(0x61, true);
				stoc.SetItemChecked(0x62, true);
				stoc.SetItemChecked(0x63, true);
				stoc.SetItemChecked(0x65, true);
				stoc.SetItemChecked(0x67, true);
				stoc.SetItemChecked(0x69, true);
				stoc.SetItemChecked(0x6C, true);
				stoc.SetItemChecked(0x6D, true);
				stoc.SetItemChecked(0x6E, true);
			}

			public override string ToString()
			{
				return "Keep packets";
			}
		}

		private class SiegePacketsTemplate : FilterTemplate
		{
			public override void ActivateTemplate(CheckedListBox stoc, CheckedListBox ctos)
			{
				base.ActivateTemplate(stoc, ctos);
				ctos.SetItemChecked(0xF5, true);
				stoc.SetItemChecked(0x12, true);
				stoc.SetItemChecked(0xD9, true);
				stoc.SetItemChecked(0xE3, true);
//				stoc.SetItemChecked(0xA1, true);
				stoc.SetItemChecked(0xF5, true);
			}

			public override string ToString()
			{
				return "Siege packets";
			}
		}

		private class RegionChangePacketsTemplate: FilterTemplate
		{
			public override void ActivateTemplate(CheckedListBox stoc, CheckedListBox ctos)
			{
				base.ActivateTemplate(stoc, ctos);
				ctos.SetItemChecked(0x90, true);
				stoc.SetItemChecked(0x20, true);
				stoc.SetItemChecked(0xB1, true);
				stoc.SetItemChecked(0xB7, true);
			}

			public override string ToString()
			{
				return "ChangeRegion packets";
			}
		}

		private class HousePacketsTemplate : FilterTemplate
		{
			public override void ActivateTemplate(CheckedListBox stoc, CheckedListBox ctos)
			{
				base.ActivateTemplate(stoc, ctos);
				ctos.SetItemChecked(0x00, true);
				ctos.SetItemChecked(0x01, true); // ?
				ctos.SetItemChecked(0x03, true);
				ctos.SetItemChecked(0x05, true);
				ctos.SetItemChecked(0x06, true);
				ctos.SetItemChecked(0x07, true);
				ctos.SetItemChecked(0x09, true);
				ctos.SetItemChecked(0x0B, true);
				ctos.SetItemChecked(0x0C, true);
				ctos.SetItemChecked(0x0D, true);
				ctos.SetItemChecked(0x0E, true);
				ctos.SetItemChecked(0x18, true);
				stoc.SetItemChecked(0x03, true);
				stoc.SetItemChecked(0x05, true);
				stoc.SetItemChecked(0x08, true);
				stoc.SetItemChecked(0x09, true);
				stoc.SetItemChecked(0x0A, true);
				stoc.SetItemChecked(0x0F, true);
				stoc.SetItemChecked(0x18, true);
				stoc.SetItemChecked(0xD1, true);
				stoc.SetItemChecked(0xD2, true);
			}

			public override string ToString()
			{
				return "House packets";
			}
		}

		private class BoatsPacketsTemplate : FilterTemplate
		{
			public override void ActivateTemplate(CheckedListBox stoc, CheckedListBox ctos)
			{
				base.ActivateTemplate(stoc, ctos);
				ctos.SetItemChecked(0xC8, true);
				ctos.SetItemChecked(0xE4, true);
				stoc.SetItemChecked(0x12, true);
				stoc.SetItemChecked(0xC8, true);
				stoc.SetItemChecked(0xE5, true);
			}

			public override string ToString()
			{
				return "Boats packets";
			}
		}

		private class TextMessagePacketsTemplate : FilterTemplate
		{
			public override void ActivateTemplate(CheckedListBox stoc, CheckedListBox ctos)
			{
				base.ActivateTemplate(stoc, ctos);
				stoc.SetItemChecked(0xAF, true);
				stoc.SetItemChecked(0x4D, true);
				ctos.SetItemChecked(0xAF, true);
			}

			public override string ToString()
			{
				return "Text message packets";
			}
		}

		private class PetPacketsTemplate : FilterTemplate
		{
			public override void ActivateTemplate(CheckedListBox stoc, CheckedListBox ctos)
			{
				base.ActivateTemplate(stoc, ctos);
				ctos.SetItemChecked(0x8A, true);
				stoc.SetItemChecked(0x88, true);
			}

			public override string ToString()
			{
				return "Pet packets";
			}
		}

		private class UnknownPacketsTemplate: FilterTemplate
		{
			public override void ActivateTemplate(CheckedListBox stoc, CheckedListBox ctos)
			{
				base.ActivateTemplate(stoc, ctos);
				ctos.SetItemChecked(0x1D, true);
				ctos.SetItemChecked(0x40, true);
				ctos.SetItemChecked(0x41, true);
				ctos.SetItemChecked(0x42, true);
				ctos.SetItemChecked(0x43, true);
				ctos.SetItemChecked(0xB9, true);
				stoc.SetItemChecked(0x24, true);
				stoc.SetItemChecked(0x45, true);
				stoc.SetItemChecked(0x46, true);
				stoc.SetItemChecked(0x59, true);
				stoc.SetItemChecked(0x5C, true);
			}

			public override string ToString()
			{
				return "Unknown packets";
			}
		}

		private void templateComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			FilterTemplate template = templateComboBox.SelectedItem as FilterTemplate;
			if (template != null)
			{
				this.stocCheckedListBox.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.resetTemplate_Event);
				this.ctosCheckedListBox.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.resetTemplate_Event);
				template.ActivateTemplate(stocCheckedListBox, ctosCheckedListBox);
				this.stocCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.resetTemplate_Event);
				this.ctosCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.resetTemplate_Event);
			}
		}

		#endregion

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
			this.stocGroupBox = new System.Windows.Forms.GroupBox();
			this.stocCheckedListBox = new System.Windows.Forms.CheckedListBox();
			this.stocClearAllButton = new System.Windows.Forms.Button();
			this.stocSelectAllButton = new System.Windows.Forms.Button();
			this.ctosGroupBox = new System.Windows.Forms.GroupBox();
			this.ctosCheckedListBox = new System.Windows.Forms.CheckedListBox();
			this.ctosClearAllButton = new System.Windows.Forms.Button();
			this.ctosSelectAllButton = new System.Windows.Forms.Button();
			this.acceptButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.ignoreAllButton = new System.Windows.Forms.Button();
			this.allowAllbutton = new System.Windows.Forms.Button();
			this.invertButton = new System.Windows.Forms.Button();
			this.selectAllKnownButton = new System.Windows.Forms.Button();
			this.templateComboBox = new System.Windows.Forms.ComboBox();
			this.listSelectedButton = new System.Windows.Forms.Button();
			this.stocGroupBox.SuspendLayout();
			this.ctosGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// stocGroupBox
			// 
			this.stocGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.stocGroupBox.Controls.Add(this.stocCheckedListBox);
			this.stocGroupBox.Controls.Add(this.stocClearAllButton);
			this.stocGroupBox.Controls.Add(this.stocSelectAllButton);
			this.stocGroupBox.Location = new System.Drawing.Point(8, 0);
			this.stocGroupBox.Name = "stocGroupBox";
			this.stocGroupBox.Size = new System.Drawing.Size(368, 456);
			this.stocGroupBox.TabIndex = 0;
			this.stocGroupBox.TabStop = false;
			this.stocGroupBox.Text = "Server to Client allowed packets";
			// 
			// stocCheckedListBox
			// 
			this.stocCheckedListBox.CheckOnClick = true;
			this.stocCheckedListBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.stocCheckedListBox.Location = new System.Drawing.Point(8, 16);
			this.stocCheckedListBox.Name = "stocCheckedListBox";
			this.stocCheckedListBox.Size = new System.Drawing.Size(352, 388);
			this.stocCheckedListBox.TabIndex = 1;
			this.stocCheckedListBox.ThreeDCheckBoxes = true;
			// 
			// stocClearAllButton
			// 
			this.stocClearAllButton.Location = new System.Drawing.Point(184, 424);
			this.stocClearAllButton.Name = "stocClearAllButton";
			this.stocClearAllButton.Size = new System.Drawing.Size(176, 23);
			this.stocClearAllButton.TabIndex = 4;
			this.stocClearAllButton.Text = "Clear All";
			this.stocClearAllButton.Click += new System.EventHandler(this.stocClearAllButton_Click);
			// 
			// stocSelectAllButton
			// 
			this.stocSelectAllButton.Location = new System.Drawing.Point(8, 424);
			this.stocSelectAllButton.Name = "stocSelectAllButton";
			this.stocSelectAllButton.Size = new System.Drawing.Size(176, 23);
			this.stocSelectAllButton.TabIndex = 3;
			this.stocSelectAllButton.Text = "Select All";
			this.stocSelectAllButton.Click += new System.EventHandler(this.stocSelectAllButton_Click);
			// 
			// ctosGroupBox
			// 
			this.ctosGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.ctosGroupBox.Controls.Add(this.ctosCheckedListBox);
			this.ctosGroupBox.Controls.Add(this.ctosClearAllButton);
			this.ctosGroupBox.Controls.Add(this.ctosSelectAllButton);
			this.ctosGroupBox.Location = new System.Drawing.Point(384, 0);
			this.ctosGroupBox.Name = "ctosGroupBox";
			this.ctosGroupBox.Size = new System.Drawing.Size(368, 456);
			this.ctosGroupBox.TabIndex = 1;
			this.ctosGroupBox.TabStop = false;
			this.ctosGroupBox.Text = "Client to Server allowed packets";
			// 
			// ctosCheckedListBox
			// 
			this.ctosCheckedListBox.CheckOnClick = true;
			this.ctosCheckedListBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ctosCheckedListBox.Location = new System.Drawing.Point(8, 16);
			this.ctosCheckedListBox.Name = "ctosCheckedListBox";
			this.ctosCheckedListBox.Size = new System.Drawing.Size(352, 388);
			this.ctosCheckedListBox.TabIndex = 2;
			// 
			// ctosClearAllButton
			// 
			this.ctosClearAllButton.Location = new System.Drawing.Point(184, 424);
			this.ctosClearAllButton.Name = "ctosClearAllButton";
			this.ctosClearAllButton.Size = new System.Drawing.Size(176, 23);
			this.ctosClearAllButton.TabIndex = 6;
			this.ctosClearAllButton.Text = "Clear All";
			this.ctosClearAllButton.Click += new System.EventHandler(this.ctosClearAllButton_Click);
			// 
			// ctosSelectAllButton
			// 
			this.ctosSelectAllButton.Location = new System.Drawing.Point(8, 424);
			this.ctosSelectAllButton.Name = "ctosSelectAllButton";
			this.ctosSelectAllButton.Size = new System.Drawing.Size(176, 23);
			this.ctosSelectAllButton.TabIndex = 5;
			this.ctosSelectAllButton.Text = "Select All";
			this.ctosSelectAllButton.Click += new System.EventHandler(this.ctosSelectAllButton_Click);
			// 
			// acceptButton
			// 
			this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.acceptButton.Location = new System.Drawing.Point(584, 496);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.TabIndex = 0;
			this.acceptButton.Text = "Accept";
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(672, 496);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 11;
			this.cancelButton.Text = "Cancel";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 496);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(360, 23);
			this.label1.TabIndex = 4;
			this.label1.Text = "Codes with at least one packet parser are marked with \">>>\"";
			// 
			// ignoreAllButton
			// 
			this.ignoreAllButton.Location = new System.Drawing.Point(408, 464);
			this.ignoreAllButton.Name = "ignoreAllButton";
			this.ignoreAllButton.TabIndex = 7;
			this.ignoreAllButton.Text = "Ignore all";
			this.ignoreAllButton.Click += new System.EventHandler(this.ignoreAllButton_Click);
			// 
			// allowAllbutton
			// 
			this.allowAllbutton.Location = new System.Drawing.Point(496, 464);
			this.allowAllbutton.Name = "allowAllbutton";
			this.allowAllbutton.TabIndex = 8;
			this.allowAllbutton.Text = "Allow all";
			this.allowAllbutton.Click += new System.EventHandler(this.allowAllbutton_Click);
			// 
			// invertButton
			// 
			this.invertButton.Location = new System.Drawing.Point(584, 464);
			this.invertButton.Name = "invertButton";
			this.invertButton.TabIndex = 9;
			this.invertButton.Text = "Invert";
			this.invertButton.Click += new System.EventHandler(this.invertButton_Click);
			// 
			// selectAllKnownButton
			// 
			this.selectAllKnownButton.Location = new System.Drawing.Point(672, 464);
			this.selectAllKnownButton.Name = "selectAllKnownButton";
			this.selectAllKnownButton.TabIndex = 10;
			this.selectAllKnownButton.Text = "All known";
			this.selectAllKnownButton.Click += new System.EventHandler(this.selectAllKnownButton_Click);
			// 
			// templateComboBox
			// 
			this.templateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.templateComboBox.Location = new System.Drawing.Point(408, 496);
			this.templateComboBox.Name = "templateComboBox";
			this.templateComboBox.Size = new System.Drawing.Size(160, 21);
			this.templateComboBox.TabIndex = 12;
			this.templateComboBox.SelectedIndexChanged += new System.EventHandler(this.templateComboBox_SelectedIndexChanged);
			// 
			// listSelectedButton
			// 
			this.listSelectedButton.Location = new System.Drawing.Point(16, 464);
			this.listSelectedButton.Name = "listSelectedButton";
			this.listSelectedButton.Size = new System.Drawing.Size(96, 23);
			this.listSelectedButton.TabIndex = 7;
			this.listSelectedButton.Text = "List selected";
			this.listSelectedButton.Click += new System.EventHandler(this.listSelectedButton_Click);
			// 
			// PacketCodeFilterForm
			// 
			this.AcceptButton = this.acceptButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(762, 527);
			this.ControlBox = false;
			this.Controls.Add(this.templateComboBox);
			this.Controls.Add(this.selectAllKnownButton);
			this.Controls.Add(this.invertButton);
			this.Controls.Add(this.allowAllbutton);
			this.Controls.Add(this.ignoreAllButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.acceptButton);
			this.Controls.Add(this.ctosGroupBox);
			this.Controls.Add(this.stocGroupBox);
			this.Controls.Add(this.listSelectedButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PacketCodeFilterForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Packet code filter";
			this.stocGroupBox.ResumeLayout(false);
			this.ctosGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region select/clear all

		private static void SetListBox(CheckedListBox list, bool state)
		{
			for (int i = 0; i < list.Items.Count; ++i)
			{
				list.SetItemChecked(i, state);
			}
		}

		private void stocSelectAllButton_Click(object sender, EventArgs e)
		{
			SetListBox(stocCheckedListBox, true);
		}

		private void stocClearAllButton_Click(object sender, EventArgs e)
		{
			SetListBox(stocCheckedListBox, false);
		}

		private void ctosSelectAllButton_Click(object sender, EventArgs e)
		{
			SetListBox(ctosCheckedListBox, true);
		}

		private void ctosClearAllButton_Click(object sender, EventArgs e)
		{
			SetListBox(ctosCheckedListBox, false);
		}

		#endregion
		
		#region ILogFilter members

		/// <summary>
		/// Determines whether the packet should be ignored.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>
		/// 	<c>true</c> if packet should be ignored; otherwise, <c>false</c>.
		/// </returns>
		public bool IsPacketIgnored(Packet packet)
		{
			if (packet == null)
				return false;
			if (packet.Direction == ePacketDirection.ClientToServer)
			{
				return !ctosCheckedListBox.GetItemChecked(packet.Code);
			}
			else
			{
				return !stocCheckedListBox.GetItemChecked(packet.Code);
			}
		}

		/// <summary>
		/// Activates the filter.
		/// </summary>
		/// <returns><code>true</code> if filter has changed and log should be updated.</returns>
		public bool ActivateFilter()
		{
			// save all check boxes, all allowed by default
			BitArray stocSaved = new BitArray(Packet.MAX_CODE, true);
			BitArray ctosSaved = new BitArray(Packet.MAX_CODE, true);
			if (m_changes > 0)
			{
				for (int i = 0; i < Packet.MAX_CODE; i++)
				{
					stocSaved[i] = stocCheckedListBox.GetItemChecked(i);
					ctosSaved[i] = ctosCheckedListBox.GetItemChecked(i);
				}
			}

			if (ShowDialog() != DialogResult.OK)
			{
				if (m_changes <= 0)
					return false;
				// restore all check boxes if the dialog was canceled
				for (int i = 0; i < Packet.MAX_CODE; i++)
				{
					stocCheckedListBox.SetItemChecked(i, stocSaved[i]);
					ctosCheckedListBox.SetItemChecked(i, ctosSaved[i]);
				}
				return false;
			}

			m_changes++;

			bool changes = false;
			for (int i = 0; i < Packet.MAX_CODE; i++)
			{
				if (stocSaved[i] != stocCheckedListBox.GetItemChecked(i)
					|| ctosSaved[i] != ctosCheckedListBox.GetItemChecked(i))
				{
					changes = true;
					break;
				}
			}

			if (!changes)
			{
				return false;
			}

			if (IsFilterActive)
			{
				FilterManager.AddFilter(this);
			}
			else
			{
				FilterManager.RemoveFilter(this);
			}

			return true;
		}

		/// <summary>
		/// Gets a value indicating whether this instance is active.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is active; otherwise, <c>false</c>.
		/// </value>
		public bool IsFilterActive
		{
			get
			{
				if (m_changes <= 0)
					return false;
				for (int i = 0; i < Packet.MAX_CODE; i++)
				{
					if (!stocCheckedListBox.GetItemChecked(i) || !ctosCheckedListBox.GetItemChecked(i))
						return true;
				}
				return false;
			}
		}

		/// <summary>
		/// Serializes data of instance of this filter.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns><code>true</code> if filter is serialized, <code>false</code> otherwise.</returns>
		public bool Serialize(MemoryStream data)
		{
			// Serialize client-to-server packets
			for (int i = 0; i < Packet.MAX_CODE; i++)
			{
				data.WriteByte((byte)(ctosCheckedListBox.GetItemChecked(i) ? 1 : 0));
			}

			// Serialize server-to-client packets
			for (int i = 0; i < Packet.MAX_CODE; i++)
			{
				data.WriteByte((byte)(stocCheckedListBox.GetItemChecked(i) ? 1 : 0));
			}
			
			return true;
		}

		/// <summary>
		/// Deserializes data of instance of this filter.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns><code>true</code> if filter is deserialized, <code>false</code> otherwise.</returns>
		public bool Deserialize(MemoryStream data)
		{
			// Clear selected packets
			SetListBox(stocCheckedListBox, false);
			SetListBox(ctosCheckedListBox, false);

			// Deserialize client-to-server packets
			for (int i = 0; i < Packet.MAX_CODE; i++)
			{
				int active = data.ReadByte();
				ctosCheckedListBox.SetItemChecked(i, (active == 1 ? true : false));
			}

			// Deserialize server-to-client packets
			for (int i = 0; i < Packet.MAX_CODE; i++)
			{
				int active = data.ReadByte();
				stocCheckedListBox.SetItemChecked(i, (active == 1 ? true : false));
			}
			
			return true;
		}

		#endregion

		private void allowAllbutton_Click(object sender, EventArgs e)
		{
			SetListBox(stocCheckedListBox, true);
			SetListBox(ctosCheckedListBox, true);
		}

		private void ignoreAllButton_Click(object sender, EventArgs e)
		{
			SetListBox(stocCheckedListBox, false);
			SetListBox(ctosCheckedListBox, false);
		}

		private void invertButton_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < Packet.MAX_CODE; i++)
			{
				stocCheckedListBox.SetItemChecked(i, !stocCheckedListBox.GetItemChecked(i));
				ctosCheckedListBox.SetItemChecked(i, !ctosCheckedListBox.GetItemChecked(i));
			}
		}

		private void selectAllKnownButton_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < Packet.MAX_CODE; i++)
			{
				if (PacketManager.GetPacketTypesCount(i, ePacketDirection.ServerToClient) > 0)
				{
					stocCheckedListBox.SetItemChecked(i, true);
				}
				else
				{
					stocCheckedListBox.SetItemChecked(i, false);
				}

				if (PacketManager.GetPacketTypesCount(i, ePacketDirection.ClientToServer) > 0)
				{
					ctosCheckedListBox.SetItemChecked(i, true);
				}
				else
				{
					ctosCheckedListBox.SetItemChecked(i, false);
				}
			}
		}

		private void listSelectedButton_Click(object sender, EventArgs e)
		{
			InfoWindowForm info = new InfoWindowForm();

			StringBuilder str = new StringBuilder(1024);
			bool found = false;
			for (int i = 0; i < Packet.MAX_CODE; i++)
			{
				if (stocCheckedListBox.GetItemChecked(i))
				{
					if (!found)
					{
						found = true;
						str.Append("server to client packets:\n");
					}
					str.AppendFormat("0x{0:X2} - ", i);
					string scDesc = DefaultPacketDescriptions.GetDescription(i, ePacketDirection.ClientToServer);
					str.Append(scDesc == null ? "unknown" : scDesc);
					str.Append("\n");
				}
			}

			if (found)
				str.Append("\n\n");

			found = false;
			for (int i = 0; i < Packet.MAX_CODE; i++)
			{
				if (ctosCheckedListBox.GetItemChecked(i))
				{
					if (!found)
					{
						found = true;
						str.Append("client to server packets:\n");
					}
					str.AppendFormat("0x{0:X2} - ", i);
					string csDesc = DefaultPacketDescriptions.GetDescription(i, ePacketDirection.ClientToServer);
					str.Append(csDesc == null ? "unknown" : csDesc);
					str.Append("\n");
				}
			}

			if (found)
				str.Append("\n\n");

			info.InfoRichTextBox.Text = str.ToString();
			info.StartWindowThread();
		}
	}
}
