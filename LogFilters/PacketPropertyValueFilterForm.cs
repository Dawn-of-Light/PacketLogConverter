using System;
using System.Collections.Generic;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using PacketLogConverter.LogWriters;
using PacketLogConverter.Utils;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// Filter by packet property value
	/// </summary>
	[LogFilter("Packet property value...", Priority = 300)]
	public partial class PacketPropertyValueFilterForm : System.Windows.Forms.Form, ILogFilter
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox packetClassComboBox;
		private System.Windows.Forms.ComboBox classPropertyComboBox;
		private System.Windows.Forms.TextBox propertyValueTextBox;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button clearButton;
		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.ListBox filtersListBox;
		private System.Windows.Forms.CheckBox enableCheckBox;
		private System.Windows.Forms.TextBox propertyValueTypeTextBox;
		private System.Windows.Forms.Button modifyButton;
		private System.Windows.Forms.ComboBox relationsList;
		private System.Windows.Forms.ComboBox Condition;
		private System.Windows.Forms.VScrollBar filterScrollBar;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PacketPropertyValueFilterForm()
		{
			InitializeComponent();
			relationsList.SelectedIndex = 0;
			Condition.SelectedIndex = 0;
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
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.packetClassComboBox = new System.Windows.Forms.ComboBox();
			this.classPropertyComboBox = new System.Windows.Forms.ComboBox();
			this.propertyValueTextBox = new System.Windows.Forms.TextBox();
			this.filtersListBox = new System.Windows.Forms.ListBox();
			this.addButton = new System.Windows.Forms.Button();
			this.removeButton = new System.Windows.Forms.Button();
			this.clearButton = new System.Windows.Forms.Button();
			this.acceptButton = new System.Windows.Forms.Button();
			this.enableCheckBox = new System.Windows.Forms.CheckBox();
			this.propertyValueTypeTextBox = new System.Windows.Forms.TextBox();
			this.modifyButton = new System.Windows.Forms.Button();
			this.relationsList = new System.Windows.Forms.ComboBox();
			this.Condition = new System.Windows.Forms.ComboBox();
			this.filterScrollBar = new System.Windows.Forms.VScrollBar();
			this.SuspendLayout();
			//
			// label1
			//
			this.label1.Location = new System.Drawing.Point(40, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Packet class:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			// label2
			//
			this.label2.Location = new System.Drawing.Point(6, 30);
			this.label2.Name = "label2";
			this.label2.TabIndex = 1;
			this.label2.Text = "Class property:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			// label3
			//
			this.label3.Location = new System.Drawing.Point(6, 53);
			this.label3.Name = "label3";
			this.label3.TabIndex = 2;
			this.label3.Text = "Property value:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			// packetClassComboBox
			//
			this.packetClassComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right)));
			this.packetClassComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.packetClassComboBox.Font = new System.Drawing.Font("Courier New", 8F);
			this.packetClassComboBox.Location = new System.Drawing.Point(118, 8);
			this.packetClassComboBox.MaxDropDownItems = 32;
			this.packetClassComboBox.Name = "packetClassComboBox";
			this.packetClassComboBox.Size = new System.Drawing.Size(511, 22);
			this.packetClassComboBox.Sorted = true;
			this.packetClassComboBox.TabIndex = 3;
			this.packetClassComboBox.SelectedValueChanged += new System.EventHandler(this.packetClassComboBox_SelectedValueChanged);
			//
			// classPropertyComboBox
			//
			this.classPropertyComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right)));
			this.classPropertyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.classPropertyComboBox.Font = new System.Drawing.Font("Courier New", 8F);
			this.classPropertyComboBox.ItemHeight = 14;
			this.classPropertyComboBox.Location = new System.Drawing.Point(118, 31);
			this.classPropertyComboBox.MaxDropDownItems = 32;
			this.classPropertyComboBox.Name = "classPropertyComboBox";
			this.classPropertyComboBox.Size = new System.Drawing.Size(511, 22);
			this.classPropertyComboBox.TabIndex = 4;
			//
			// propertyValueTextBox
			//
			this.propertyValueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right)));
			this.propertyValueTextBox.Location = new System.Drawing.Point(118, 54);
			this.propertyValueTextBox.MaxLength = 256;
			this.propertyValueTextBox.Name = "propertyValueTextBox";
			this.propertyValueTextBox.Size = new System.Drawing.Size(393, 20);
			this.propertyValueTextBox.TabIndex = 5;
			this.propertyValueTextBox.Text = "";
			this.propertyValueTextBox.TextChanged += new System.EventHandler(this.propertyValueTextBox_TextChanged);
			//
			// filtersListBox
			//
			this.filtersListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
				| System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right)));
			this.filtersListBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.filtersListBox.ItemHeight = 15;
			this.filtersListBox.Location = new System.Drawing.Point(6, 77);
			this.filtersListBox.Name = "filtersListBox";
			this.filtersListBox.Size = new System.Drawing.Size(623, 184);
			this.filtersListBox.TabIndex = 6;
			this.filtersListBox.SelectedIndexChanged += new System.EventHandler(this.filtersListBox_SelectedIndexChanged);
			//
			// addButton
			//
			this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.addButton.Location = new System.Drawing.Point(6, 269);
			this.addButton.Name = "addButton";
			this.addButton.TabIndex = 7;
			this.addButton.Text = "&Add";
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			//
			// removeButton
			//
			this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.removeButton.Location = new System.Drawing.Point(81, 269);
			this.removeButton.Name = "removeButton";
			this.removeButton.TabIndex = 8;
			this.removeButton.Text = "&Remove";
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			//
			// clearButton
			//
			this.clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.clearButton.Location = new System.Drawing.Point(231, 269);
			this.clearButton.Name = "clearButton";
			this.clearButton.TabIndex = 9;
			this.clearButton.Text = "&Clear";
			this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
			//
			// acceptButton
			//
			this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.acceptButton.Location = new System.Drawing.Point(554, 269);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.TabIndex = 10;
			this.acceptButton.Text = "&Accept";
			//
			// enableCheckBox
			//
			this.enableCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.enableCheckBox.Location = new System.Drawing.Point(479, 273);
			this.enableCheckBox.Name = "enableCheckBox";
			this.enableCheckBox.Size = new System.Drawing.Size(59, 17);
			this.enableCheckBox.TabIndex = 11;
			this.enableCheckBox.Text = "&Enable";
			//
			// propertyValueTypeTextBox
			//
			this.propertyValueTypeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.propertyValueTypeTextBox.Location = new System.Drawing.Point(557, 54);
			this.propertyValueTypeTextBox.MaxLength = 16;
			this.propertyValueTypeTextBox.Name = "propertyValueTypeTextBox";
			this.propertyValueTypeTextBox.ReadOnly = true;
			this.propertyValueTypeTextBox.Size = new System.Drawing.Size(72, 20);
			this.propertyValueTypeTextBox.TabIndex = 12;
			this.propertyValueTypeTextBox.Text = "";
			//
			// modifyButton
			//
			this.modifyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.modifyButton.Location = new System.Drawing.Point(156, 269);
			this.modifyButton.Name = "modifyButton";
			this.modifyButton.TabIndex = 13;
			this.modifyButton.Text = "&Modify";
			this.modifyButton.Click += new System.EventHandler(this.modifyButton_Click);
			//
			// relationsList
			//
			this.relationsList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.relationsList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.relationsList.Items.AddRange(new object[] {
															   "==",
															   "!=",
															   ">",
															   "<",
															   "&&",
															   "!&",
															   "&="});
			this.relationsList.Location = new System.Drawing.Point(514, 54);
			this.relationsList.Name = "relationsList";
			this.relationsList.Size = new System.Drawing.Size(40, 21);
			this.relationsList.TabIndex = 15;
			this.relationsList.SelectedIndexChanged += new System.EventHandler(this.relationsList_SelectedIndexChanged);
			//
			// Condition
			//
			this.Condition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.Condition.Items.AddRange(new object[] {
														   "OR",
														   "AND",
														   "OR("});
			this.Condition.Location = new System.Drawing.Point(8, 8);
			this.Condition.Name = "Condition";
			this.Condition.Size = new System.Drawing.Size(48, 21);
			this.Condition.TabIndex = 16;
			this.Condition.Visible = false;
			this.Condition.SelectedIndexChanged += new System.EventHandler(this.Condition_SelectedIndexChanged);
			//
			// filterScrollBar
			//
			this.filterScrollBar.LargeChange = 1;
			this.filterScrollBar.Location = new System.Drawing.Point(8, 48);
			this.filterScrollBar.Maximum = 1;
			this.filterScrollBar.Minimum = -1;
			this.filterScrollBar.Name = "filterScrollBar";
			this.filterScrollBar.Size = new System.Drawing.Size(17, 23);
			this.filterScrollBar.TabIndex = 17;
			this.filterScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.filterScrollBar_Scroll);
			//
			// PacketPropertyValueFilterForm
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(641, 304);
			this.ControlBox = false;
			this.Controls.Add(this.filterScrollBar);
			this.Controls.Add(this.Condition);
			this.Controls.Add(this.relationsList);
			this.Controls.Add(this.modifyButton);
			this.Controls.Add(this.propertyValueTypeTextBox);
			this.Controls.Add(this.enableCheckBox);
			this.Controls.Add(this.acceptButton);
			this.Controls.Add(this.clearButton);
			this.Controls.Add(this.removeButton);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.filtersListBox);
			this.Controls.Add(this.propertyValueTextBox);
			this.Controls.Add(this.classPropertyComboBox);
			this.Controls.Add(this.packetClassComboBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PacketPropertyValueFilterForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Packet property value filter";
			this.ResumeLayout(false);

		}
		#endregion

		#region ILogFilter Members

		/// <summary>
		/// Activates the filter.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// 	<code>true</code> if filter has changed and log should be updated.
		/// </returns>
		public bool ActivateFilter(IExecutionContext context)
		{
			InitPacketsList();

			bool oldEnable = enableCheckBox.Checked;
			ArrayList oldFilterList = new ArrayList(filtersListBox.Items);

			ShowDialog();

			if (IsFilterActive)
			{
				context.FilterManager.AddFilter(this);
			}
			else
			{
				context.FilterManager.RemoveFilter(this);
				return false;
			}

			// update the log data if changes to filters
			for (int i = 0; i < filtersListBox.Items.Count; i++)
			{
				object item = (object)filtersListBox.Items[i];
				if (!oldFilterList.Contains(item))
					return true;
			}

			return enableCheckBox.Checked != oldEnable || filtersListBox.Items.Count != oldFilterList.Count;
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
			// check logic always compared with previous condition current packet
			// condition "OR(" begin new (conditions) while again not meet "OR("
			// samples:
			// (A || (B && C)) = A "OR" B "AND" C
			// ((A || B) && C) = A "AND" C "OR(" B "AND" C
			// ((A && B) || C) = A "AND" B OR C = A "AND" B "OR(" C
			// ((A && B) || (C && D) = A "AND" B "OR(" C "AND" D
			// (((A && B) || C) && D) = A "AND" B "OR" C "AND" D
			bool state;
			int i = 0;
			bool packetIgnoreState = true;
			bool flagBadAndCheck = false;
			foreach (FilterListEntry entry in filtersListBox.Items)
			{
				if (entry.packetClass.type == null || entry.packetClass.type.IsAssignableFrom(packet.GetType()))
				{
					state = entry.IsPacketIgnored(packet);
					if (entry.condition == "OR(")
					{
						if (!packetIgnoreState) // if previouse Subcheck from this packet is not ignored then packet pass
							return false;
						flagBadAndCheck = false;
						i = 0; // begin new Subcheck
					}
					else if (flagBadAndCheck) // if previous AND check == false and this releation is not new Subcheck, then packets is ignored
					{
						return true;
					}
					if (i == 0)
						packetIgnoreState = state;
					else
					{
						if (entry.condition == "AND")
							packetIgnoreState |= state;
						else if (entry.condition == "OR")
							packetIgnoreState &= state;
					}
					if (packetIgnoreState && (i > 0) && entry.condition == "AND") // if this check is last in Subcheck then packet will be ignored
						flagBadAndCheck = true;
					i++;
				}
			}
			return packetIgnoreState;
		}

		/// <summary>
		/// Gets a value indicating whether this instance is active.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is active; otherwise, <c>false</c>.
		/// </value>
		public bool IsFilterActive
		{
			get { return enableCheckBox.Checked; }
		}

		/// <summary>
		/// Serializes data of instance of this filter.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns><code>true</code> if filter is serialized, <code>false</code> otherwise.</returns>
		public bool Serialize(MemoryStream data)
		{
			// Do not save empty filters
			if (filtersListBox.Items.Count == 0)
				return false;

			// Save all entries
			using (BinaryWriter binWriter = new BinaryWriter(data))
			{
				// Enabled flag
				binWriter.Write(enableCheckBox.Checked);

				BinaryFormatter serializer = new BinaryFormatter();

				// Count of entries
				binWriter.Write(filtersListBox.Items.Count);
				binWriter.Flush();

				// Content of entries
				foreach (FilterListEntry entry in filtersListBox.Items)
				{
					// Seralize info about packet class
					serializer.Serialize(data, entry);
				}
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
			// Clear current data
			filtersListBox.Items.Clear();

			// Load all entries
			using (BinaryReader binReader = new BinaryReader(data))
			{
				// Enabled flag
				enableCheckBox.Checked = binReader.ReadBoolean();

				BinaryFormatter serializer = new BinaryFormatter();
				for (int i = binReader.ReadInt32(); i > 0; i--)
				{
					FilterListEntry entry = (FilterListEntry) serializer.Deserialize(data);
					entry.InitNonSerialized();
					filtersListBox.Items.Add(entry);
				}
			}
			UpdateButtons();
			return true;
		}

		#endregion

		private bool m_initialized;

		private void InitPacketsList()
		{
			if (m_initialized)
				return;
			packetClassComboBox.Items.Clear();
			PacketClass anyClassItem = new PacketClass(null, null);
			packetClassComboBox.Items.Add(anyClassItem);

			foreach (DictionaryEntry entry in PacketManager.GetPacketTypesByAttribute())
			{
				LogPacketAttribute attr = (LogPacketAttribute)entry.Key;
				Type type = (Type)entry.Value;
				packetClassComboBox.Items.Add(new PacketClass(type, attr));
			}

			packetClassComboBox.SelectedItem = anyClassItem;
			UpdateClassPropertiesList();
			UpdateButtons();

			m_initialized = true;
		}

		// Create the Scroll event handler.
		private void filterScrollBar_Scroll(Object sender, ScrollEventArgs e)
		{
			if (filterScrollBar.Value != e.NewValue)
			{
				int newValue = e.NewValue;
				e.NewValue = 0;
				int index = filtersListBox.SelectedIndex;
				if (index + newValue < 0 || index + newValue >= filtersListBox.Items.Count)
					return;
				FilterListEntry entry = filtersListBox.SelectedItem as FilterListEntry;
				filtersListBox.Items[index] = filtersListBox.Items[index + newValue];
				filtersListBox.Items[index + newValue] = entry;
				filtersListBox.SelectedIndex = index + newValue;
				filtersListBoxChanges();
			}
		}

		private void packetClassComboBox_SelectedValueChanged(object sender, EventArgs e)
		{
			UpdateClassPropertiesList();
		}

		private void UpdateClassPropertiesList()
		{
			// Clear current list and add "any property" entries
			classPropertyComboBox.Items.Clear();
			classPropertyComboBox.Items.Add(new ClassMemberPath(null, false));
			classPropertyComboBox.Items.Add(new ClassMemberPath(null, true));

			BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
			PacketClass selectedClass = (PacketClass)packetClassComboBox.SelectedItem;
			Type classType = selectedClass.type;
			if (classType == null)
			{
				flags = flags & ~BindingFlags.DeclaredOnly;
				classType = typeof(Packet);
			}

			// Get list of paths to properties of a class
			int depth = RECURSIVE_SEARCH_DEPTH;
			List<List<MemberInfo>> classMembers = new List<List<MemberInfo>>();
			GetPropertiesAndFields(classType, depth, flags, classMembers, new List<MemberInfo>());

			// Add all properties to combobox
			foreach (List<MemberInfo> members in classMembers)
			{
				classPropertyComboBox.Items.Add(new ClassMemberPath(members, true));
			}

			classPropertyComboBox.SelectedIndex = 0;
		}

		#region buttons

		private void addButton_Click(object sender, System.EventArgs e)
		{
			// Convert string to integer
			string value = propertyValueTextBox.Text;
			long integer;
			if (Util.ParseLong(value, out integer))
			{
				value = integer.ToString();
			}

			ClassMemberPath selectedPath = (ClassMemberPath) classPropertyComboBox.SelectedItem;

			filtersListBox.Items.Add(new FilterListEntry(
			                         	(PacketClass)packetClassComboBox.SelectedItem,
			                         	selectedPath,
			                         	value,
										selectedPath.isRecursive,
			                         	relationsList.Text,
			                         	Condition.Text));
			UpdateButtons();
		}

		private void removeButton_Click(object sender, System.EventArgs e)
		{
			if (filtersListBox.SelectedIndex >= 0)
				filtersListBox.Items.RemoveAt(filtersListBox.SelectedIndex);
			UpdateButtons();
		}

		private void clearButton_Click(object sender, System.EventArgs e)
		{
			filtersListBox.Items.Clear();
			relationsList.SelectedIndex = 0;
			Condition.SelectedIndex = 0;
			UpdateButtons();
		}

		private void modifyButton_Click(object sender, System.EventArgs e)
		{
			int index = filtersListBox.SelectedIndex;
			if (index < 0 || index >= filtersListBox.Items.Count)
				return;

			// Convert string to integer
			string value = propertyValueTextBox.Text;
			long integer;
			if (Util.ParseLong(value, out integer))
			{
				value = integer.ToString();
			}

			ClassMemberPath selectedPath = (ClassMemberPath)classPropertyComboBox.SelectedItem;
			filtersListBox.Items.Insert(
				index,
				new FilterListEntry(
					(PacketClass)packetClassComboBox.SelectedItem,
					selectedPath,
					value,
					selectedPath.isRecursive,
					relationsList.Text,
					Condition.Text));
			filtersListBox.Items.RemoveAt(index+1);
			filtersListBox.SelectedIndex = index;
		}

		private void UpdateButtons()
		{
			modifyButton.Enabled = removeButton.Enabled = filtersListBox.SelectedIndex >= 0 && filtersListBox.SelectedIndex < filtersListBox.Items.Count;
			clearButton.Enabled = filtersListBox.Items.Count > 0;
			Condition.Enabled = Condition.Visible = (filtersListBox.Items.Count > 1) && filtersListBox.SelectedIndex >= 1 && filtersListBox.SelectedIndex < filtersListBox.Items.Count;
			filterScrollBar.Visible = (filtersListBox.Items.Count > 1);
			filterScrollBar.Enabled = filterScrollBar.Visible && filtersListBox.SelectedIndex >= 0 && filtersListBox.SelectedIndex < filtersListBox.Items.Count;
		}

		#endregion

		private void propertyValueTextBox_TextChanged(object sender, System.EventArgs e)
		{
			long value;
			if (Util.ParseLong(propertyValueTextBox.Text, out value))
				propertyValueTypeTextBox.Text = "0x" + value.ToString("X4");
			else
				propertyValueTypeTextBox.Text = "(string)";
		}

		#region filtersListBox

		private void filtersListBox_MouseClick(object sender, MouseEventArgs e)
		{
			filtersListBoxChanges();
		}

		private void filtersListBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			filtersListBoxChanges();
		}

		private void filtersListBoxChanges()
		{
			FilterListEntry entry = filtersListBox.SelectedItem as FilterListEntry;
			if (entry != null)
			{
				packetClassComboBox.SelectedItem = entry.packetClass;
				classPropertyComboBox.SelectedItem = entry.classProperty;
				propertyValueTextBox.Text = entry.valueToFind;
				relationsList.Text = entry.relation;
				Condition.SelectedItem = entry.condition;
			}
			UpdateButtons();
		}

		#endregion
		// It really needed ?
		private void relationsList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		}

		// It really needed ?
		private void Condition_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		}
	}
}
