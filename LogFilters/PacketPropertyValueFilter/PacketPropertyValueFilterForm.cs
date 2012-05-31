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
using PacketLogConverter.LogFilters.PacketPropertyValueFilter;
using PacketLogConverter.LogFilters.PacketPropertyValueFilter.ValueCheckers;
using PacketLogConverter.LogWriters;
using PacketLogConverter.Utils.Serialization;
using PacketLogConverter.Utils.UI;

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
		private System.Windows.Forms.ComboBox packetClassPropertyComboBox;
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
		private GroupBox packetsGroupBox;
		private GroupBox variablesGroupBox;
		private ListBox varListBox;
		private Button varAddButton;
		private Button varRemoveButton;
		private Button varModifyButton;
		private Button varClearButton;
		private Label varNameLabel;
		private TextBox varNameTextBox;
		private Label varPacketsLinksLabel;
		private ListBox varPacketsLinksListBox;
		private Button varPacketsFieldsClearButton;
		private Button varPacketsFieldsModifyButton;
		private Button varPacketsFieldsRemoveButton;
		private Button varPacketsFieldsAddButton;

		private Filter filter;
		private IExecutionContext context;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PacketPropertyValueFilterForm()
			: this(null)
		{
		}

		public PacketPropertyValueFilterForm(IExecutionContext context)
		{
			this.context = context;

			InitializeComponent();
			relationsList.SelectedIndex = 0;
			Condition.SelectedIndex = 0;

			packetsGroupBox.AllowDrop = true;

			// Variables
			variablesManager.VariableAdded			+= variablesManager_VariableAdded;
			variablesManager.VariableModified		+= variablesManager_VariableModified;
			variablesManager.VariableRemoved		+= variablesManager_VariableRemoved;
			variablesManager.VariableLinkAdded		+= variablesManager_VariableLinkAdded;
			variablesManager.VariableLinkRemoved	+= variablesManager_VariableLinkRemoved;
			variablesManager.VariableLinkReplaced	+= variablesManager_VariableLinkReplaced;
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
			this.packetClassPropertyComboBox = new System.Windows.Forms.ComboBox();
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
			this.packetsGroupBox = new System.Windows.Forms.GroupBox();
			this.variablesGroupBox = new System.Windows.Forms.GroupBox();
			this.varPacketsFieldsClearButton = new System.Windows.Forms.Button();
			this.varPacketsFieldsModifyButton = new System.Windows.Forms.Button();
			this.varPacketsFieldsRemoveButton = new System.Windows.Forms.Button();
			this.varPacketsFieldsAddButton = new System.Windows.Forms.Button();
			this.varPacketsLinksListBox = new System.Windows.Forms.ListBox();
			this.varPacketsLinksLabel = new System.Windows.Forms.Label();
			this.varNameLabel = new System.Windows.Forms.Label();
			this.varNameTextBox = new System.Windows.Forms.TextBox();
			this.varAddButton = new System.Windows.Forms.Button();
			this.varRemoveButton = new System.Windows.Forms.Button();
			this.varModifyButton = new System.Windows.Forms.Button();
			this.varClearButton = new System.Windows.Forms.Button();
			this.varListBox = new System.Windows.Forms.ListBox();
			this.packetsGroupBox.SuspendLayout();
			this.variablesGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(35, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(73, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Packet class:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(10, 34);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 23);
			this.label2.TabIndex = 1;
			this.label2.Text = "Class property:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(4, 18);
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
			this.packetClassComboBox.Location = new System.Drawing.Point(114, 12);
			this.packetClassComboBox.MaxDropDownItems = 32;
			this.packetClassComboBox.Name = "packetClassComboBox";
			this.packetClassComboBox.Size = new System.Drawing.Size(518, 22);
			this.packetClassComboBox.Sorted = true;
			this.packetClassComboBox.TabIndex = 3;
			this.packetClassComboBox.SelectedValueChanged += new System.EventHandler(this.packetClassComboBox_SelectedValueChanged);
			// 
			// packetClassPropertyComboBox
			// 
			this.packetClassPropertyComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.packetClassPropertyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.packetClassPropertyComboBox.Font = new System.Drawing.Font("Courier New", 8F);
			this.packetClassPropertyComboBox.ItemHeight = 14;
			this.packetClassPropertyComboBox.Location = new System.Drawing.Point(114, 35);
			this.packetClassPropertyComboBox.MaxDropDownItems = 32;
			this.packetClassPropertyComboBox.Name = "packetClassPropertyComboBox";
			this.packetClassPropertyComboBox.Size = new System.Drawing.Size(518, 22);
			this.packetClassPropertyComboBox.TabIndex = 4;
			this.packetClassPropertyComboBox.SelectedIndexChanged += new System.EventHandler(this.packetClassPropertyComboBox_SelectedIndexChanged);
			// 
			// propertyValueTextBox
			// 
			this.propertyValueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.propertyValueTextBox.Location = new System.Drawing.Point(108, 19);
			this.propertyValueTextBox.MaxLength = 256;
			this.propertyValueTextBox.Name = "propertyValueTextBox";
			this.propertyValueTextBox.Size = new System.Drawing.Size(280, 20);
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
			this.filtersListBox.Location = new System.Drawing.Point(6, 43);
			this.filtersListBox.Name = "filtersListBox";
			this.filtersListBox.Size = new System.Drawing.Size(608, 94);
			this.filtersListBox.TabIndex = 6;
			this.filtersListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.filtersListBox_MouseDoubleClick);
			this.filtersListBox.SelectedIndexChanged += new System.EventHandler(this.filtersListBox_SelectedIndexChanged);
			// 
			// addButton
			// 
			this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.addButton.Location = new System.Drawing.Point(6, 144);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(75, 23);
			this.addButton.TabIndex = 7;
			this.addButton.Text = "&Add";
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// removeButton
			// 
			this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.removeButton.Location = new System.Drawing.Point(81, 144);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(75, 23);
			this.removeButton.TabIndex = 8;
			this.removeButton.Text = "&Remove";
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// clearButton
			// 
			this.clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.clearButton.Location = new System.Drawing.Point(231, 144);
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
			this.acceptButton.Location = new System.Drawing.Point(557, 456);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.Size = new System.Drawing.Size(75, 23);
			this.acceptButton.TabIndex = 10;
			this.acceptButton.Text = "&Accept";
			// 
			// enableCheckBox
			// 
			this.enableCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.enableCheckBox.Location = new System.Drawing.Point(492, 460);
			this.enableCheckBox.Name = "enableCheckBox";
			this.enableCheckBox.Size = new System.Drawing.Size(59, 17);
			this.enableCheckBox.TabIndex = 11;
			this.enableCheckBox.Text = "&Enable";
			// 
			// propertyValueTypeTextBox
			// 
			this.propertyValueTypeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.propertyValueTypeTextBox.Location = new System.Drawing.Point(440, 19);
			this.propertyValueTypeTextBox.MaxLength = 16;
			this.propertyValueTypeTextBox.Name = "propertyValueTypeTextBox";
			this.propertyValueTypeTextBox.ReadOnly = true;
			this.propertyValueTypeTextBox.Size = new System.Drawing.Size(174, 20);
			this.propertyValueTypeTextBox.TabIndex = 12;
			// 
			// modifyButton
			// 
			this.modifyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.modifyButton.Location = new System.Drawing.Point(156, 144);
			this.modifyButton.Name = "modifyButton";
			this.modifyButton.Size = new System.Drawing.Size(75, 23);
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
			this.relationsList.Location = new System.Drawing.Point(394, 19);
			this.relationsList.Name = "relationsList";
			this.relationsList.Size = new System.Drawing.Size(40, 21);
			this.relationsList.TabIndex = 15;
			// 
			// Condition
			// 
			this.Condition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.Condition.Items.AddRange(new object[] {
            "OR",
            "AND",
            "OR("});
			this.Condition.Location = new System.Drawing.Point(12, 12);
			this.Condition.Name = "Condition";
			this.Condition.Size = new System.Drawing.Size(48, 21);
			this.Condition.TabIndex = 16;
			this.Condition.Visible = false;
			// 
			// filterScrollBar
			// 
			this.filterScrollBar.LargeChange = 1;
			this.filterScrollBar.Location = new System.Drawing.Point(6, 16);
			this.filterScrollBar.Maximum = 1;
			this.filterScrollBar.Minimum = -1;
			this.filterScrollBar.Name = "filterScrollBar";
			this.filterScrollBar.Size = new System.Drawing.Size(17, 23);
			this.filterScrollBar.TabIndex = 17;
			this.filterScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.filterScrollBar_Scroll);
			// 
			// packetsGroupBox
			// 
			this.packetsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.packetsGroupBox.Controls.Add(this.addButton);
			this.packetsGroupBox.Controls.Add(this.filterScrollBar);
			this.packetsGroupBox.Controls.Add(this.label3);
			this.packetsGroupBox.Controls.Add(this.relationsList);
			this.packetsGroupBox.Controls.Add(this.modifyButton);
			this.packetsGroupBox.Controls.Add(this.propertyValueTypeTextBox);
			this.packetsGroupBox.Controls.Add(this.propertyValueTextBox);
			this.packetsGroupBox.Controls.Add(this.filtersListBox);
			this.packetsGroupBox.Controls.Add(this.removeButton);
			this.packetsGroupBox.Controls.Add(this.clearButton);
			this.packetsGroupBox.Location = new System.Drawing.Point(12, 63);
			this.packetsGroupBox.Name = "packetsGroupBox";
			this.packetsGroupBox.Size = new System.Drawing.Size(620, 173);
			this.packetsGroupBox.TabIndex = 18;
			this.packetsGroupBox.TabStop = false;
			this.packetsGroupBox.Text = "Packets";
			this.packetsGroupBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.packetsGroupBox_DragDrop);
			this.packetsGroupBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.packetsGroupBox_DragEnter);
			// 
			// variablesGroupBox
			// 
			this.variablesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.variablesGroupBox.Controls.Add(this.varPacketsFieldsClearButton);
			this.variablesGroupBox.Controls.Add(this.varPacketsFieldsModifyButton);
			this.variablesGroupBox.Controls.Add(this.varPacketsFieldsRemoveButton);
			this.variablesGroupBox.Controls.Add(this.varPacketsFieldsAddButton);
			this.variablesGroupBox.Controls.Add(this.varPacketsLinksListBox);
			this.variablesGroupBox.Controls.Add(this.varPacketsLinksLabel);
			this.variablesGroupBox.Controls.Add(this.varNameLabel);
			this.variablesGroupBox.Controls.Add(this.varNameTextBox);
			this.variablesGroupBox.Controls.Add(this.varAddButton);
			this.variablesGroupBox.Controls.Add(this.varRemoveButton);
			this.variablesGroupBox.Controls.Add(this.varModifyButton);
			this.variablesGroupBox.Controls.Add(this.varClearButton);
			this.variablesGroupBox.Controls.Add(this.varListBox);
			this.variablesGroupBox.Location = new System.Drawing.Point(12, 242);
			this.variablesGroupBox.Name = "variablesGroupBox";
			this.variablesGroupBox.Size = new System.Drawing.Size(620, 208);
			this.variablesGroupBox.TabIndex = 0;
			this.variablesGroupBox.TabStop = false;
			this.variablesGroupBox.Text = "Variables (drag&&drop to \"packets\" group to use as packet value)";
			// 
			// varPacketsFieldsClearButton
			// 
			this.varPacketsFieldsClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.varPacketsFieldsClearButton.Location = new System.Drawing.Point(538, 177);
			this.varPacketsFieldsClearButton.Name = "varPacketsFieldsClearButton";
			this.varPacketsFieldsClearButton.Size = new System.Drawing.Size(75, 23);
			this.varPacketsFieldsClearButton.TabIndex = 32;
			this.varPacketsFieldsClearButton.Text = "Clear";
			this.varPacketsFieldsClearButton.UseVisualStyleBackColor = true;
			this.varPacketsFieldsClearButton.Click += new System.EventHandler(this.varPacketsFieldsClearButton_Click);
			// 
			// varPacketsFieldsModifyButton
			// 
			this.varPacketsFieldsModifyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.varPacketsFieldsModifyButton.Location = new System.Drawing.Point(463, 177);
			this.varPacketsFieldsModifyButton.Name = "varPacketsFieldsModifyButton";
			this.varPacketsFieldsModifyButton.Size = new System.Drawing.Size(75, 23);
			this.varPacketsFieldsModifyButton.TabIndex = 31;
			this.varPacketsFieldsModifyButton.Text = "Modify";
			this.varPacketsFieldsModifyButton.UseVisualStyleBackColor = true;
			this.varPacketsFieldsModifyButton.Click += new System.EventHandler(this.varPacketsFieldsModifyButton_Click);
			// 
			// varPacketsFieldsRemoveButton
			// 
			this.varPacketsFieldsRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.varPacketsFieldsRemoveButton.Location = new System.Drawing.Point(388, 177);
			this.varPacketsFieldsRemoveButton.Name = "varPacketsFieldsRemoveButton";
			this.varPacketsFieldsRemoveButton.Size = new System.Drawing.Size(75, 23);
			this.varPacketsFieldsRemoveButton.TabIndex = 30;
			this.varPacketsFieldsRemoveButton.Text = "Remove";
			this.varPacketsFieldsRemoveButton.UseVisualStyleBackColor = true;
			this.varPacketsFieldsRemoveButton.Click += new System.EventHandler(this.varPacketsFieldsRemoveButton_Click);
			// 
			// varPacketsFieldsAddButton
			// 
			this.varPacketsFieldsAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.varPacketsFieldsAddButton.Location = new System.Drawing.Point(313, 177);
			this.varPacketsFieldsAddButton.Name = "varPacketsFieldsAddButton";
			this.varPacketsFieldsAddButton.Size = new System.Drawing.Size(75, 23);
			this.varPacketsFieldsAddButton.TabIndex = 29;
			this.varPacketsFieldsAddButton.Text = "Add";
			this.varPacketsFieldsAddButton.UseVisualStyleBackColor = true;
			this.varPacketsFieldsAddButton.Click += new System.EventHandler(this.varPacketsFieldsAddButton_Click);
			// 
			// varPacketsLinksListBox
			// 
			this.varPacketsLinksListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.varPacketsLinksListBox.FormattingEnabled = true;
			this.varPacketsLinksListBox.Location = new System.Drawing.Point(313, 45);
			this.varPacketsLinksListBox.Name = "varPacketsLinksListBox";
			this.varPacketsLinksListBox.Size = new System.Drawing.Size(301, 121);
			this.varPacketsLinksListBox.TabIndex = 27;
			this.varPacketsLinksListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.varPacketsLinksListBox_MouseDoubleClick);
			this.varPacketsLinksListBox.SelectedIndexChanged += new System.EventHandler(this.varPacketsLinksListBox_SelectedIndexChanged);
			// 
			// varPacketsLinksLabel
			// 
			this.varPacketsLinksLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.varPacketsLinksLabel.AutoSize = true;
			this.varPacketsLinksLabel.Location = new System.Drawing.Point(312, 22);
			this.varPacketsLinksLabel.Name = "varPacketsLinksLabel";
			this.varPacketsLinksLabel.Size = new System.Drawing.Size(200, 13);
			this.varPacketsLinksLabel.TabIndex = 26;
			this.varPacketsLinksLabel.Text = "Links of selected variable to packet field:";
			// 
			// varNameLabel
			// 
			this.varNameLabel.AutoSize = true;
			this.varNameLabel.Location = new System.Drawing.Point(6, 22);
			this.varNameLabel.Name = "varNameLabel";
			this.varNameLabel.Size = new System.Drawing.Size(38, 13);
			this.varNameLabel.TabIndex = 25;
			this.varNameLabel.Text = "Name:";
			// 
			// varNameTextBox
			// 
			this.varNameTextBox.Location = new System.Drawing.Point(50, 19);
			this.varNameTextBox.Name = "varNameTextBox";
			this.varNameTextBox.Size = new System.Drawing.Size(257, 20);
			this.varNameTextBox.TabIndex = 19;
			// 
			// varAddButton
			// 
			this.varAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.varAddButton.Location = new System.Drawing.Point(6, 177);
			this.varAddButton.Name = "varAddButton";
			this.varAddButton.Size = new System.Drawing.Size(75, 23);
			this.varAddButton.TabIndex = 21;
			this.varAddButton.Text = "Add";
			this.varAddButton.UseVisualStyleBackColor = true;
			this.varAddButton.Click += new System.EventHandler(this.varAddButton_Click);
			// 
			// varRemoveButton
			// 
			this.varRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.varRemoveButton.Location = new System.Drawing.Point(81, 177);
			this.varRemoveButton.Name = "varRemoveButton";
			this.varRemoveButton.Size = new System.Drawing.Size(75, 23);
			this.varRemoveButton.TabIndex = 22;
			this.varRemoveButton.Text = "Remove";
			this.varRemoveButton.UseVisualStyleBackColor = true;
			this.varRemoveButton.Click += new System.EventHandler(this.varRemoveButton_Click);
			// 
			// varModifyButton
			// 
			this.varModifyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.varModifyButton.Location = new System.Drawing.Point(156, 177);
			this.varModifyButton.Name = "varModifyButton";
			this.varModifyButton.Size = new System.Drawing.Size(75, 23);
			this.varModifyButton.TabIndex = 23;
			this.varModifyButton.Text = "Modify";
			this.varModifyButton.UseVisualStyleBackColor = true;
			this.varModifyButton.Click += new System.EventHandler(this.varModifyButton_Click);
			// 
			// varClearButton
			// 
			this.varClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.varClearButton.Location = new System.Drawing.Point(231, 177);
			this.varClearButton.Name = "varClearButton";
			this.varClearButton.Size = new System.Drawing.Size(75, 23);
			this.varClearButton.TabIndex = 24;
			this.varClearButton.Text = "Clear";
			this.varClearButton.UseVisualStyleBackColor = true;
			this.varClearButton.Click += new System.EventHandler(this.varClearButton_Click);
			// 
			// varListBox
			// 
			this.varListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.varListBox.FormattingEnabled = true;
			this.varListBox.Location = new System.Drawing.Point(6, 45);
			this.varListBox.Name = "varListBox";
			this.varListBox.Size = new System.Drawing.Size(301, 121);
			this.varListBox.TabIndex = 20;
			this.varListBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.varListBox_MouseClick);
			this.varListBox.SelectedIndexChanged += new System.EventHandler(this.varListBox_SelectedIndexChanged);
			this.varListBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.varListBox_MouseMove);
			this.varListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.varListBox_MouseDown);
			this.varListBox.MouseLeave += new System.EventHandler(this.varListBox_MouseLeave);
			// 
			// PacketPropertyValueFilterForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(644, 505);
			this.ControlBox = false;
			this.Controls.Add(this.variablesGroupBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.packetsGroupBox);
			this.Controls.Add(this.Condition);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.packetClassPropertyComboBox);
			this.Controls.Add(this.packetClassComboBox);
			this.Controls.Add(this.enableCheckBox);
			this.Controls.Add(this.acceptButton);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(652, 513);
			this.Name = "PacketPropertyValueFilterForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Packet property value filter";
			this.packetsGroupBox.ResumeLayout(false);
			this.packetsGroupBox.PerformLayout();
			this.variablesGroupBox.ResumeLayout(false);
			this.variablesGroupBox.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		#region ILogFilter Members

		#region Filtering logic

		/// <summary>
		/// Activates the filter.
		/// </summary>
		/// <returns>
		/// 	<code>true</code> if filter has changed and log should be updated.
		/// </returns>
		public bool ActivateFilter()
		{
#warning TODO: Instance of filter class should be activated and show this dialog, remove this hack

			InitPacketsList();

			bool oldEnable = enableCheckBox.Checked;
			ArrayList oldFilterList = new ArrayList(filtersListBox.Items);

			ShowDialog();

			bool ret = ActivateFilterImpl(oldFilterList, oldEnable);

			return ret;
		}

		/// <summary>
		/// Activates the filter.
		/// </summary>
		/// <param name="oldFilterList">The old filter list.</param>
		/// <param name="oldEnable">if set to <c>true</c> [old enable].</param>
		/// <returns><code>true</code> if filter has changed and log should be updated.</returns>
		private bool ActivateFilterImpl(ArrayList oldFilterList, bool oldEnable)
		{
			if (IsFilterActive)
			{
				CreateFilter();

				context.FilterManager.AddFilter(this);
			}
			else
			{
				context.FilterManager.RemoveFilter(this);
				ReleaseFilter();
				return false;
			}

			// update the log data if changes to filters
			if (null != oldFilterList)
			{
				for (int i = 0; i < filtersListBox.Items.Count; i++)
				{
					object item = (object) filtersListBox.Items[i];
					if (!oldFilterList.Contains(item))
						return true;
				}
			}

			return enableCheckBox.Checked != oldEnable || (null == oldFilterList || filtersListBox.Items.Count != oldFilterList.Count);
		}

		private void CreateFilter()
		{
			ReleaseFilter();

			// Initialize filter instance
			ValueCheckerBuilder builder = new ValueCheckerBuilder(variablesManager);
			List<IValueChecker<object>> checkers = builder.CreateCheckersFromFilterListEntries(filtersListBox.Items);
			filter = new Filter(context, variablesManager, checkers);
			// Have to pass "context" to it for variables manager to function properly
			filter.ActivateFilter();
		}

		/// <summary>
		/// Releases the filter.
		/// </summary>
		private void ReleaseFilter()
		{
			if (null != filter)
			{
				// Remove all events
				filter.IsFilterActive = false;

				// Release all resources
				filter.Dispose();
				filter = null;
			}
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
			bool ret = filter.IsPacketIgnored(packet);
			return ret;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is active.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is active; otherwise, <c>false</c>.
		/// </value>
		public bool IsFilterActive
		{
			get { return enableCheckBox.Checked; }
			set
			{
				// Change active state - modify all event handlers
				if (value != enableCheckBox.Checked)
				{
					enableCheckBox.Checked = value;
					ActivateFilterImpl(null, !value);
				}
			}
		}

		#endregion

		#region Serialization

		/// <summary>
		/// Serializes data of instance of this filter.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns><code>true</code> if filter is serialized, <code>false</code> otherwise.</returns>
		public bool Serialize(MemoryStream data)
		{
			// Do not save empty filters
			bool ret = (filtersListBox.Items.Count > 0 || varListBox.Items.Count > 0);

			if (ret)
			{
				// Save all entries
				using (BinaryWriter binWriter = new BinaryWriter(data))
				{
					// Enabled flag
					binWriter.Write(enableCheckBox.Checked);

					// Serialize list boxes
					SerializationHelper serializer = new SerializationHelper(data, binWriter);
					serializer.SerializeCollection(filtersListBox.Items);
					serializer.SerializeCollection(varListBox.Items);
				}
			}

			return ret;
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
			varListBox.Items.Clear();
			varPacketsLinksListBox.Items.Clear();

			bool oldActive = enableCheckBox.Checked;
			bool newActive = oldActive;

			// Load all entries
			using (BinaryReader binReader = new BinaryReader(data))
			{
				// Enabled flag
				newActive = binReader.ReadBoolean();

				//
				// Deserialize list boxes
				//
				DeserializationHelper deserializer = new DeserializationHelper(binReader);
				IList filterEntries = new ArrayList(),
					variableEntries = new ArrayList();
				deserializer.DeserializeList(filterEntries);
				deserializer.DeserializeList(variableEntries);
				LoadLists(filterEntries, variableEntries);
			}
			UpdateButtonsEnabledState();

			// Activate new filter state
			IsFilterActive = newActive;

			return true;
		}

		/// <summary>
		/// Loads the lists.
		/// </summary>
		/// <param name="filterEntries">The filter entries.</param>
		/// <param name="variableEntries">The variable entries.</param>
		private void LoadLists(IList filterEntries, IList variableEntries)
		{
			// Remove old variables
			variablesManager.ClearVariables();

			// Add all new variables - will be added to all lists automatically using events
			foreach (PacketPropertyVariable variable in variableEntries)
			{
				variablesManager.AddVariable(variable);
			}

			// Add all new filter list entries
			foreach (FilterListEntry filterEntry in filterEntries)
			{
				// Nothing special about the value - add as-is
				FilterListEntry entry = filterEntry;
				if (filterEntry.searchValue is PacketPropertyVariable)
				{
					// Find real variable instance by name
 				}
				filtersListBox.Items.Add(entry);
			}
		}

		#endregion

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
			UpdateButtonsEnabledState();

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

		/// <summary>
		/// Handles the SelectedValueChanged event of the packetClassComboBox control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void packetClassComboBox_SelectedValueChanged(object sender, EventArgs e)
		{
			UpdateClassPropertiesList();
		}

		/// <summary>
		/// Handles the SelectedIndexChanged event of the packetClassPropertyComboBox control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void packetClassPropertyComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Variables "Add" button depends on value in this control
			UpdateButtonsEnabledState();
		}

		/// <summary>
		/// Updates the class properties list.
		/// </summary>
		private void UpdateClassPropertiesList()
		{
			// Clear current list and add "any property" entries
			packetClassPropertyComboBox.Items.Clear();
			packetClassPropertyComboBox.Items.Add(new ClassMemberPath(null, false));
			packetClassPropertyComboBox.Items.Add(new ClassMemberPath(null, true));

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
				packetClassPropertyComboBox.Items.Add(new ClassMemberPath(members, true));
			}

			packetClassPropertyComboBox.SelectedIndex = 0;
		}

		#region Buttons

		/// <summary>
		/// Handles the Click event of the addButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void addButton_Click(object sender, System.EventArgs e)
		{
			ClassMemberPath selectedPath = (ClassMemberPath) packetClassPropertyComboBox.SelectedItem;

			filtersListBox.Items.Add(new FilterListEntry(
			                         	(PacketClass)packetClassComboBox.SelectedItem,
			                         	selectedPath,
			                         	currentPropertyValue,
										selectedPath.isRecursive,
			                         	relationsList.Text,
			                         	Condition.Text));
			UpdateButtonsEnabledState();
		}

		/// <summary>
		/// Handles the Click event of the removeButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void removeButton_Click(object sender, System.EventArgs e)
		{
			if (filtersListBox.SelectedIndex >= 0)
				filtersListBox.Items.RemoveAt(filtersListBox.SelectedIndex);
			UpdateButtonsEnabledState();
		}

		/// <summary>
		/// Handles the Click event of the clearButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void clearButton_Click(object sender, System.EventArgs e)
		{
			filtersListBox.Items.Clear();
			relationsList.SelectedIndex = 0;
			Condition.SelectedIndex = 0;
			UpdateButtonsEnabledState();
		}

		/// <summary>
		/// Handles the Click event of the modifyButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void modifyButton_Click(object sender, System.EventArgs e)
		{
			int index = filtersListBox.SelectedIndex;
			if (index < 0 || index >= filtersListBox.Items.Count)
				return;

			ClassMemberPath selectedPath = (ClassMemberPath)packetClassPropertyComboBox.SelectedItem;
			filtersListBox.Items.Insert(
				index,
				new FilterListEntry(
					(PacketClass)packetClassComboBox.SelectedItem,
					selectedPath,
					currentPropertyValue,
					selectedPath.isRecursive,
					relationsList.Text,
					Condition.Text));
			filtersListBox.Items.RemoveAt(index+1);
			filtersListBox.SelectedIndex = index;
		}

		/// <summary>
		/// Updates the buttons.
		/// </summary>
		private void UpdateButtonsEnabledState()
		{
			// Packet value filter
			modifyButton.Enabled						=
				removeButton.Enabled					= filtersListBox.SelectedIndex >= 0 && filtersListBox.SelectedIndex < filtersListBox.Items.Count;
			clearButton.Enabled							= filtersListBox.Items.Count > 0;
			Condition.Enabled							=
				Condition.Visible						= (filtersListBox.Items.Count > 1) && filtersListBox.SelectedIndex >= 1 && filtersListBox.SelectedIndex < filtersListBox.Items.Count;
			filterScrollBar.Visible						= (filtersListBox.Items.Count > 1);
			filterScrollBar.Enabled						= filterScrollBar.Visible && filtersListBox.SelectedIndex >= 0 && filtersListBox.SelectedIndex < filtersListBox.Items.Count;

			// Variables
			varAddButton.Enabled						= (null != packetClassPropertyComboBox.SelectedItem && null != ((ClassMemberPath)packetClassPropertyComboBox.SelectedItem).members);
			varModifyButton.Enabled						= 
				varRemoveButton.Enabled					= (null != varListBox.SelectedItem);
			varClearButton.Enabled						= varListBox.Items.Count > 0;

			// Variable links
			varPacketsFieldsAddButton.Enabled			= (null != packetClassPropertyComboBox.SelectedItem
															&& null != ((ClassMemberPath)packetClassPropertyComboBox.SelectedItem).members
															&& null != varListBox.SelectedItem
														);
			// There always should be at least single packet field, variable makes no sence otherwise and should be removed instead
			varPacketsFieldsRemoveButton.Enabled		= (null != varPacketsLinksListBox.SelectedItem && varPacketsLinksListBox.Items.Count > 1);
			varPacketsFieldsModifyButton.Enabled		= (varPacketsFieldsAddButton.Enabled && null != varPacketsLinksListBox.SelectedItem);
			varPacketsFieldsClearButton.Enabled			= varPacketsLinksListBox.Items.Count > 1;
		}

		#endregion

		/// <summary>
		/// Handles the TextChanged event of the propertyValueTextBox control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void propertyValueTextBox_TextChanged(object sender, System.EventArgs e)
		{
			SetCurrentPropertyValue(propertyValueTextBox.Text);
		}

		private object currentPropertyValue = string.Empty;

