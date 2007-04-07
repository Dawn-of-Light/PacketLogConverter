using System;
using System.Collections.Generic;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using PacketLogConverter.LogWriters;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// Filter by packet property value
	/// </summary>
	[LogFilter("Packet property value...", Priority = 300)]
	public class PacketPropertyValueFilterForm : System.Windows.Forms.Form, ILogFilter
	{
		private static readonly int RECURSIVE_SEARCH_DEPTH = 3;
		
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
		private CheckBox recursiveCheckBox;
		private System.Windows.Forms.ComboBox relationsList;
		private System.Windows.Forms.ComboBox Condition;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PacketPropertyValueFilterForm()
		{
			InitializeComponent();
			relationsList.SelectedIndex = 0;
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
			this.recursiveCheckBox = new System.Windows.Forms.CheckBox();
			this.relationsList = new System.Windows.Forms.ComboBox();
			this.Condition = new System.Windows.Forms.ComboBox();
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
			this.label2.Size = new System.Drawing.Size(100, 23);
			this.label2.TabIndex = 1;
			this.label2.Text = "Class property:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(6, 53);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 23);
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
			this.packetClassComboBox.Size = new System.Drawing.Size(424, 22);
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
			this.classPropertyComboBox.Size = new System.Drawing.Size(424, 22);
			this.classPropertyComboBox.TabIndex = 4;
			// 
			// propertyValueTextBox
			// 
			this.propertyValueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.propertyValueTextBox.Location = new System.Drawing.Point(118, 54);
			this.propertyValueTextBox.MaxLength = 256;
			this.propertyValueTextBox.Name = "propertyValueTextBox";
			this.propertyValueTextBox.Size = new System.Drawing.Size(306, 20);
			this.propertyValueTextBox.TabIndex = 5;
			this.propertyValueTextBox.TextChanged += new System.EventHandler(this.propertyValueTextBox_TextChanged);
			// 
			// filtersListBox
			// 
			this.filtersListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.filtersListBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.filtersListBox.ItemHeight = 15;
			this.filtersListBox.Location = new System.Drawing.Point(6, 77);
			this.filtersListBox.Name = "filtersListBox";
			this.filtersListBox.Size = new System.Drawing.Size(536, 169);
			this.filtersListBox.TabIndex = 6;
			this.filtersListBox.SelectedIndexChanged += new System.EventHandler(this.filtersListBox_SelectedIndexChanged);
			// 
			// addButton
			// 
			this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.addButton.Location = new System.Drawing.Point(6, 252);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(75, 23);
			this.addButton.TabIndex = 7;
			this.addButton.Text = "&Add";
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// removeButton
			// 
			this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.removeButton.Location = new System.Drawing.Point(81, 252);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(75, 23);
			this.removeButton.TabIndex = 8;
			this.removeButton.Text = "&Remove";
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// clearButton
			// 
			this.clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.clearButton.Location = new System.Drawing.Point(231, 252);
			this.clearButton.Name = "clearButton";
			this.clearButton.Size = new System.Drawing.Size(75, 23);
			this.clearButton.TabIndex = 9;
			this.clearButton.Text = "&Clear";
			this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
			// 
			// acceptButton
			// 
			this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.acceptButton.Location = new System.Drawing.Point(467, 252);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.Size = new System.Drawing.Size(75, 23);
			this.acceptButton.TabIndex = 10;
			this.acceptButton.Text = "&Accept";
			// 
			// enableCheckBox
			// 
			this.enableCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.enableCheckBox.AutoSize = true;
			this.enableCheckBox.Location = new System.Drawing.Point(392, 256);
			this.enableCheckBox.Name = "enableCheckBox";
			this.enableCheckBox.Size = new System.Drawing.Size(59, 17);
			this.enableCheckBox.TabIndex = 11;
			this.enableCheckBox.Text = "&Enable";
			// 
			// propertyValueTypeTextBox
			// 
			this.propertyValueTypeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.propertyValueTypeTextBox.Location = new System.Drawing.Point(470, 54);
			this.propertyValueTypeTextBox.MaxLength = 16;
			this.propertyValueTypeTextBox.Name = "propertyValueTypeTextBox";
			this.propertyValueTypeTextBox.ReadOnly = true;
			this.propertyValueTypeTextBox.Size = new System.Drawing.Size(72, 20);
			this.propertyValueTypeTextBox.TabIndex = 12;
			// 
			// modifyButton
			// 
			this.modifyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.modifyButton.Location = new System.Drawing.Point(156, 252);
			this.modifyButton.Name = "modifyButton";
			this.modifyButton.Size = new System.Drawing.Size(75, 23);
			this.modifyButton.TabIndex = 13;
			this.modifyButton.Text = "&Modify";
			this.modifyButton.Click += new System.EventHandler(this.modifyButton_Click);
			// 
			// recursiveCheckBox
			// 
			this.recursiveCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.recursiveCheckBox.AutoSize = true;
			this.recursiveCheckBox.Location = new System.Drawing.Point(312, 256);
			this.recursiveCheckBox.Name = "recursiveCheckBox";
			this.recursiveCheckBox.Size = new System.Drawing.Size(74, 17);
			this.recursiveCheckBox.TabIndex = 14;
			this.recursiveCheckBox.Text = "Re&cursive";
			this.recursiveCheckBox.UseVisualStyleBackColor = true;
			this.recursiveCheckBox.CheckedChanged += new System.EventHandler(this.recursiveCheckBox_CheckedChanged);
			// 
			// relationsList
			// 
			this.relationsList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.relationsList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.relationsList.Items.AddRange(new object[] {
            "==",
            "!="});
			this.relationsList.Location = new System.Drawing.Point(427, 54);
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
            "AND"});
			this.Condition.Location = new System.Drawing.Point(8, 8);
			this.Condition.Name = "Condition";
			this.Condition.Size = new System.Drawing.Size(48, 21);
			this.Condition.TabIndex = 16;
			this.Condition.Visible = false;
			this.Condition.SelectedIndexChanged += new System.EventHandler(this.Condition_SelectedIndexChanged);
			// 
			// PacketPropertyValueFilterForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(554, 287);
			this.ControlBox = false;
			this.Controls.Add(this.recursiveCheckBox);
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
			this.PerformLayout();

		}
		#endregion

		#region ILogFilter Members

		public bool ActivateFilter()
		{
			InitPacketsList();

			bool oldEnable = enableCheckBox.Checked;
			ArrayList oldFilterList = new ArrayList(filtersListBox.Items);

			ShowDialog();

			if (IsFilterActive)
				FilterManager.AddFilter(this);
			else
			{
				FilterManager.RemoveFilter(this);
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

		public bool IsPacketIgnored(Packet packet)
		{
			bool state;
			int i = 0;
			bool rc = false;
			foreach (FilterListEntry entry in filtersListBox.Items)
			{
				state = entry.IsPacketIgnored(packet);
				if (i == 0)
					rc = state;
				else
				{
					if (entry.condition == "AND")
						rc |= state;
					else if (entry.condition == "OR")
						rc &= state;
				}
				if (rc && (i > 0) && entry.condition == "AND")
					return true;
				i++;
			}
			return rc;
		}

		public bool IsFilterActive
		{
			get { return enableCheckBox.Checked; }
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

		private void packetClassComboBox_SelectedValueChanged(object sender, EventArgs e)
		{
			UpdateClassPropertiesList();
		}

		private void recursiveCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			UpdateClassPropertiesList();
		}

		private void UpdateClassPropertiesList()
		{
			// Clear current list and add "any property" entry
			classPropertyComboBox.Items.Clear();
			ClassMemberPath anyPropertyItem = new ClassMemberPath(null);
			classPropertyComboBox.Items.Add(anyPropertyItem);
			
			BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
			PacketClass selectedClass = (PacketClass)packetClassComboBox.SelectedItem;
			Type classType = selectedClass.type;
			if (classType == null)
			{
				flags = flags & ~BindingFlags.DeclaredOnly;
				classType = typeof(Packet);
			}

			// Get list of paths to properties of a class
			int depth = (recursiveCheckBox.Checked ? RECURSIVE_SEARCH_DEPTH : 0);
			List<List<MemberInfo>> classMembers = new List<List<MemberInfo>>();
			GetPropertiesAndFields(classType, depth, flags, classMembers, new List<MemberInfo>());
			
			// Add all properties to combobox
			foreach (List<MemberInfo> members in classMembers)
			{
				classPropertyComboBox.Items.Add(new ClassMemberPath(members));
			}

			classPropertyComboBox.SelectedItem = anyPropertyItem;
		}

		/// <summary>
		/// Gets paths to every property and field a class.
		/// </summary>
		/// <param name="clazz">The class to read properties for.</param>
		/// <param name="depth">How deep to read properties/fields. Zero means only properties/fields of specified class.</param>
		/// <param name="flags">The flags of property and field to read.</param>
		/// <param name="result">List with all paths.</param>
		/// <param name="curList">Path to current node.</param>
		private static void GetPropertiesAndFields(Type clazz, int depth, BindingFlags flags, List<List<MemberInfo>> result, List<MemberInfo> curList)
		{
			if (depth < 0)
			{
				return;
			}
			if (IsClassIgnored(clazz))
			{
				return;
			}

			// Get properties
			PropertyInfo[] properties = clazz.GetProperties(flags);
			foreach (PropertyInfo info in properties)
			{
				// Clone current path, add this node if property is readabe and is not indexer ("item[..]")
				if (!IsClassIgnored(info.PropertyType) && info.CanRead && info.GetIndexParameters().GetLength(0) == 0)
				{
					List<MemberInfo> newList = new List<MemberInfo>(curList);
					newList.Add(info);

					// Get sub-element type; can be array
					Type propType = info.PropertyType;
					if (propType.HasElementType)
					{
						propType = propType.GetElementType();
					}
					else
					{
						result.Add(newList);
					}
					GetPropertiesAndFields(propType, depth - 1, flags, result, newList);
				}
			}

			// Get fields
			FieldInfo[] fields = clazz.GetFields(flags);
			foreach (FieldInfo info in fields)
			{
				// Clone current path, add this node
				if (!IsClassIgnored(info.FieldType))
				{
					List<MemberInfo> newList = new List<MemberInfo>(curList);
					newList.Add(info);

					// Get sub-element type; can be array
					Type fieldType = info.FieldType;
					if (fieldType.HasElementType)
					{
						fieldType = fieldType.GetElementType();
					}
					else
					{
						result.Add(newList);
					}
					GetPropertiesAndFields(fieldType, depth - 1, flags, result, newList);
				}
			}
		}

		#region buttons

		private void addButton_Click(object sender, System.EventArgs e)
		{
			string value = propertyValueTextBox.Text;
			long integer;
			bool recursive = recursiveCheckBox.Checked;
			if (Util.ParseLong(value, out integer))
				value = integer.ToString();

			filtersListBox.Items.Add(new FilterListEntry((PacketClass)packetClassComboBox.SelectedItem, (ClassMemberPath)classPropertyComboBox.SelectedItem, value, recursive, relationsList.Text, Condition.Text));
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
			bool recursive = recursiveCheckBox.Checked;
			filtersListBox.Items.Insert(index, new FilterListEntry((PacketClass)packetClassComboBox.SelectedItem, (ClassMemberPath)classPropertyComboBox.SelectedItem, propertyValueTextBox.Text, recursive, relationsList.Text, Condition.Text));
			filtersListBox.Items.RemoveAt(index+1);
			filtersListBox.SelectedIndex = index;
		}

		private void UpdateButtons()
		{
			modifyButton.Enabled = removeButton.Enabled = filtersListBox.SelectedIndex >= 0 && filtersListBox.SelectedIndex < filtersListBox.Items.Count;
			clearButton.Enabled = filtersListBox.Items.Count > 0;
			Condition.Enabled = Condition.Visible = (filtersListBox.Items.Count > 1) && filtersListBox.SelectedIndex >= 1 && filtersListBox.SelectedIndex < filtersListBox.Items.Count;
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
				propertyValueTextBox.Text = entry.value;
				recursiveCheckBox.Checked = entry.isRecursive;
				relationsList.Text = entry.relation;
				Condition.Text = entry.condition;
			}
			UpdateButtons();
		}

		#endregion
		private void relationsList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		}

		private void Condition_SelectedIndexChanged(object sender, System.EventArgs e)
		{

		}


		#region data containers

		private class PacketClass
		{
			public readonly Type type;
			public readonly LogPacketAttribute attr;

			public PacketClass(Type type, LogPacketAttribute attr)
			{
				this.type = type;
				this.attr = attr;
			}

			public override string ToString()
			{
				if (type == null || attr == null)
					return "(any packet)";
				string dir = (attr.Direction == ePacketDirection.ServerToClient ? "S=>C" : "S<=C");
				string desc = (attr.Description != null ? attr.Description : DefaultPacketDescriptions.GetDescription(attr.Code, attr.Direction));
				return string.Format("{0} 0x{1:X2}: \"{2}\"", dir, attr.Code, desc);
			}
		}

		/// <summary>
		/// This class is used in combo box to show path to single member of a class (packet).
		/// </summary>
		private class ClassMemberPath
		{
			public readonly List<MemberInfo> members;
			private readonly string fullName;

			/// <summary>
			/// Initializes a new instance of the <see cref="T:ClassMemberPath"/> class.
			/// </summary>
			/// <param name="members">Path to single member.</param>
			public ClassMemberPath(List<MemberInfo> members)
			{
				this.members = members;
				
				if (members == null)
				{
					fullName = "(any property)";
				}
				else
				{
					// Construct full path to property
					StringBuilder str = new StringBuilder(members.Count * 16);
					foreach (MemberInfo info in members)
					{
						if (str.Length > 0)
						{
							str.Append('.');
						}
						str.Append(info.Name);
						
						// Check if member is an array
						if ((info is PropertyInfo && ((PropertyInfo)info).PropertyType.IsArray)
							|| (info is FieldInfo && ((FieldInfo)info).FieldType.IsArray))
						{
							str.Append("[]");
						}
					}
					fullName = str.ToString();
				}
			}

			/// <summary>
			/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
			/// </summary>
			/// <returns>
			/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
			/// </returns>
			public override string ToString()
			{
				return fullName;
			}

			///<summary>
			///Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
			///</summary>
			///
			///<returns>
			///true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
			///</returns>
			///
			///<param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>. </param><filterpriority>2</filterpriority>
			public override bool Equals(object obj)
			{
				// Needed for proper list updates
				ClassMemberPath prop = obj as ClassMemberPath;
				bool bRet = prop != null;
				if (bRet)
				{
					if (prop.members == members)
					{
						bRet = true;
					}
					else if (prop.members != null && members != null && prop.members.Count == members.Count)
					{
						// Check every element of collections
						for(int i = members.Count - 1; i >= 0; i--)
						{
							if (!members[i].Equals(prop.members[i]))
							{
								bRet = false;
								break;
							}
						}
					}
					else
					{
						bRet = false;
					}
				}
				return bRet;
			}

			///<summary>
			///Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
			///</summary>
			///
			///<returns>
			///A hash code for the current <see cref="T:System.Object"></see>.
			///</returns>
			///<filterpriority>2</filterpriority>
			public override int GetHashCode()
			{
				if (members == null)
					return 0;
				return members.Count;
			}
		}

		private class FilterListEntry
		{
			public readonly PacketClass packetClass;
			public readonly ClassMemberPath classProperty;
			public readonly string value;
			public readonly bool isRecursive;
			public readonly string relation;
			public readonly string condition;
			public static readonly ArrayList m_ignoredProperties = new ArrayList();

			static FilterListEntry()
			{
				Type ignoredType = typeof(MemoryStream);
				foreach (PropertyInfo property in ignoredType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
				{
					m_ignoredProperties.Add(property);
				}
			}

			public FilterListEntry(PacketClass packetClass, ClassMemberPath classProperty, string value, bool isRecursive, string relation, string condition)
			{
				this.packetClass = packetClass;
				this.classProperty = classProperty;
				this.value = value.ToLower();
				this.isRecursive = isRecursive;
				this.relation = relation;
				this.condition = condition;
			}

			public override string ToString()
			{
				string rec = (isRecursive && classProperty.members == null ? " (rec)" : "");
				return string.Format("{5,-3} {0} [{1}] {4} \"{2}\"{3}", packetClass, classProperty, value, rec, relation, condition);
			}

			public bool IsPacketIgnored(Packet packet)
			{
				bool isIgnored = true;
				
				// Filter by packet type
				if (packetClass.type != null && !packetClass.type.IsAssignableFrom(packet.GetType()))
				{
					isIgnored = true;
				}
				else
				{
					int depth = (isRecursive ? RECURSIVE_SEARCH_DEPTH : 0);
					isIgnored = IsValueIgnored(packet, classProperty.members, depth);
				}

				return isIgnored;
			}

			private bool IsValueIgnored(object val, List<MemberInfo> path, int depth)
			{
				// Limit depth
				if (depth < 0)
				{
					return true;
				}

				bool isIgnored = true;
				
				if (val != null)
				{
					// Ignore certain types for better performance
					Type valType = val.GetType();
					if (IsClassIgnored(valType))
					{
						isIgnored = true;
					}
					else if (valType.IsPublic || valType.IsNestedPublic)
					{
						if (path != null)
						{
							// Make sure that depth is same as path length
							if (depth >= path.Count)
							{
								depth = path.Count - 1;
							}
							
							// Get data from property or field
							MemberInfo node = path[path.Count - 1 - depth];
							object data = null;
							if (node is PropertyInfo)
							{
								PropertyInfo propInfo = (PropertyInfo) node;
								data = propInfo.GetValue(val, null);
							}
							else if (node is FieldInfo)
							{
								FieldInfo fieldInfo = (FieldInfo) node;
								data = fieldInfo.GetValue(val);
							}
							
							// Check every element of collection
							if (!(data is string) && data is IEnumerable)
							{
								foreach (object o in (IEnumerable) data)
								{
									isIgnored = IsValueIgnored(o, path, depth - 1);
									if (!isIgnored)
									{
										break;
									}
								}
							}
							else
							{
								// Check read value
								val = data;
								isIgnored = IsValueIgnored(val, path, depth - 1);
							}
						}
						else
						{
							// Recursively check all properties of collection
							if (!(val is string) && val is IEnumerable)
							{
								foreach (object o in (IEnumerable)val)
								{
									isIgnored = IsValueIgnored(o, path, depth - 1);

									// Property is not ignored - break the loop
									if (!isIgnored)
									{
										break;
									}
								}
							}

							// Check all object's properties/fields
							else
							{
								// Check all properties
								foreach (PropertyInfo property in valType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
								{
									if (!property.CanRead) continue;
									// Ignore MemoryStream properties because they throw exceptions
//									if (m_ignoredProperties.Contains(property)) continue;
									if (property.DeclaringType.Equals(typeof(MemoryStream))) continue;
									if (property.DeclaringType.Equals(typeof(Stream))) continue;
									if (property.DeclaringType.Equals(typeof(Type))) continue;
									if (property.DeclaringType.Equals(typeof(Object))) continue;
									if (property.GetIndexParameters().GetLength(0) != 0) continue;

									object objPropVal = property.GetValue(val, null);
									isIgnored = IsValueIgnored(objPropVal, path, depth - 1);

									// Property is not ignored - break the loop
									if (!isIgnored)
									{
										break;
									}
								}

								// Check all fields
								if (isIgnored)
								{
									foreach (FieldInfo field in valType.GetFields(BindingFlags.Instance | BindingFlags.Public))
									{
										if (!field.IsPublic) continue;

										object objPropVal = field.GetValue(val);
										isIgnored = IsValueIgnored(objPropVal, path, depth - 1);

										// Field is not ignored - break the loop
										if (!isIgnored)
										{
											break;
										}
									}
								}
							}
						}

						// Compare strings
						if (isIgnored && (path == null || depth == 0))
						{
							if (val is string && value != "" && ((string)val).ToLower().IndexOf(value) != -1)
							{
								// Filter string is a sub-string of packet property
								if (relation == "==")
									isIgnored = false;
								else if (relation == "!=")
									isIgnored = false;
							}
							else
							{
								// Packet property string equals filter value
								bool rc = val.ToString().ToLower().Equals(value);
								if (rc && relation == "==")
									isIgnored = false;
								else if (!rc && relation == "!=")
									isIgnored = false;
							}
						}
					}
				}
				
				return isIgnored;
			}
		}
		#endregion

		/// <summary>
		/// Determines whether class is ignored.
		/// </summary>
		/// <param name="classType">Type of the class.</param>
		/// <returns>
		/// 	<c>true</c> if class is ignored; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsClassIgnored(Type classType)
		{
			return classType == typeof(TimeSpan)
				|| classType == typeof(LogPacketAttribute)
//				|| valType == typeof(String)
				|| classType == typeof(ePacketProtocol)
				|| classType == typeof(ePacketDirection)
				|| typeof(Exception).IsAssignableFrom(classType);
		}
	}
}
