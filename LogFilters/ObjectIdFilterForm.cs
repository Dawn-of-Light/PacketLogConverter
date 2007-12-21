using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using PacketLogConverter.LogPackets;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// Filter log packets by OID
	/// </summary>
	[LogFilter("OID filter...", Shortcut.CtrlI, Priority=900)]
	public class ObjectIdFilterForm : Form, ILogFilter
	{
		private ListBoxOid[] m_selectedOidsCache;

		private Label label1;
		private Button acceptButton;
		private Label label3;
		private TextBox oidInput;
		private TextBox oidCurrent;
		private CheckBox includeMessagesCheckBox;
		private System.Windows.Forms.Button clearButton;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.ListBox allowedOidListBox;
		private System.Windows.Forms.CheckBox enableFilterCheckBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		public ObjectIdFilterForm()
		{
			InitializeComponent();
			UpdateControls();

			// Cache entered OIDs
			FilterManager.FilteringStartedEvent +=
				delegate(IExecutionContext log)
					{
						if (IsFilterActive)
						{
							ListBoxOid[] oids = new ListBoxOid[allowedOidListBox.Items.Count];
							int i = 0;
							foreach (ListBoxOid oid in allowedOidListBox.Items)
							{
								oids[i++] = oid;
							}
							m_selectedOidsCache = oids;
						}
					};

			// Clear cached OIDs
			FilterManager.FilteringStoppedEvent +=
				delegate(IExecutionContext log)
					{
						m_selectedOidsCache = null;
					};
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
			this.label1 = new System.Windows.Forms.Label();
			this.oidInput = new System.Windows.Forms.TextBox();
			this.oidCurrent = new System.Windows.Forms.TextBox();
			this.acceptButton = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.includeMessagesCheckBox = new System.Windows.Forms.CheckBox();
			this.clearButton = new System.Windows.Forms.Button();
			this.removeButton = new System.Windows.Forms.Button();
			this.addButton = new System.Windows.Forms.Button();
			this.allowedOidListBox = new System.Windows.Forms.ListBox();
			this.enableFilterCheckBox = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 88);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Add oid:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// oidInput
			// 
			this.oidInput.Location = new System.Drawing.Point(80, 88);
			this.oidInput.Name = "oidInput";
			this.oidInput.Size = new System.Drawing.Size(72, 20);
			this.oidInput.TabIndex = 0;
			this.oidInput.Text = "";
			this.oidInput.TextChanged += new System.EventHandler(this.updateControls_Event);
			// 
			// oidCurrent
			// 
			this.oidCurrent.Location = new System.Drawing.Point(160, 88);
			this.oidCurrent.Name = "oidCurrent";
			this.oidCurrent.ReadOnly = true;
			this.oidCurrent.Size = new System.Drawing.Size(72, 20);
			this.oidCurrent.TabIndex = 4;
			this.oidCurrent.Text = "";
			// 
			// acceptButton
			// 
			this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.acceptButton.Location = new System.Drawing.Point(160, 216);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.TabIndex = 6;
			this.acceptButton.Text = "Accept";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(216, 48);
			this.label3.TabIndex = 8;
			this.label3.Text = "Filter for all packets that implement IObjectIdPacket interface. Packet is allowed if " +
				"either oid1 or oid2 is in the list.";
			// 
			// includeMessagesCheckBox
			// 
			this.includeMessagesCheckBox.Location = new System.Drawing.Point(16, 56);
			this.includeMessagesCheckBox.Name = "includeMessagesCheckBox";
			this.includeMessagesCheckBox.Size = new System.Drawing.Size(216, 16);
			this.includeMessagesCheckBox.TabIndex = 1;
			this.includeMessagesCheckBox.Text = "Include messages (0xAF, 0x4D)";
			// 
			// clearButton
			// 
			this.clearButton.Location = new System.Drawing.Point(160, 184);
			this.clearButton.Name = "clearButton";
			this.clearButton.TabIndex = 5;
			this.clearButton.Text = "Clear";
			this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
			// 
			// removeButton
			// 
			this.removeButton.Location = new System.Drawing.Point(160, 152);
			this.removeButton.Name = "removeButton";
			this.removeButton.TabIndex = 4;
			this.removeButton.Text = "Remove";
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// addButton
			// 
			this.addButton.Location = new System.Drawing.Point(160, 120);
			this.addButton.Name = "addButton";
			this.addButton.TabIndex = 3;
			this.addButton.Text = "Add";
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// allowedOidListBox
			// 
			this.allowedOidListBox.Location = new System.Drawing.Point(8, 120);
			this.allowedOidListBox.Name = "allowedOidListBox";
			this.allowedOidListBox.Size = new System.Drawing.Size(144, 134);
			this.allowedOidListBox.TabIndex = 2;
			this.allowedOidListBox.SelectedIndexChanged += new System.EventHandler(this.updateControls_Event);
			// 
			// enableFilterCheckBox
			// 
			this.enableFilterCheckBox.Location = new System.Drawing.Point(160, 240);
			this.enableFilterCheckBox.Name = "enableFilterCheckBox";
			this.enableFilterCheckBox.Size = new System.Drawing.Size(75, 24);
			this.enableFilterCheckBox.TabIndex = 7;
			this.enableFilterCheckBox.Text = "Enable";
			// 
			// ObjectIdFilterForm
			// 
			this.AcceptButton = this.acceptButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(242, 271);
			this.ControlBox = false;
			this.Controls.Add(this.enableFilterCheckBox);
			this.Controls.Add(this.allowedOidListBox);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.removeButton);
			this.Controls.Add(this.clearButton);
			this.Controls.Add(this.includeMessagesCheckBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.acceptButton);
			this.Controls.Add(this.oidCurrent);
			this.Controls.Add(this.oidInput);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ObjectIdFilterForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Object ID filter";
			this.ResumeLayout(false);

		}
		#endregion

		#region ILogFilter Members

		/// <summary>
		/// Activates the filter.
		/// </summary>
		/// <returns><code>true</code> if filter has changed and log should be updated.</returns>
		public bool ActivateFilter()
		{
			bool oldEnabled = enableFilterCheckBox.Checked;

			ArrayList oldAllowed = new ArrayList();
			for (int i = 0; i < allowedOidListBox.Items.Count; i++)
			{
				object item = (object)allowedOidListBox.Items[i];
				oldAllowed.Add(item);
			}

			bool oldIncludeMessages = includeMessagesCheckBox.Checked;

			oidInput.Focus();
			oidInput.SelectAll();

			ShowDialog();

			if (IsFilterActive)
			{
				FilterManager.AddFilter(this);
			}
			else
			{
				FilterManager.RemoveFilter(this);
				return false;
			}

			for (int i = 0; i < allowedOidListBox.Items.Count; i++)
			{
				object item = (object)allowedOidListBox.Items[i];
				if (!oldAllowed.Contains(item))
					return true;
			}

			return oldAllowed.Count != allowedOidListBox.Items.Count || oldIncludeMessages != includeMessagesCheckBox.Checked || oldEnabled != enableFilterCheckBox.Checked;
		}

		/// <summary>
		/// Determines whether the packet should be ignored.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>
		/// 	<c>true</c> if packet should be ignored; otherwise, <c>false</c>.
		/// </returns>
		public bool IsPacketIgnored(Packet packet)
		{
			// Check whether message packets should be included
			if (includeMessagesCheckBox.Checked)
			{
				if (packet is StoC_0xAF_Message || packet is StoC_0x4D_SpellMessage_174)
				{
					return false;
				}
			}

			// Session Id
			ISessionIdPacket spak = packet as ISessionIdPacket;
			if (spak != null)
			{
				foreach (ListBoxOid boxOid in m_selectedOidsCache)
				{
					if (spak.SessionId == boxOid.oid)
					{
						return false;
					}
				}
			}

			// Object Ids
			IObjectIdPacket pak = packet as IObjectIdPacket;
			if (pak != null)
			{
				// Check all entered OIDs
				foreach (ListBoxOid boxOid in m_selectedOidsCache)
				{
					// ...in packet's list of OIDs
					foreach (ushort id in pak.ObjectIds)
					{
						if (id == boxOid.oid)
						{
							return false;
						}
					}
				}
			}

			// House Id
			IHouseIdPacket hpak = packet as IHouseIdPacket;
			if (hpak != null)
			{
				foreach (ListBoxOid boxOid in m_selectedOidsCache)
				{
					if (hpak.HouseId == boxOid.oid)
					{
						return false;
					}
				}
			}

			// Keep Ids
			IKeepIdPacket kpak = packet as IKeepIdPacket;
			if (kpak != null)
			{
				// Check all entered OIDs
				foreach (ListBoxOid boxOid in m_selectedOidsCache)
				{
					// ...in packet's list of OIDs
					foreach (ushort id in kpak.KeepIds)
					{
						if (id == boxOid.oid)
						{
							return false;
						}
					}
				}
			}
			
			return true;
		}

		private abstract class AbstractChecker<T> where T : Packet
		{
			protected T m_packet;

			/// <summary>
			/// Inits the checker.
			/// </summary>
			/// <param name="packet">The packet.</param>
			/// <returns><c>true</c> if checker is initialized successfully and should be used for checking, <c>false</c> otherwise</returns>
			public bool Init(Packet packet)
			{
				m_packet = packet as T;
				bool ret = (m_packet != null);
				return ret;
			}

			/// <summary>
			/// Disposes this instance.
			/// </summary>
			public void Dispose()
			{
				m_packet = null;
			}

			/// <summary>
			/// Checks the oid.
			/// </summary>
			/// <param name="oid">The oid.</param>
			/// <returns></returns>
			protected abstract bool CheckOid(ListBoxOid oid);
		}

		/// <summary>
		/// Gets a value indicating whether this instance is active.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is active; otherwise, <c>false</c>.
		/// </value>
		public bool IsFilterActive
		{
			get { return enableFilterCheckBox.Checked; }
		}

		/// <summary>
		/// Serializes data of instance of this filter.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns><code>true</code> if filter is serialized, <code>false</code> otherwise.</returns>
		public bool Serialize(MemoryStream data)
		{
			return false;
		}

		/// <summary>
		/// Deserializes data of instance of this filter.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns><code>true</code> if filter is deserialized, <code>false</code> otherwise.</returns>
		public bool Deserialize(MemoryStream data)
		{
			return false;
		}

		#endregion

		private void UpdateControls()
		{
			int oid;
			if (!Util.ParseInt(oidInput.Text, out oid))
			{
				oidCurrent.Text = "(none)";
				addButton.Enabled = false;
			}
			else
			{
				oidCurrent.Text = string.Format("0x{0:X4}", oid);
				addButton.Enabled = true;
			}
			removeButton.Enabled = allowedOidListBox.SelectedIndex >= 0;
			clearButton.Enabled = allowedOidListBox.Items.Count > 0;
			enableFilterCheckBox.Enabled = allowedOidListBox.Items.Count > 0;
			if (allowedOidListBox.Items.Count <= 0)
			{
				enableFilterCheckBox.Checked = false;
			}
		}

		private void updateControls_Event(object sender, EventArgs e)
		{
			UpdateControls();
		}

		private void addButton_Click(object sender, System.EventArgs e)
		{
			int oid;
			if (!Util.ParseInt(oidInput.Text, out oid))
				oid = -1;
			allowedOidListBox.Items.Add(new ListBoxOid(oid));
			oidInput.Text = "";
			UpdateControls();
			oidInput.Focus();
			oidInput.SelectAll();
		}

		private void removeButton_Click(object sender, System.EventArgs e)
		{
			if (allowedOidListBox.SelectedIndex < 0)
				return;
			allowedOidListBox.Items.RemoveAt(allowedOidListBox.SelectedIndex);
			UpdateControls();
		}

		private void clearButton_Click(object sender, System.EventArgs e)
		{
			allowedOidListBox.Items.Clear();
			UpdateControls();
		}

		private class ListBoxOid
		{
			public int oid;

			public ListBoxOid(int oid)
			{
				this.oid = oid;
			}

			public override string ToString()
			{
				return "0x" + oid.ToString("X4");
			}
		}
	}
}
