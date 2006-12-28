using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// Filter by packet property value
	/// </summary>
	[LogFilter("Packet property value...", Priority = 300)]
	public class PacketPropertyValueFilterForm : System.Windows.Forms.Form, ILogFilter
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
		private CheckBox recursiveCheckBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PacketPropertyValueFilterForm()
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
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 23);
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
			this.packetClassComboBox.MaxDropDownItems = 20;
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
			this.classPropertyComboBox.Location = new System.Drawing.Point(118, 31);
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
			this.propertyValueTextBox.Size = new System.Drawing.Size(344, 20);
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
			this.filtersListBox.Location = new System.Drawing.Point(6, 76);
			this.filtersListBox.Name = "filtersListBox";
			this.filtersListBox.Size = new System.Drawing.Size(536, 154);
			this.filtersListBox.TabIndex = 6;
			this.filtersListBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.filtersListBox_MouseClick);
			this.filtersListBox.SelectedIndexChanged += new System.EventHandler(this.filtersListBox_SelectedIndexChanged);
			// 
			// addButton
			// 
			this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.addButton.Location = new System.Drawing.Point(86, 236);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(75, 23);
			this.addButton.TabIndex = 7;
			this.addButton.Text = "&Add";
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// removeButton
			// 
			this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.removeButton.Location = new System.Drawing.Point(161, 236);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(75, 23);
			this.removeButton.TabIndex = 8;
			this.removeButton.Text = "&Remove";
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// clearButton
			// 
			this.clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.clearButton.Location = new System.Drawing.Point(311, 236);
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
			this.acceptButton.Location = new System.Drawing.Point(398, 236);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.Size = new System.Drawing.Size(75, 23);
			this.acceptButton.TabIndex = 10;
			this.acceptButton.Text = "&Accept";
			// 
			// enableCheckBox
			// 
			this.enableCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.enableCheckBox.Location = new System.Drawing.Point(478, 236);
			this.enableCheckBox.Name = "enableCheckBox";
			this.enableCheckBox.Size = new System.Drawing.Size(64, 24);
			this.enableCheckBox.TabIndex = 11;
			this.enableCheckBox.Text = "&Enable";
			// 
			// propertyValueTypeTextBox
			// 
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
			this.modifyButton.Location = new System.Drawing.Point(236, 236);
			this.modifyButton.Name = "modifyButton";
			this.modifyButton.Size = new System.Drawing.Size(75, 23);
			this.modifyButton.TabIndex = 13;
			this.modifyButton.Text = "&Modify";
			this.modifyButton.Click += new System.EventHandler(this.modifyButton_Click);
			// 
			// recursiveCheckBox
			// 
			this.recursiveCheckBox.AutoSize = true;
			this.recursiveCheckBox.Location = new System.Drawing.Point(6, 240);
			this.recursiveCheckBox.Name = "recursiveCheckBox";
			this.recursiveCheckBox.Size = new System.Drawing.Size(74, 17);
			this.recursiveCheckBox.TabIndex = 14;
			this.recursiveCheckBox.Text = "Re&cursive";
			this.recursiveCheckBox.UseVisualStyleBackColor = true;
			// 
			// PacketPropertyValueFilterForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(554, 271);
			this.ControlBox = false;
			this.Controls.Add(this.recursiveCheckBox);
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
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
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
			foreach (FilterListEntry entry in filtersListBox.Items)
			{
				if (!entry.IsPacketIgnored(packet))
					return false;
			}
			return true;
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

		private void UpdateClassPropertiesList()
		{
			classPropertyComboBox.Items.Clear();
			ClassProperty anyPropertyItem = new ClassProperty(null);
			classPropertyComboBox.Items.Add(anyPropertyItem);

			BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
			PacketClass selectedClass = (PacketClass)packetClassComboBox.SelectedItem;
			Type classType = selectedClass.type;
			if (classType == null)
			{
				flags = flags & ~BindingFlags.DeclaredOnly;
				classType = typeof(Packet);
			}

			foreach (PropertyInfo property in classType.GetProperties(flags))
			{
				if (!property.CanRead) continue;
				if (property.GetIndexParameters().GetLength(0) != 0) continue;
				classPropertyComboBox.Items.Add(new ClassProperty(property));
			}

			classPropertyComboBox.SelectedItem = anyPropertyItem;
		}

		#region buttons

		private void addButton_Click(object sender, System.EventArgs e)
		{
			string value = propertyValueTextBox.Text;
			long integer;
			bool recursive = recursiveCheckBox.Checked;
			if (Util.ParseLong(value, out integer))
				value = integer.ToString();

			filtersListBox.Items.Add(new FilterListEntry((PacketClass)packetClassComboBox.SelectedItem, (ClassProperty)classPropertyComboBox.SelectedItem, value, recursive));
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
			UpdateButtons();
		}

		private void modifyButton_Click(object sender, System.EventArgs e)
		{
			int index = filtersListBox.SelectedIndex;
			if (index < 0 || index >= filtersListBox.Items.Count)
				return;
			bool recursive = recursiveCheckBox.Checked;
			filtersListBox.Items.Insert(index, new FilterListEntry((PacketClass)packetClassComboBox.SelectedItem, (ClassProperty)classPropertyComboBox.SelectedItem, propertyValueTextBox.Text, recursive));
			filtersListBox.Items.RemoveAt(index+1);
			filtersListBox.SelectedIndex = index;
		}

		private void UpdateButtons()
		{
			modifyButton.Enabled = removeButton.Enabled = filtersListBox.SelectedIndex >= 0 && filtersListBox.SelectedIndex < filtersListBox.Items.Count;
			clearButton.Enabled = filtersListBox.Items.Count > 0;
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
			}
			UpdateButtons();
		}

		#endregion

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

		private class ClassProperty
		{
			public readonly PropertyInfo property;

			public ClassProperty(PropertyInfo property)
			{
				this.property = property;
			}

			public override string ToString()
			{
				if (property == null)
					return "(any property)";
				return property.Name;
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
				// needed for proper list updates
				ClassProperty prop = obj as ClassProperty;
				if (prop == null || prop.property != property)
					return false;
				return true;
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
				if (property == null)
					return base.GetHashCode();
				return property.GetHashCode();
			}
		}

		private class FilterListEntry
		{
			public readonly PacketClass packetClass;
			public readonly ClassProperty classProperty;
			public readonly string value;
			public readonly bool isRecursive;
			public static readonly ArrayList m_ignoredProperties = new ArrayList();

			static FilterListEntry()
			{
				Type ignoredType = typeof(MemoryStream);
				foreach (PropertyInfo property in ignoredType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
				{
					m_ignoredProperties.Add(property);
				}
			}

			public FilterListEntry(PacketClass packetClass, ClassProperty classProperty, string value, bool isRecursive)
			{
				this.packetClass = packetClass;
				this.classProperty = classProperty;
				this.value = value.ToLower();
				this.isRecursive = isRecursive;
			}

			public override string ToString()
			{
				string rec = (isRecursive ? " (rec)" : "");
				return string.Format("{0} [{1}] == \"{2}\"{3}", packetClass, classProperty, value, rec);
			}

			public bool IsPacketIgnored(Packet packet)
			{
				bool isIgnored = true;
				
				// Filter by packet type
				if (packetClass.type != null && packetClass.type.IsAssignableFrom(packet.GetType()))
				{
					isIgnored = false;
				}
				else if (classProperty.property != null)
				{
					// Filter by selected property
					object val = classProperty.property.GetValue(packet, null);
					isIgnored = IsValueIgnored(val, isRecursive, 2);
				}
				else
				{
					isIgnored = IsValueIgnored(packet, isRecursive, 3);
				}
				
				return isIgnored;
			}

			private bool IsValueIgnored(object val, bool recursive, int depth)
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
					if (valType == typeof(TimeSpan)
					|| valType == typeof(LogPacketAttribute)
					|| valType == typeof(String)
					|| valType == typeof(ePacketProtocol)
					|| valType == typeof(ePacketDirection)
					|| typeof(Exception).IsAssignableFrom(valType))
					{
						isIgnored = true;
					}
					else if (valType.IsPublic || valType.IsNestedPublic)
					{
						if (recursive)
						{
							// Recursively check all properties of collection
							if (!(val is string) && val is IEnumerable)
							{
								foreach (object o in (IEnumerable)val)
								{
									isIgnored = IsValueIgnored(o, recursive, depth - 1);

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
									isIgnored = IsValueIgnored(objPropVal, recursive, depth - 1);

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
										isIgnored = IsValueIgnored(objPropVal, recursive, depth - 1);

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
						if (isIgnored)
						{
							if (val is string && value != "" && ((string)val).ToLower().IndexOf(value) != -1)
							{
								// Filter string is a substring of packet property
								isIgnored = false;
							}
							else if (val.ToString().ToLower().Equals(value))
							{
								// Packet property string equals filter value
								isIgnored = false;
							}
						}
					}
				}
				
				return isIgnored;
			}
		}
		#endregion
	}
}