#warning TODO: Add checkbox to make to make it possible to activate list box items by single-click instead of double-click
#warning TODO: Check if variable is used on deletion of a variable
#warning TODO: Disable "add" buttons if variable link cannot be added/deleted/updated
#warning TODO: Default/initial value of a variable

		private bool isProcessingSetCurrentPropertyValue;

		private void SetCurrentPropertyValue(object newValue)
		{
			// Do re-entrance check because of event handlers on text boxes that we want to modify by this code
			if (isProcessingSetCurrentPropertyValue)
			{
				return;
			}

			isProcessingSetCurrentPropertyValue = true;
			try
			{
				// Safety check
				if (null == newValue)
				{
					newValue = string.Empty;
				}

				currentPropertyValue = newValue;
				string userFriendlyDescription = "(unknown)";

				// Check if it is a string
				string strValue = newValue as string;
				if (null != strValue)
				{
					long value;
					if (Util.ParseLong(strValue, out value))
					{
						currentPropertyValue = value;
						userFriendlyDescription = "0x" + value.ToString("X4");
					}
					else
					{
						currentPropertyValue = strValue.ToLower();
						userFriendlyDescription = "(string)";
					}
				}
				else
				{
					// Some object is stored
					currentPropertyValue = newValue;
					userFriendlyDescription = currentPropertyValue.ToString();
				}

#warning TODO: Show real long value to user on filter entry activation (string is shown right now)
				// Show description to the end-user
				propertyValueTypeTextBox.Text = userFriendlyDescription;
				propertyValueTextBox.Text = (newValue is PacketPropertyVariable || null == newValue ? string.Empty : newValue.ToString());
			}
			finally
			{
				isProcessingSetCurrentPropertyValue = false;
			}
		}

		#region Packet class/property combos

		/// <summary>
		/// Selects the packet and its property in comboboxes.
		/// </summary>
		/// <param name="packetClass">The packet class.</param>
		/// <param name="packetClassMember">The packet class member.</param>
		private void SelectPacketAndProperty(PacketClass packetClass, ClassMemberPath packetClassMember)
		{
			if (null != packetClass)
			{
				packetClassComboBox.SelectedItem = packetClass;
			}
			if (null != packetClassMember)
			{
				packetClassPropertyComboBox.SelectedItem = packetClassMember;
			}
		}

		#endregion

		#region filtersListBox

		/// <summary>
		/// Handles the SelectedIndexChanged event of the filtersListBox control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void filtersListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			filtersListBoxChanges();
