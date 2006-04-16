using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using PacketLogConverter.LogReaders;
using PacketLogConverter.LogWriters;

namespace PacketLogConverter
{
	public class MainForm : Form
	{
		private MainMenu mainMenu1;
		private TabPage logInfoTab;
		private RichTextBox logDataText;
		private MenuItem menuOpenFile;
		private MenuItem menuSaveFile;
		private OpenFileDialog openLogDialog;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		private Label label1;
		private TextBox li_clientVersion;
		private TabPage logDataTab;
		private TabControl mainFormTabs;
		private TabPage instantParseTab;
		private Label label2;
		private GroupBox inputDataGroupBox;
		private GroupBox instantResultGroupBox1;
		private RichTextBox instantParseOut;
		private Label label4;
		private Label label5;
		private TextBox instantVersion;
		private TextBox instantCode;
		private Label label6;
		private RadioButton instantClientToServer;
		private RadioButton instantServerToClient;
		private SaveFileDialog saveLogDialog;
		private Label label7;
		private Label label8;
		private TextBox li_packetsCount;
		private TextBox instantParseInput;
		private TextBox logDataFindTextBox;
		private Button logDataFindButton;
		private Button button1;
		private CheckBox logDataDisableUpdatesCheckBox;
		private Button applyButton;
		private Label li_changesLabel;
		private MenuItem menuItem1;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox li_unknownPacketsCount;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.CheckBox li_ignoreVersionChanges;
		private System.Windows.Forms.MenuItem menuOpenAnother;
		private System.Windows.Forms.OpenFileDialog openAnotherLogDialog;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem packetTimeDiffMenuItem;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuRecentFiles;
		private System.Windows.Forms.MenuItem menuExitApp;
		private Label label3;

		public MainForm()
		{
			m_progress = new Progress(this);
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuOpenFile = new System.Windows.Forms.MenuItem();
			this.menuOpenAnother = new System.Windows.Forms.MenuItem();
			this.menuSaveFile = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.packetTimeDiffMenuItem = new System.Windows.Forms.MenuItem();
			this.openLogDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveLogDialog = new System.Windows.Forms.SaveFileDialog();
			this.mainFormTabs = new System.Windows.Forms.TabControl();
			this.instantParseTab = new System.Windows.Forms.TabPage();
			this.instantResultGroupBox1 = new System.Windows.Forms.GroupBox();
			this.instantParseOut = new System.Windows.Forms.RichTextBox();
			this.inputDataGroupBox = new System.Windows.Forms.GroupBox();
			this.button1 = new System.Windows.Forms.Button();
			this.instantParseInput = new System.Windows.Forms.TextBox();
			this.instantServerToClient = new System.Windows.Forms.RadioButton();
			this.instantClientToServer = new System.Windows.Forms.RadioButton();
			this.label6 = new System.Windows.Forms.Label();
			this.instantCode = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.instantVersion = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.logInfoTab = new System.Windows.Forms.TabPage();
			this.li_ignoreVersionChanges = new System.Windows.Forms.CheckBox();
			this.label9 = new System.Windows.Forms.Label();
			this.li_unknownPacketsCount = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.li_changesLabel = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.applyButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.li_clientVersion = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.li_packetsCount = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.logDataTab = new System.Windows.Forms.TabPage();
			this.logDataDisableUpdatesCheckBox = new System.Windows.Forms.CheckBox();
			this.logDataFindButton = new System.Windows.Forms.Button();
			this.logDataFindTextBox = new System.Windows.Forms.TextBox();
			this.logDataText = new System.Windows.Forms.RichTextBox();
			this.openAnotherLogDialog = new System.Windows.Forms.OpenFileDialog();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuRecentFiles = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuExitApp = new System.Windows.Forms.MenuItem();
			this.mainFormTabs.SuspendLayout();
			this.instantParseTab.SuspendLayout();
			this.instantResultGroupBox1.SuspendLayout();
			this.inputDataGroupBox.SuspendLayout();
			this.logInfoTab.SuspendLayout();
			this.logDataTab.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem2});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuOpenFile,
																					  this.menuOpenAnother,
																					  this.menuSaveFile,
																					  this.menuItem3,
																					  this.menuRecentFiles,
																					  this.menuItem5,
																					  this.menuExitApp});
			this.menuItem1.Text = "&File";
			// 
			// menuOpenFile
			// 
			this.menuOpenFile.Index = 0;
			this.menuOpenFile.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.menuOpenFile.Text = "&Open...";
			this.menuOpenFile.Click += new System.EventHandler(this.menuOpenFile_Click);
			// 
			// menuOpenAnother
			// 
			this.menuOpenAnother.Index = 1;
			this.menuOpenAnother.Text = "Open another...";
			this.menuOpenAnother.Click += new System.EventHandler(this.menuOpenAnother_Click);
			// 
			// menuSaveFile
			// 
			this.menuSaveFile.Enabled = false;
			this.menuSaveFile.Index = 2;
			this.menuSaveFile.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.menuSaveFile.Text = "&Save...";
			this.menuSaveFile.Click += new System.EventHandler(this.menuSaveFile_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.packetTimeDiffMenuItem});
			this.menuItem2.Text = "&View";
			// 
			// packetTimeDiffMenuItem
			// 
			this.packetTimeDiffMenuItem.Index = 0;
			this.packetTimeDiffMenuItem.Text = "Packet time difference";
			this.packetTimeDiffMenuItem.Click += new System.EventHandler(this.packetTimeDiffMenuItem_Click);
			// 
			// openLogDialog
			// 
			this.openLogDialog.Multiselect = true;
			// 
			// saveLogDialog
			// 
			this.saveLogDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveLogDialog_FileOk);
			// 
			// mainFormTabs
			// 
			this.mainFormTabs.Controls.Add(this.instantParseTab);
			this.mainFormTabs.Controls.Add(this.logInfoTab);
			this.mainFormTabs.Controls.Add(this.logDataTab);
			this.mainFormTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainFormTabs.Location = new System.Drawing.Point(0, 0);
			this.mainFormTabs.Name = "mainFormTabs";
			this.mainFormTabs.SelectedIndex = 0;
			this.mainFormTabs.Size = new System.Drawing.Size(592, 373);
			this.mainFormTabs.TabIndex = 0;
			// 
			// instantParseTab
			// 
			this.instantParseTab.Controls.Add(this.instantResultGroupBox1);
			this.instantParseTab.Controls.Add(this.inputDataGroupBox);
			this.instantParseTab.Location = new System.Drawing.Point(4, 22);
			this.instantParseTab.Name = "instantParseTab";
			this.instantParseTab.Size = new System.Drawing.Size(584, 347);
			this.instantParseTab.TabIndex = 2;
			this.instantParseTab.Text = "Instant parse";
			// 
			// instantResultGroupBox1
			// 
			this.instantResultGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.instantResultGroupBox1.Controls.Add(this.instantParseOut);
			this.instantResultGroupBox1.Location = new System.Drawing.Point(0, 176);
			this.instantResultGroupBox1.Name = "instantResultGroupBox1";
			this.instantResultGroupBox1.Size = new System.Drawing.Size(584, 172);
			this.instantResultGroupBox1.TabIndex = 2;
			this.instantResultGroupBox1.TabStop = false;
			this.instantResultGroupBox1.Text = "Instant result";
			// 
			// instantParseOut
			// 
			this.instantParseOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.instantParseOut.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.instantParseOut.Location = new System.Drawing.Point(8, 16);
			this.instantParseOut.Name = "instantParseOut";
			this.instantParseOut.ReadOnly = true;
			this.instantParseOut.Size = new System.Drawing.Size(568, 148);
			this.instantParseOut.TabIndex = 0;
			this.instantParseOut.Text = "";
			this.instantParseOut.WordWrap = false;
			// 
			// inputDataGroupBox
			// 
			this.inputDataGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.inputDataGroupBox.Controls.Add(this.button1);
			this.inputDataGroupBox.Controls.Add(this.instantParseInput);
			this.inputDataGroupBox.Controls.Add(this.instantServerToClient);
			this.inputDataGroupBox.Controls.Add(this.instantClientToServer);
			this.inputDataGroupBox.Controls.Add(this.label6);
			this.inputDataGroupBox.Controls.Add(this.instantCode);
			this.inputDataGroupBox.Controls.Add(this.label5);
			this.inputDataGroupBox.Controls.Add(this.instantVersion);
			this.inputDataGroupBox.Controls.Add(this.label4);
			this.inputDataGroupBox.Location = new System.Drawing.Point(0, 0);
			this.inputDataGroupBox.Name = "inputDataGroupBox";
			this.inputDataGroupBox.Size = new System.Drawing.Size(584, 168);
			this.inputDataGroupBox.TabIndex = 1;
			this.inputDataGroupBox.TabStop = false;
			this.inputDataGroupBox.Text = "Input data";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(512, 23);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(64, 23);
			this.button1.TabIndex = 8;
			this.button1.Text = "Clear";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// instantParseInput
			// 
			this.instantParseInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.instantParseInput.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.instantParseInput.Location = new System.Drawing.Point(8, 48);
			this.instantParseInput.Multiline = true;
			this.instantParseInput.Name = "instantParseInput";
			this.instantParseInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.instantParseInput.Size = new System.Drawing.Size(568, 112);
			this.instantParseInput.TabIndex = 7;
			this.instantParseInput.Text = "";
			this.instantParseInput.WordWrap = false;
			this.instantParseInput.TextChanged += new System.EventHandler(this.InstantParseUpdateEvent);
			// 
			// instantServerToClient
			// 
			this.instantServerToClient.Checked = true;
			this.instantServerToClient.Location = new System.Drawing.Point(312, 24);
			this.instantServerToClient.Name = "instantServerToClient";
			this.instantServerToClient.Size = new System.Drawing.Size(104, 20);
			this.instantServerToClient.TabIndex = 6;
			this.instantServerToClient.TabStop = true;
			this.instantServerToClient.Text = "server to client";
			this.instantServerToClient.CheckedChanged += new System.EventHandler(this.InstantParseUpdateEvent);
			// 
			// instantClientToServer
			// 
			this.instantClientToServer.Location = new System.Drawing.Point(416, 24);
			this.instantClientToServer.Name = "instantClientToServer";
			this.instantClientToServer.Size = new System.Drawing.Size(96, 20);
			this.instantClientToServer.TabIndex = 5;
			this.instantClientToServer.Text = "client to server";
			this.instantClientToServer.CheckedChanged += new System.EventHandler(this.InstantParseUpdateEvent);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(248, 24);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(56, 20);
			this.label6.TabIndex = 4;
			this.label6.Text = "Direction:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// instantCode
			// 
			this.instantCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.instantCode.Location = new System.Drawing.Point(176, 24);
			this.instantCode.MaxLength = 10;
			this.instantCode.Name = "instantCode";
			this.instantCode.Size = new System.Drawing.Size(64, 20);
			this.instantCode.TabIndex = 3;
			this.instantCode.Text = "";
			this.instantCode.TextChanged += new System.EventHandler(this.InstantParseUpdateEvent);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(128, 24);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(40, 20);
			this.label5.TabIndex = 2;
			this.label5.Text = "Code:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// instantVersion
			// 
			this.instantVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.instantVersion.Location = new System.Drawing.Point(64, 24);
			this.instantVersion.MaxLength = 10;
			this.instantVersion.Name = "instantVersion";
			this.instantVersion.Size = new System.Drawing.Size(56, 20);
			this.instantVersion.TabIndex = 1;
			this.instantVersion.Text = "";
			this.instantVersion.TextChanged += new System.EventHandler(this.InstantParseUpdateEvent);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 20);
			this.label4.TabIndex = 0;
			this.label4.Text = "Version:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// logInfoTab
			// 
			this.logInfoTab.Controls.Add(this.li_ignoreVersionChanges);
			this.logInfoTab.Controls.Add(this.label9);
			this.logInfoTab.Controls.Add(this.li_unknownPacketsCount);
			this.logInfoTab.Controls.Add(this.label10);
			this.logInfoTab.Controls.Add(this.li_changesLabel);
			this.logInfoTab.Controls.Add(this.label3);
			this.logInfoTab.Controls.Add(this.applyButton);
			this.logInfoTab.Controls.Add(this.label2);
			this.logInfoTab.Controls.Add(this.li_clientVersion);
			this.logInfoTab.Controls.Add(this.label1);
			this.logInfoTab.Controls.Add(this.label7);
			this.logInfoTab.Controls.Add(this.li_packetsCount);
			this.logInfoTab.Controls.Add(this.label8);
			this.logInfoTab.Location = new System.Drawing.Point(4, 22);
			this.logInfoTab.Name = "logInfoTab";
			this.logInfoTab.Size = new System.Drawing.Size(584, 347);
			this.logInfoTab.TabIndex = 0;
			this.logInfoTab.Text = "Log info";
			// 
			// li_ignoreVersionChanges
			// 
			this.li_ignoreVersionChanges.Location = new System.Drawing.Point(456, 112);
			this.li_ignoreVersionChanges.Name = "li_ignoreVersionChanges";
			this.li_ignoreVersionChanges.Size = new System.Drawing.Size(104, 32);
			this.li_ignoreVersionChanges.TabIndex = 9;
			this.li_ignoreVersionChanges.Text = "Ignore version packets";
			this.li_ignoreVersionChanges.CheckedChanged += new System.EventHandler(this.li_changes_Event);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(240, 78);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(320, 32);
			this.label9.TabIndex = 8;
			this.label9.Text = "The count of unknown packets in the log.";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// li_unknownPacketsCount
			// 
			this.li_unknownPacketsCount.Location = new System.Drawing.Point(120, 84);
			this.li_unknownPacketsCount.MaxLength = 10;
			this.li_unknownPacketsCount.Name = "li_unknownPacketsCount";
			this.li_unknownPacketsCount.ReadOnly = true;
			this.li_unknownPacketsCount.TabIndex = 7;
			this.li_unknownPacketsCount.Text = "";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(16, 82);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(100, 24);
			this.label10.TabIndex = 6;
			this.label10.Text = "Unknown packets:";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// li_changesLabel
			// 
			this.li_changesLabel.Location = new System.Drawing.Point(16, 13);
			this.li_changesLabel.Name = "li_changesLabel";
			this.li_changesLabel.TabIndex = 5;
			this.li_changesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(240, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(320, 32);
			this.label3.TabIndex = 4;
			this.label3.Text = "Parse the log again using current info.";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// applyButton
			// 
			this.applyButton.Location = new System.Drawing.Point(120, 13);
			this.applyButton.Name = "applyButton";
			this.applyButton.Size = new System.Drawing.Size(100, 23);
			this.applyButton.TabIndex = 3;
			this.applyButton.Text = "Apply";
			this.applyButton.Click += new System.EventHandler(this.li_applyButton_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(240, 112);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(208, 32);
			this.label2.TabIndex = 2;
			this.label2.Text = "Can be set explicitly if no version info is in the log else it will be overwriten" +
				".";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// li_clientVersion
			// 
			this.li_clientVersion.Location = new System.Drawing.Point(120, 118);
			this.li_clientVersion.MaxLength = 10;
			this.li_clientVersion.Name = "li_clientVersion";
			this.li_clientVersion.TabIndex = 1;
			this.li_clientVersion.Text = "";
			this.li_clientVersion.TextChanged += new System.EventHandler(this.li_changes_Event);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 116);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Log version:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(240, 44);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(320, 32);
			this.label7.TabIndex = 2;
			this.label7.Text = "The count of successfully parsed packets.";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// li_packetsCount
			// 
			this.li_packetsCount.Location = new System.Drawing.Point(120, 50);
			this.li_packetsCount.MaxLength = 10;
			this.li_packetsCount.Name = "li_packetsCount";
			this.li_packetsCount.ReadOnly = true;
			this.li_packetsCount.TabIndex = 1;
			this.li_packetsCount.Text = "";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(16, 48);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(100, 24);
			this.label8.TabIndex = 0;
			this.label8.Text = "Packets count:";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// logDataTab
			// 
			this.logDataTab.Controls.Add(this.logDataDisableUpdatesCheckBox);
			this.logDataTab.Controls.Add(this.logDataFindButton);
			this.logDataTab.Controls.Add(this.logDataFindTextBox);
			this.logDataTab.Controls.Add(this.logDataText);
			this.logDataTab.Location = new System.Drawing.Point(4, 22);
			this.logDataTab.Name = "logDataTab";
			this.logDataTab.Size = new System.Drawing.Size(584, 347);
			this.logDataTab.TabIndex = 1;
			this.logDataTab.Text = "Log data";
			// 
			// logDataDisableUpdatesCheckBox
			// 
			this.logDataDisableUpdatesCheckBox.Location = new System.Drawing.Point(16, 8);
			this.logDataDisableUpdatesCheckBox.Name = "logDataDisableUpdatesCheckBox";
			this.logDataDisableUpdatesCheckBox.TabIndex = 3;
			this.logDataDisableUpdatesCheckBox.Text = "disable updates";
			this.logDataDisableUpdatesCheckBox.CheckedChanged += new System.EventHandler(this.logDataDisableUpdatesCheckBox_CheckedChanged);
			// 
			// logDataFindButton
			// 
			this.logDataFindButton.Location = new System.Drawing.Point(8, 40);
			this.logDataFindButton.Name = "logDataFindButton";
			this.logDataFindButton.TabIndex = 2;
			this.logDataFindButton.Text = "&Find";
			this.logDataFindButton.Click += new System.EventHandler(this.logDataFindButton_Click);
			// 
			// logDataFindTextBox
			// 
			this.logDataFindTextBox.Location = new System.Drawing.Point(88, 40);
			this.logDataFindTextBox.Name = "logDataFindTextBox";
			this.logDataFindTextBox.Size = new System.Drawing.Size(488, 20);
			this.logDataFindTextBox.TabIndex = 1;
			this.logDataFindTextBox.Text = "";
			this.logDataFindTextBox.WordWrap = false;
			this.logDataFindTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.logDataFindText_KeyPress);
			// 
			// logDataText
			// 
			this.logDataText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.logDataText.DetectUrls = false;
			this.logDataText.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.logDataText.Location = new System.Drawing.Point(0, 72);
			this.logDataText.Name = "logDataText";
			this.logDataText.ReadOnly = true;
			this.logDataText.Size = new System.Drawing.Size(584, 272);
			this.logDataText.TabIndex = 0;
			this.logDataText.Text = "";
			this.logDataText.WordWrap = false;
			this.logDataText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.logDataFindText_KeyPress);
			// 
			// openAnotherLogDialog
			// 
			this.openAnotherLogDialog.Multiselect = true;
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 3;
			this.menuItem3.Text = "-";
			// 
			// menuRecentFiles
			// 
			this.menuRecentFiles.Index = 4;
			this.menuRecentFiles.Text = "&Recent Files";
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 5;
			this.menuItem5.Text = "-";
			// 
			// menuExitApp
			// 
			this.menuExitApp.Index = 6;
			this.menuExitApp.Text = "E&xit";
			this.menuExitApp.Click += new System.EventHandler(this.menuExitApp_Click);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(592, 373);
			this.Controls.Add(this.mainFormTabs);
			this.Menu = this.mainMenu1;
			this.MinimumSize = new System.Drawing.Size(600, 400);
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.mainFormTabs.ResumeLayout(false);
			this.instantParseTab.ResumeLayout(false);
			this.instantResultGroupBox1.ResumeLayout(false);
			this.inputDataGroupBox.ResumeLayout(false);
			this.logInfoTab.ResumeLayout(false);
			this.logDataTab.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		
		private static MainForm m_formInstance;

		/// <summary>
		/// Gets the form instance.
		/// </summary>
		/// <value>The form instance.</value>
		public static MainForm Instance
		{
			get { return m_formInstance; }
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			try
			{
				Thread.CurrentThread.Name = "Main";
				AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionCallback);
				m_formInstance = new MainForm();
				Application.Run(m_formInstance);
				Application.Exit();

				Debug.Close();
			}
			catch (Exception e)
			{
				Log.Error("application loop", e);
			}
		}

		private static void UnhandledExceptionCallback(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.ExceptionObject is Exception)
				Log.Error("unhandled exception", (Exception) e.ExceptionObject);
			else
				Log.Error(e.ExceptionObject.ToString());
		}
		
		#region Events
		
		public delegate void LogReaderDelegate(ILogReader reader);
		
		public event LogReaderDelegate FilesLoaded;
		
		#endregion

		#region Misc

		private readonly Progress m_progress;
		
		private PacketLog m_currentLog;

		public PacketLog CurrentLog
		{
			get { return m_currentLog; }
			set
			{
				PacketLog oldLog = m_currentLog;
				m_currentLog = value;
				UpdateLogDataTab();
				UpdateLogInfoTab();
				UpdateCaption();
				menuSaveFile.Enabled = (value != null);
				if (oldLog != null)
					GC.Collect(GC.MaxGeneration);
			}
		}

		private ArrayList m_logReaders = new ArrayList();
		private ArrayList m_logWriters = new ArrayList();
		private ArrayList m_logFilters = new ArrayList();
		private ArrayList m_logActions = new ArrayList();

		private void MainForm_Load(object sender, EventArgs e)
		{
			SortedList readers = new SortedList();
			SortedList writers = new SortedList();
			SortedList filters = new SortedList();
			SortedList actions = new SortedList();
			Hashtable readerFilterStrings = new Hashtable();
			Hashtable writerFilterStrings = new Hashtable();
			Hashtable filterMenuItems = new Hashtable();
			Hashtable actionMenuItems = new Hashtable();

			foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (Type type in asm.GetTypes())
				{
					if (!type.IsClass) continue;

					try
					{
						// is log reader
						if (typeof(ILogReader).IsAssignableFrom(type))
						{
							foreach (LogReaderAttribute attr in type.GetCustomAttributes(typeof(LogReaderAttribute), false))
							{
								int position = -attr.Priority;
								while (readers.ContainsKey(position))
									++position;
								readers.Add(position, Activator.CreateInstance(type));
								readerFilterStrings.Add(position, string.Format("{0} ({1})|{1}", attr.Description, attr.FileMask));
							}
						}

						// is log writer
						if (typeof(ILogWriter).IsAssignableFrom(type))
						{
							foreach (LogWriterAttribute attr in type.GetCustomAttributes(typeof(LogWriterAttribute), false))
							{
								int position = -attr.Priority;
								while (writers.ContainsKey(position))
									++position;
								writers.Add(position, Activator.CreateInstance(type));
								writerFilterStrings.Add(position, string.Format("{0} ({1})|{1}", attr.Description, attr.FileMask));
							}
						}

						// is log filter
						if (typeof(ILogFilter).IsAssignableFrom(type))
						{
							foreach (LogFilterAttribute attr in type.GetCustomAttributes(typeof(LogFilterAttribute), false))
							{
								string name = attr.FilterName;
								if (name == null || name.Length <= 0)
									name = type.Name;
								MenuItem filterMenuItem = new MenuItem(name, new EventHandler(FilterClick_Event));
								filterMenuItem.ShowShortcut = true;
								filterMenuItem.Shortcut = attr.ShortcutKey;

								int position = -attr.Priority;
								while (filters.ContainsKey(position))
									++position;
                                filters.Add(position, Activator.CreateInstance(type));
								filterMenuItems.Add(position, filterMenuItem);
							}
						}

						// is log action
						if (typeof(ILogAction).IsAssignableFrom(type))
						{
							foreach (LogActionAttribute attr in type.GetCustomAttributes(typeof(LogActionAttribute), false))
							{
								string name = attr.Name;
								if (name == null || name.Length <= 0)
									name = type.Name;
								MenuItem actionMenuItem = new MenuItem(name, new EventHandler(LogActionClick_Event));

								int position = -attr.Priority;
								while (actions.ContainsKey(position))
									++position;
								actions.Add(position, Activator.CreateInstance(type));
								actionMenuItems.Add(position, actionMenuItem);
							}
						}
					}
					catch (Exception e1)
					{
						Log.Error("loading type: " + type.FullName, e1);
					}
				}
			}

			string openFilter = "";
			foreach (DictionaryEntry entry in readers)
			{
				int position = (int)entry.Key;
				ILogReader reader = (ILogReader)entry.Value;
				if (openFilter.Length > 0)
					openFilter += "|";
				openFilter += (string)readerFilterStrings[position];
				m_logReaders.Add(reader);
			}
			openLogDialog.Filter = openFilter;
			openAnotherLogDialog.Filter = openFilter;

			string saveFilter = "";
			foreach (DictionaryEntry entry in writers)
			{
				int position = (int)entry.Key;
				ILogWriter writer = (ILogWriter)entry.Value;
				if (saveFilter.Length > 0)
					saveFilter += "|";
				saveFilter += (string)writerFilterStrings[position];
				m_logWriters.Add(writer);
			}
			saveLogDialog.Filter = saveFilter;


			if (filters.Count > 0)
			{
				ArrayList menu = new ArrayList();
				
				m_combineFiltersMenuItem = new MenuItem("Combine filters", new EventHandler(FilterClick_Event));
				m_combineFiltersMenuItem.Checked = FilterManager.CombineFilters;
				menu.Add(m_combineFiltersMenuItem);
				
				m_invertCheckMenuItem = new MenuItem("Invert check", new EventHandler(FilterClick_Event));
				m_invertCheckMenuItem.Checked = false;
				menu.Add(m_invertCheckMenuItem);
				
				menu.Add(new MenuItem("-"));

				foreach (DictionaryEntry entry in filters)
				{
					int position = (int)entry.Key;
					ILogFilter filter = (ILogFilter)entry.Value;
					menu.Add(filterMenuItems[position]);
					m_logFilters.Add(filter);
				}

				mainMenu1.MenuItems.Add("F&ilters", (MenuItem[])menu.ToArray(typeof (MenuItem)));
			}

			if (actions.Count > 0)
			{
				ArrayList actionsMenu = new ArrayList();
				foreach (DictionaryEntry entry in actions)
				{
					int position = (int)entry.Key;
					ILogAction action = (ILogAction)entry.Value;
					actionsMenu.Add(actionMenuItems[position]);
					m_logActions.Add(action);
				}
				m_logActionsMenu = new ContextMenu((MenuItem[])actionsMenu.ToArray(typeof (MenuItem)));
				logDataText.MouseDown += new System.Windows.Forms.MouseEventHandler(logDataText_MouseClickEvent);
			}
			
			UpdateRecentFilesMenu();

			UpdateCaption();
		}

		private void UpdateCaption()
		{
			string caption = "packet log converter v" + Assembly.GetExecutingAssembly().GetName().Version;
			if (CurrentLog != null && CurrentLog.StreamName != null && CurrentLog.StreamName.Length > 0)
			{
				caption += ": " + CurrentLog.StreamName;
			}
			Text = caption;
		}

		#region open files

		private void LoadFiles(ILogReader reader, PacketLog log, string[] files, ProgressCallback progress)
		{
			foreach (string fileName in files)
			{
				try
				{
					m_progress.SetDescription("Loading file: " + fileName + "...");
					FileInfo fileInfo = new FileInfo(fileName);
					if (!fileInfo.Exists)
					{
						Log.Info("File \"" + fileInfo.FullName + "\" doesn't exist, ignored.");
						continue;
					}
					using(FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						log.AddRange(reader.ReadLog(new BufferedStream(stream, 64*1024), progress));
					}
					if (log.StreamName.Length > 0)
						log.StreamName += "; ";
					log.StreamName += fileInfo.Name;
					AddRecentFile(fileInfo.FullName);
				}
				catch (Exception e)
				{
					Log.Error("loading files", e);
				}
			}
		}
		
		private class OpenData
		{
			public string[] Files;
			public ILogReader Reader;
		}

		private void menuOpenFile_Click(object sender, EventArgs e)
		{
			if (m_logReaders.Count <= 0)
			{
				Log.Info("No log readers found.");
				return;
			}

			if (openLogDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					OpenData data = new OpenData();
					data.Files = openLogDialog.FileNames;
					data.Reader = (ILogReader) m_logReaders[openLogDialog.FilterIndex - 1];
					CurrentLog = null;
					m_progress.SetDescription("Reading file(s)...");
					m_progress.WorkFinishedCallback = new StateObjectCallback(OpenFileFinishedCallback);
					m_progress.Start(new WorkCallback(OpenFilesWorkCallback), data);
				}
				catch (Exception e1)
				{
					Log.Error("opening file", e1);
				}
				finally
				{
					UpdateCaption();
				}
			}
		}

		private void OpenFilesWorkCallback(ProgressCallback progress, object state)
		{
			OpenData data = (OpenData) state;
			PacketLog log = CurrentLog;
			if (log == null)
				log = new PacketLog();
			log.IgnoreVersionChanges = li_ignoreVersionChanges.Checked;
			LoadFiles(data.Reader, log, data.Files, progress);
			m_currentLog = log;

			m_progress.SetDescription("Initializing log and packets...");
			CurrentLog.Init(3, progress);
		}
		
		private void OpenFileFinishedCallback(object state)
		{
			if (m_currentLog != null && m_currentLog.Count > 100000)
			{
				logDataDisableUpdatesCheckBox.Checked = true;
				mainFormTabs.SelectedTab = logInfoTab;
			}
			else
			{
				mainFormTabs.SelectedTab = logDataTab;
			}
			Refresh();
			CurrentLog = m_currentLog;
			li_clientVersion.Text = CurrentLog.Version.ToString();
			logDataText.Focus();
			
			OpenData data = (OpenData) state;
			LogReaderDelegate e = FilesLoaded;
			if (e != null)
				e(data.Reader);
		}

		private void menuOpenAnother_Click(object sender, System.EventArgs e)
		{
			if (m_logReaders.Count > 0)
			{
				openAnotherLogDialog.InitialDirectory = openLogDialog.InitialDirectory;
				if (openAnotherLogDialog.ShowDialog() == DialogResult.OK)
				{
					try
					{
						if (m_currentLog != null && m_currentLog.Count > 100000)
						{
							logDataDisableUpdatesCheckBox.Checked = true;
						}
						OpenData data = new OpenData();
						data.Files = openAnotherLogDialog.FileNames;
						data.Reader = (ILogReader) m_logReaders[openAnotherLogDialog.FilterIndex - 1];
						CurrentLog = null;
						m_progress.SetDescription("Reading file(s)...");
						m_progress.WorkFinishedCallback = new StateObjectCallback(OpenFileFinishedCallback);
						m_progress.Start(new WorkCallback(OpenFilesWorkCallback), data);
					}
					catch (Exception e1)
					{
						Log.Error("opening another file", e1);
					}
					finally
					{
						UpdateCaption();
					}
				}
			}
			else
				Log.Info("No log readers found.");
		}

		#endregion

		private void menuSaveFile_Click(object sender, EventArgs e)
		{
			if (CurrentLog == null)
			{
				Log.Info("Nothing to save.");
				return;
			}

			if (m_logWriters.Count > 0)
				saveLogDialog.ShowDialog();
			else
				Log.Info("No log writers found.");
		}

		private void saveLogDialog_FileOk(object sender, CancelEventArgs e)
		{
			try
			{
				m_progress.SetDescription("Saving file...");
				m_progress.Start(new WorkCallback(SaveFileProc), saveLogDialog.FileName);
			}
			catch (Exception e1)
			{
				Log.Error("saving file", e1);
			}
		}

		private void SaveFileProc(ProgressCallback callback, object state)
		{
			ILogWriter writer = (ILogWriter)m_logWriters[saveLogDialog.FilterIndex-1];
			using(FileStream stream = new FileStream(saveLogDialog.FileName, FileMode.Create))
			{
				writer.WriteLog(CurrentLog, stream, callback);
			}
			AddRecentFile(new FileInfo(saveLogDialog.FileName).FullName);
		}

		private void menuExitApp_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		#endregion

		#region Recent Files
		
		const string RegKeysPath = @"Software\DawnOfLight\PacketLogConverter\";

		private void UpdateRecentFilesMenu()
		{
			ArrayList menu = new ArrayList();

			RegistryKey key = Registry.CurrentUser.OpenSubKey(RegKeysPath);
			
			if (key != null)
			{
				using (key)
				{
					ArrayList keyNames = new ArrayList();
					foreach (string subvalue in key.GetValueNames())
					{
						if (subvalue.StartsWith("RecentFile"))
							keyNames.Add(subvalue);
					}
					keyNames.Sort();
					foreach (string subkey in keyNames)
					{
						string path = (string) key.GetValue(subkey);
						if (path.Length > 0)
						{
							MenuItem item = new MenuItem(path.Replace("&", "&&"));
							item.Click += new EventHandler(RecentFileMenuItem_Click);
							menu.Add(item);
						}
					}
				}
			}

			menuRecentFiles.MenuItems.Clear();
			
			if (menu.Count > 0)
			{
				menuRecentFiles.Enabled = true;
				menuRecentFiles.MenuItems.AddRange((MenuItem[]) menu.ToArray(typeof (MenuItem)));
			}
			else
			{
				menuRecentFiles.Enabled = false;
			}
		}
		
		private void AddRecentFile(string fileName)
		{
			RegistryKey key = Registry.CurrentUser.CreateSubKey(RegKeysPath);
			
			if (key != null)
			{
				using (key)
				{
					SortedList filesList = new SortedList();
					foreach (string subvalue in key.GetValueNames())
					{
						if (subvalue.StartsWith("RecentFile"))
						{
							string val = (string) key.GetValue(subvalue, "");
							if (val != fileName)
								filesList.Add(subvalue, val);
						}
					}
					key.SetValue("RecentFile00", fileName);
					int index = 1;
					foreach (DictionaryEntry entry in filesList)
					{
						string file = (string) entry.Value;
						string newKey = string.Format("RecentFile{0:D2}", index);
						key.SetValue(newKey, file);
						if (index++ > 8)
							break;
					}
				}
				
				UpdateRecentFilesMenu();
			}
		}

		private void RecentFileMenuItem_Click(object sender, EventArgs e)
		{
//			if (m_logReaders.Count <= 0)
//				return;
			
			MenuItem item = (MenuItem) sender;
			
			CurrentLog = null;
			OpenData data = new OpenData();
			data.Reader = new AutoDetectLogReader();
			data.Files = new string[] { item.Text.Replace("&&", "&") };
			m_progress.SetDescription("Opening recent file...");
			m_progress.WorkFinishedCallback = new StateObjectCallback(OpenFileFinishedCallback);
			m_progress.Start(new WorkCallback(OpenFilesWorkCallback), data);
		}

		#endregion

		#region Log data tab

		private void UpdateLogDataTab()
		{
			try
			{
				string newTabName = "Log data";
				if (FilterManager.FiltersCount > 0)
				{
					newTabName += " (filtered)";
				}
				logDataTab.Text = newTabName;

				if (logDataDisableUpdatesCheckBox.Checked)
					return;

				if (CurrentLog == null)
				{
					logDataText.Clear();
					return;
				}

				int selectedPacketIndex = 0;
				if (logDataText.TextLength > 0)
					selectedPacketIndex = CurrentLog.GetPacketIndexByTextIndex(logDataText.SelectionStart);
				int packetsCount = 0;
				bool timeDiff = packetTimeDiffMenuItem.Checked;
				TimeSpan baseTime = new TimeSpan(0);

				StringBuilder text = new StringBuilder();
				foreach (Packet pak in CurrentLog)
				{
					if (FilterManager.IsPacketIgnored(pak))
					{
						pak.LogTextIndex = -1;
						continue;
					}
					pak.LogTextIndex = text.Length;
					++packetsCount;
					text.Append(pak.ToHumanReadableString(baseTime)).Append('\n');
					if (timeDiff)
						baseTime = pak.Time;
				}

				newTabName = string.Format("{0} ({1}{2:N0} packets)", newTabName, (timeDiff ? "time diff, " : ""), packetsCount);
				logDataTab.Text = newTabName;

				logDataText.SelectionIndent = 4;
				logDataText.Text = text.ToString();
				int restoreIndex = -1;
				if (CurrentLog.Count > 0)
					restoreIndex = CurrentLog[selectedPacketIndex].LogTextIndex;
				if (restoreIndex >= 0)
					logDataText.SelectionStart = restoreIndex;
			}
			catch (Exception e)
			{
				Log.Error("updating data tab", e);
			}
		}

		private void logDataFindButton_Click(object sender, EventArgs e)
		{
			LogDataFindText();
		}

		private void LogDataFindText()
		{
			int start = logDataText.SelectionStart+1;
			if (start >= logDataText.TextLength || start < 0)
				start = 0;
			logDataText.Find(logDataFindTextBox.Text, start, RichTextBoxFinds.None);
			logDataText.Focus();
		}

		private void logDataFindText_KeyPress(object sender, KeyPressEventArgs e)
		{
//			MessageBox.Show("key pressed: 0x"+((int)e.KeyChar).ToString("X4"));
			if (e.KeyChar == '\x000D')
				LogDataFindText();
		}

		private void logDataDisableUpdatesCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (logDataDisableUpdatesCheckBox.Checked)
				logDataText.Clear();
			else
				UpdateLogDataTab();
		}

		private ContextMenu m_logActionsMenu;
		private int m_logDataClickIndex;

		private void logDataText_MouseClickEvent(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				if (m_logActionsMenu == null)
					return;
				if (CurrentLog == null)
					return;

				Point clickPoint = new Point(e.X, e.Y);
				m_logDataClickIndex = logDataText.GetCharIndexFromPosition(clickPoint);
				m_logActionsMenu.Show(logDataText, clickPoint);
			}
		}

		private void LogActionClick_Event(object sender, EventArgs e)
		{
			logDataText.Invalidate();
			if (CurrentLog == null)
				return;
			MenuItem menu = sender as MenuItem;
			if (menu == null) return;
			if (menu.Index > m_logActions.Count)
				return;
			int packetIndex = CurrentLog.GetPacketIndexByTextIndex(m_logDataClickIndex);
			if (packetIndex < 0)
				return;

			try
			{
				ILogAction action = (ILogAction)m_logActions[menu.Index];
				if (action.Activate(CurrentLog, packetIndex))
					UpdateLogDataTab();
			}
			catch (Exception e1)
			{
				Log.Error("activating log action", e1);
			}
		}

		private void packetTimeDiffMenuItem_Click(object sender, System.EventArgs e)
		{
			packetTimeDiffMenuItem.Checked = !packetTimeDiffMenuItem.Checked;
			UpdateLogDataTab();
		}

		#endregion

		#region Log info tab

		private void UpdateLogInfoTab()
		{
			if (CurrentLog != null)
			{
				li_packetsCount.Text = CurrentLog.Count.ToString("N0");
				li_unknownPacketsCount.Text = CurrentLog.UnknownPacketsCount.ToString("N0");
				li_clientVersion.Text = CurrentLog.Version.ToString();
				li_changesLabel.Text = "";
			}
			else
			{
				li_packetsCount.Text = string.Empty;
				li_clientVersion.Text = string.Empty;
				li_unknownPacketsCount.Text = string.Empty;
				li_changesLabel.Text = string.Empty;
			}
		}

		private void li_applyButton_Click(object sender, EventArgs e)
		{
			if (CurrentLog == null)
			{
				Log.Info("Nothing loaded.");
				return;
			}

			CurrentLog.IgnoreVersionChanges = false;
			try
			{
				CurrentLog.Version = int.Parse(li_clientVersion.Text);
			}
			catch
			{
				CurrentLog.Version = -1;
			}
			CurrentLog.IgnoreVersionChanges = li_ignoreVersionChanges.Checked;


			try
			{
				m_progress.SetDescription("Reinitializing log and packets...");
				m_progress.Start(new WorkCallback(InitLog), null);
			}
			finally
			{
				UpdateLogInfoTab();
				UpdateLogDataTab();
			}
		}

		private void InitLog(ProgressCallback callback, object state)
		{
			CurrentLog.Init(3, callback);
		}

		private void li_changes_Event(object sender, EventArgs e)
		{
			li_changesLabel.Text = "(changes)";
		}

		#endregion

		#region Instant parse tab

		private void InstantParseUpdateEvent(object sender, EventArgs e)
		{
			UpdateInstantParseTab();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			instantParseInput.Clear();
		}

		private void UpdateInstantParseTab()
		{
			int ver;
			if (!Util.ParseInt(instantVersion.Text, out ver))
				ver = -1;

			Packet pak = new Packet(0);
			int code;
			if (!Util.ParseInt(instantCode.Text, out code))
				code = 0;
			pak.Code = code;
			bool daocLoggerPacket = false;
			try
			{
				if (instantParseInput.Text[0] == '<')
				{
					try
					{
						string[] lines = instantParseInput.Lines;

						ePacketDirection dir;
						ePacketProtocol prot;
						TimeSpan time;
						int dataLen = DaocLoggerV3TextLogReader.ParseHeader(lines[0], out code, out dir, out prot, out time);
						pak.Code = code;
						pak.Direction = dir;
						pak.Protocol = prot;
						pak.Time = time;

						for (int i = 1; i < lines.Length; i++)
						{
							DaocLoggerV3TextLogReader.ParseDataLine(lines[i], pak);
						}
						daocLoggerPacket = true;

						if (pak.Direction == ePacketDirection.ClientToServer)
						{
							instantClientToServer.Checked = true;
						}
						else
						{
							instantServerToClient.Checked = true;
						}
					} 
					catch(Exception) {}
				}
				// failed to read daoc logger format
				if (!daocLoggerPacket)
				{
					pak.Position = 0;
					foreach (string line in instantParseInput.Text.Split('\n'))
					{
						foreach (string str in line.Split(' '))
						{
							try
							{
								if (str.Trim().Length != 2) continue;
								byte b = byte.Parse(str.Trim(), NumberStyles.HexNumber);
								pak.WriteByte(b);
							}
							catch(Exception)
							{
							}
						}
					}
				}
			}
			catch(Exception)
			{
			}

			if (instantServerToClient.Checked)
				pak.Direction = ePacketDirection.ServerToClient;
			else
				pak.Direction = ePacketDirection.ClientToServer;

			pak = PacketManager.CreatePacket(ver, pak.Code,  pak.Direction).CopyFrom(pak);

			StringBuilder result = new StringBuilder();
			result.AppendFormat("ver:{0}  code:0x{1:X2} (old:0x{2:X2})  dir:{3}  len:{4} (0x{4:X})  logger packet:{5}", ver, pak.Code, pak.Code^168, pak.Direction, pak.Length, daocLoggerPacket);
			result.Append("\npacket class: ").Append(pak.GetType().Name).Append("\n");
			result.AppendFormat("desc: \"{0}\"\n\n", pak.Description);
			try
			{
				pak.InitException = null;
				pak.Initialized = false;
				pak.Position = 0;
				pak.Init();
				pak.PositionAfterInit = pak.Position;
				pak.Initialized = true;
				result.Append(pak.GetPacketDataString());
			}
			catch (OutOfMemoryException e)
			{
				pak.Initialized = false;
				pak.InitException = e;
				result.Append(e.ToString());
			}
			catch (Exception e)
			{
				pak.InitException = e;
				result.AppendFormat("{0}: {1}", e.GetType().ToString(), e.Message);
//				result.Append("\n\n").Append(e.ToString());
			}

			if (pak.PositionAfterInit > pak.Length)
			{
				result.AppendFormat("\n(pak.PositionAfterInit > pak.Length !)");
			}

			string notInitialized = pak.GetNotInitializedData();
			if (notInitialized.Length > 0)
			{
				result.AppendFormat("\n\nnot initialized:\n{0}", notInitialized);
			}

			result.Append('\n');

			instantParseOut.Text = result.ToString();

			if (daocLoggerPacket)
			{
				instantCode.Text = "0x"+pak.Code.ToString("X2");
				instantClientToServer.Checked = pak.Direction == ePacketDirection.ClientToServer;
			}
		}

		#endregion

		#region Filters

		private MenuItem m_combineFiltersMenuItem;
		private MenuItem m_invertCheckMenuItem;

		private void FilterClick_Event(object sender, EventArgs e)
		{
			MenuItem menu = sender as MenuItem;
			if (menu == null) return;
			if (menu == m_combineFiltersMenuItem)
			{
				m_combineFiltersMenuItem.Checked = (FilterManager.CombineFilters = !FilterManager.CombineFilters);
				UpdateLogDataTab();
				return;
			}
			
			if (menu == m_invertCheckMenuItem)
			{
				m_invertCheckMenuItem.Checked = (FilterManager.InvertCheck = !FilterManager.InvertCheck);
				UpdateLogDataTab();
				return;
			}

			int index = menu.Index - 3;
			if (index >= m_logFilters.Count || index < 0)
				return;

			bool update = false;
			int oldFilters = FilterManager.FiltersCount;

			try
			{
				ILogFilter filter = (ILogFilter)m_logFilters[index];
				update |= filter.ActivateFilter(); // changes to the filter

				update |= oldFilters != FilterManager.FiltersCount;

				if (update)
				{
					UpdateLogDataTab();
				}
				menu.Checked = filter.IsFilterActive;
			}
			catch (Exception e1)
			{
				Log.Error("activating filter", e1);
			}
		}

		#endregion

	}
}