//			UpdateButtonsEnabledState();
		}

		/// <summary>
		/// Handles the MouseDoubleClick event of the filtersListBox control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
		private void filtersListBox_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			filtersListBoxChanges();
		}

		/// <summary>
		/// Handles changes of "filters list box".
		/// </summary>
		private void filtersListBoxChanges()
		{
			FilterListEntry entry = filtersListBox.SelectedItem as FilterListEntry;
			if (entry != null)
			{
				SelectPacketAndProperty(entry.packetClass, entry.classProperty);
				SetCurrentPropertyValue(entry.searchValue);
				relationsList.Text = entry.relation;
				Condition.SelectedItem = entry.condition;
			}
			UpdateButtonsEnabledState();
		}

		#endregion

		#region Variables

		private readonly PacketPropertyVariablesManager variablesManager = new PacketPropertyVariablesManager();

		private int lastVariableIndex;

		/// <summary>
		/// Generates the name of a new variable.
		/// </summary>
		/// <returns></returns>
		private string GenerateNewVariableName()
		{
#warning TODO: Find really unique name and consider input textbox
			string ret = "Name" + ++lastVariableIndex;
			return ret;
		}

		#region Updates

		/// <summary>
		/// Updates the selected variable data.
		/// </summary>
		private void UpdateSelectedVariableData()
		{
			// Get selected variable
			PacketPropertyVariable var = (PacketPropertyVariable)varListBox.SelectedItem;

			ShowVariableLinks(var);

			if (null == var)
			{
				// Clear variable name as there is nothing to show
				varNameTextBox.Clear();
			}
			else
			{
				// Show variable name
				varNameTextBox.Text = var.Name;
			}
		}

		/// <summary>
		/// Shows the links of a variable.
		/// </summary>
		/// <param name="variable">The variable.</param>
		private void ShowVariableLinks(PacketPropertyVariable variable)
		{
			varPacketsLinksListBox.Items.Clear();
			if (null != variable)
			{
				ListBoxHelpers.AddRange(varPacketsLinksListBox, variable.Links);
			}
		}

		#endregion

		#region Variable manager events

		//
		// Synchronize user-visible data with modifications of variables manager
		//

		/// <summary>
		/// Adds the variable.
		/// </summary>
		/// <param name="variable">The variable.</param>
		private void variablesManager_VariableAdded(PacketPropertyVariable variable)
		{
			// Add and select new variable
			varListBox.Items.Add(variable);
			varListBox.SelectedItem = variable;

			// Updates
			UpdateSelectedVariableData();
			UpdateButtonsEnabledState();

			// Select variable name
			varNameTextBox.Focus();
			varNameTextBox.SelectAll();
		}

		/// <summary>
		/// Modifies the variable.
		/// </summary>
		/// <param name="variable">The variable.</param>
		private void variablesManager_VariableModified(PacketPropertyVariable variable)
		{
			// Update modified item
			int index = varListBox.Items.IndexOf(variable);
			varListBox.Items[index] = variable;

			// Update packet filters list
			ListBoxHelpers.Refresh(filtersListBox);

			UpdateButtonsEnabledState();
		}

		/// <summary>
		/// Removes the variable.
		/// </summary>
		/// <param name="variable">The variable.</param>
		private void variablesManager_VariableRemoved(PacketPropertyVariable variable)
		{
			varListBox.Items.Remove(variable);
			UpdateButtonsEnabledState();
		}

		/// <summary>
		/// Variableses the manager_ variable link added.
		/// </summary>
		/// <param name="variable">The variable.</param>
		/// <param name="link">The link.</param>
		private void variablesManager_VariableLinkAdded(PacketPropertyVariable variable, PacketPropertyVariableLink link)
		{
			// Update links listbox only if variable is currently selected (and hence links list contains variables links)
			if (varListBox.SelectedItem == variable)
			{
				varPacketsLinksListBox.Items.Add(link);
			}
		}

		/// <summary>
		/// Variableses the manager_ variable link replaced.
		/// </summary>
		/// <param name="variable">The variable.</param>
		/// <param name="link">The link.</param>
		private void variablesManager_VariableLinkReplaced(PacketPropertyVariable variable, PacketPropertyVariableLink link)
		{
			// Update links listbox only if variable is currently selected (and hence links list contains variables links)
			if (varListBox.SelectedItem == variable)
			{
				// Save current selection
				int selectedIndex = varPacketsLinksListBox.SelectedIndex;

				// Clear listbox items
				varPacketsLinksListBox.Items.Clear();

				// Add all variable items
				ListBoxHelpers.AddRange(varPacketsLinksListBox, variable.Links);

				// Restore selection index
				varPacketsLinksListBox.SelectedIndex = selectedIndex;
			}
		}

		/// <summary>
		/// Variableses the manager_ variable link removed.
		/// </summary>
		/// <param name="variable">The variable.</param>
		/// <param name="link">The link.</param>
		private void variablesManager_VariableLinkRemoved(PacketPropertyVariable variable, PacketPropertyVariableLink link)
		{
			// Update links listbox only if variable is currently selected (and hence links list contains variables links)
			if (varListBox.SelectedItem == variable)
			{
				varPacketsLinksListBox.Items.Remove(link);
			}
		}

		#endregion

		#region UI Events

		/// <summary>
		/// Handles the Click event of the varAddButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void varAddButton_Click(object sender, EventArgs e)
		{
			try
			{
				// Create and select a new var
				PacketClass				pak	= (PacketClass) packetClassComboBox.SelectedItem;
				ClassMemberPath			mem	= (ClassMemberPath) packetClassPropertyComboBox.SelectedItem;
				PacketPropertyVariable	var = new PacketPropertyVariable(pak, mem, GenerateNewVariableName());

				variablesManager.AddVariable(var);
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex);
			}
		}

		/// <summary>
		/// Handles the Click event of the varRemoveButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void varRemoveButton_Click(object sender, EventArgs e)
		{
			try
			{
				PacketPropertyVariable var = (PacketPropertyVariable) varListBox.SelectedItem;
				if (null == var)
				{
					Log.Info("Please select a variable first.");
				}
				else
				{
					variablesManager.RemoveVariable(var.Name);
				}
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex);
			}
		}

		/// <summary>
		/// Handles the Click event of the varModifyButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void varModifyButton_Click(object sender, EventArgs e)
		{
			try
			{
				// Get selected variable
				PacketPropertyVariable var = (PacketPropertyVariable)varListBox.SelectedItem;

				if (null == var)
				{
					Log.Info("Please select a variable first.");
				}
				else
				{
					// Update its name
					if (!variablesManager.RenameVariable(var.Name, varNameTextBox.Text))
					{
						Log.Info("Failed to rename variable.");
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex);
			}
		}

		/// <summary>
		/// Handles the Click event of the varClearButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void varClearButton_Click(object sender, EventArgs e)
		{
			try
			{
				bool error = false;
				// Iterate all variables
				foreach (PacketPropertyVariable var in new ArrayList(varListBox.Items))
				{
					if (!variablesManager.RemoveVariable(var.Name))
					{
						error = true;
					}
				}

				// Check for error
				if (error)
				{
					Log.Error("Failed to remove at least one variable.");
				}
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex);
			}
		}

		/// <summary>
		/// Handles the MouseClick event of the varListBox control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
		private void varListBox_MouseClick(object sender, MouseEventArgs e)
		{
			try
			{
				UpdateSelectedVariableData();

				// Suggest to modify the name by focusing on textbox and selecting all text
				varNameTextBox.Focus();
				varNameTextBox.SelectAll();
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex);
			}
		}

		/// <summary>
		/// Handles the DoubleClick event of the varListBox control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void varListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateSelectedVariableData();
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex);
			}
		}

		#endregion

		#endregion

		#region Drag&Drop from variables list to packets group

		private Rectangle noDragRect;
		private bool dragStarted = false;

		/// <summary>
		/// Handles the MouseDown event of the varListBox control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
		private void varListBox_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				// Find center
				Size size = SystemInformation.DragSize;
				Point dragStart = e.Location;
				dragStart.X -= size.Width/2;
				dragStart.Y -= size.Height/2;

				// Create no-drag area
				noDragRect = new Rectangle(dragStart, size);

				dragStarted = true;

				// Updates
				UpdateSelectedVariableData();
				UpdateButtonsEnabledState();
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex);
			}
		}

		/// <summary>
		/// Handles the MouseLeave event of the varListBox control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void varListBox_MouseLeave(object sender, EventArgs e)
		{
			// Stop drag-start detection if mouse left vaListBox
			dragStarted = false;
		}

		/// <summary>
		/// Handles the MouseMove event of the varListBox control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
		private void varListBox_MouseMove(object sender, MouseEventArgs e)
		{
			try
			{
				// Start drag&drop only if mouse is outside of no-drag area
				if (dragStarted && !noDragRect.Contains(e.Location))
				{
					DoDragDrop(varListBox.SelectedItem, DragDropEffects.Link);
				}
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex);
			}
		}

		/// <summary>
		/// Handles the DragEnter event of the packetsGroupBox control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
		private void packetsGroupBox_DragEnter(object sender, DragEventArgs e)
		{
			try
			{
				if (e.Data.GetDataPresent(typeof(PacketPropertyVariable)))
				{
					e.Effect = DragDropEffects.Link;
				}
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex);
			}
		}

		/// <summary>
		/// Handles the DragDrop event of the packetsGroupBox control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
		private void packetsGroupBox_DragDrop(object sender, DragEventArgs e)
		{
			try
			{
				object dropData = e.Data.GetData(typeof (PacketPropertyVariable));
				SetCurrentPropertyValue(dropData);
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex);
			}
		}

		#endregion

		#region Variable packet links

		/// <summary>
		/// Handles the Click event of the varPacketsFieldsAddButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void varPacketsFieldsAddButton_Click(object sender, EventArgs e)
		{
			try
			{
				// Get currently selected variable
				PacketPropertyVariable var = (PacketPropertyVariable)varListBox.SelectedItem;

				if (null == var)
				{
					Log.Info("Please select a variable first.");
				}
				else
				{
					// Create variable link
					PacketClass					pak		= (PacketClass) packetClassComboBox.SelectedItem;
					ClassMemberPath				mem		= (ClassMemberPath) packetClassPropertyComboBox.SelectedItem;
					PacketPropertyVariableLink	link	= new PacketPropertyVariableLink(pak, mem);

					// Add selected link to selected variable
					if (!variablesManager.AddVariableLink(var.Name, link))
					{
						Log.Info("Failed to add variable link.");
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex);
			}
		}

		/// <summary>
		/// Handles the Click event of the varPacketsFieldsRemoveButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void varPacketsFieldsRemoveButton_Click(object sender, EventArgs e)
		{
			try
			{
				// Get currently selected data
				PacketPropertyVariable		var		= (PacketPropertyVariable) varListBox.SelectedItem;
				PacketPropertyVariableLink	link	= (PacketPropertyVariableLink) varPacketsLinksListBox.SelectedItem;

				// Both variable and link must be selected
				if (null == var || null == link)
				{
					Log.Info("Please select a variable and link first.");
				}
				else
				{
					// One link must stay
					if (1 >= varPacketsLinksListBox.Items.Count)
					{
						Log.Info("Varible needs at least one active link. Remove the variable instead.");
					}
					// Remove selected link
					else if (!variablesManager.RemoveVariableLink(var.Name, link))
					{
						Log.Info("Failed to remove variable link.");
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex);
			}
		}

		/// <summary>
		/// Handles the Click event of the varPacketsFieldsModifyButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void varPacketsFieldsModifyButton_Click(object sender, EventArgs e)
		{
			try
			{
				// Get currently selected data
				PacketPropertyVariable		var		= (PacketPropertyVariable)varListBox.SelectedItem;
				PacketPropertyVariableLink	oldLink	= (PacketPropertyVariableLink)varPacketsLinksListBox.SelectedItem;

				// Both variable and link must be selected
				if (null == var || null == oldLink)
				{
					Log.Info("Please select a variable and link first.");
				}
				else
				{
					// Create new variable link
					PacketClass					pak		= (PacketClass)packetClassComboBox.SelectedItem;
					ClassMemberPath				mem		= (ClassMemberPath)packetClassPropertyComboBox.SelectedItem;
					PacketPropertyVariableLink	newLink	= new PacketPropertyVariableLink(pak, mem);

					// Update selected link
					if (!variablesManager.ReplaceVariableLink(var.Name, oldLink, newLink))
					{
						Log.Info("Failed to update variable link.");
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex);
			}
		}

		/// <summary>
		/// Handles the Click event of the varPacketsFieldsClearButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void varPacketsFieldsClearButton_Click(object sender, EventArgs e)
		{
			try
			{
				// Get currently selected data
				PacketPropertyVariable		var		= (PacketPropertyVariable)varListBox.SelectedItem;

				// Check selected variable
				if (null == var)
				{
					Log.Info("Please select a variable first.");
				}
					// Check count of links in listbox
				else if (varPacketsLinksListBox.Items.Count <= 1)
				{
					Log.Info("At least one link must stay. Please delete the variable instead.");
				}
				else
				{
					// Keep selected item
					PacketPropertyVariableLink keepLink = varPacketsLinksListBox.SelectedItem as PacketPropertyVariableLink;
					if (null == keepLink)
					{
						// Keep first entry in list if no selection
						keepLink = (PacketPropertyVariableLink) varPacketsLinksListBox.Items[0];
					}

					// Iterate all links of the variable
					bool error = false;
					// Clone the list because it is to be modified
					foreach (PacketPropertyVariableLink link in new List<PacketPropertyVariableLink>(var.Links))
					{
						if (keepLink != link && !variablesManager.RemoveVariableLink(var.Name, link))
						{
							// Failed to delete at least one link
							error = true;
						}
					}

					// Check for error
					if (error)
					{
						Log.Info("Failed to delete at least one link.");
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex);
			}
		}

		/// <summary>
		/// Handles the MouseDoubleClick event of the varPacketsLinksListBox control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
		private void varPacketsLinksListBox_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			try
			{
				PacketPropertyVariableLink link = (PacketPropertyVariableLink)varPacketsLinksListBox.SelectedItem;
				SelectPacketAndProperty(link.PacketClass, link.PacketClassMemberPath);
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex);
			}
		}

		/// <summary>
		/// Handles the SelectedIndexChanged event of the varPacketsLinksListBox control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void varPacketsLinksListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateButtonsEnabledState();
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex);
			}
		}

		#endregion
	}
}
