using System;
using System.Collections;
using System.Collections.Generic;
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
	public class MainForm : Form, IExecutionContext
	{
		private MainMenu mainMenu1;
		private TabPage logInfoTab;
		private RichTextBox logDataText;
		private MenuItem menuOpenFile;
		private MenuItem menuSaveFile;
		private OpenFileDialog openLogDialog;
		private IContainer components;

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
		private System.Windows.Forms.MenuItem menuOpenFolder;
		private System.Windows.Forms.FolderBrowserDialog openFolderLogDialog;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem packetTimeDiffMenuItem;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuRecentFiles;
		private System.Windows.Forms.MenuItem menuExitApp;
		private System.Windows.Forms.MenuItem mnuPacketFlags;
		private System.Windows.Forms.MenuItem mnuPacketSequence;
		private OpenFileDialog openFilterDialog;
		private SaveFileDialog saveFilterDialog;
		private Label label3;

		public MainForm()
		{
			m_progress = new Progress(this);
			InitializeComponent();

			m_currentLogs.OnPacketLogsChanged += OnPacketLogsChanged;
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
			this.components = new System.ComponentModel.Container();
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuOpenFile = new System.Windows.Forms.MenuItem();
			this.menuOpenAnother = new System.Windows.Forms.MenuItem();
			this.menuOpenFolder = new System.Windows.Forms.MenuItem();
			this.menuSaveFile = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuRecentFiles = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuExitApp = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.packetTimeDiffMenuItem = new System.Windows.Forms.MenuItem();
			this.mnuPacketSequence = new System.Windows.Forms.MenuItem();
			this.mnuPacketFlags = new System.Windows.Forms.MenuItem();
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
			this.openFolderLogDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.openFilterDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFilterDialog = new System.Windows.Forms.SaveFileDialog();
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
            this.menuOpenFolder,
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
			// menuOpenFolder
			//
			this.menuOpenFolder.Index = 2;
			this.menuOpenFolder.Text = "Open folder ...";
			this.menuOpenFolder.Click += new System.EventHandler(this.menuOpenFolder_Click);
			//
			// menuSaveFile
			//
			this.menuSaveFile.Enabled = false;
			this.menuSaveFile.Index = 3;
			this.menuSaveFile.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.menuSaveFile.Text = "&Save...";
			this.menuSaveFile.Click += new System.EventHandler(this.menuSaveFile_Click);
			//
			// menuItem3
			//
			this.menuItem3.Index = 4;
			this.menuItem3.Text = "-";
			//
			// menuRecentFiles
			//
			this.menuRecentFiles.Index = 5;
			this.menuRecentFiles.Text = "&Recent Files";
			//
			// menuItem5
			//
			this.menuItem5.Index = 6;
			this.menuItem5.Text = "-";
			//
			// menuExitApp
			//
			this.menuExitApp.Index = 7;
			this.menuExitApp.Text = "E&xit";
			this.menuExitApp.Click += new System.EventHandler(this.menuExitApp_Click);
			//
			// menuItem2
			//
			this.menuItem2.Index = 1;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.packetTimeDiffMenuItem,
            this.mnuPacketSequence,
            this.mnuPacketFlags});
			this.menuItem2.Text = "&View";
			//
			// packetTimeDiffMenuItem
			//
			this.packetTimeDiffMenuItem.Index = 0;
			this.packetTimeDiffMenuItem.Text = "Packet time difference";
			this.packetTimeDiffMenuItem.Click += new System.EventHandler(this.packetTimeDiffMenuItem_Click);
			//
			// mnuPacketSequence
			//
			this.mnuPacketSequence.Index = 1;
			this.mnuPacketSequence.Text = "Packet sequence ";
			this.mnuPacketSequence.Click += new System.EventHandler(this.mnuPacketSequence_Click);
			//
			// mnuPacketFlags
			//
			this.mnuPacketFlags.Index = 2;
			this.mnuPacketFlags.Text = "Packet flags";
			this.mnuPacketFlags.Click += new System.EventHandler(this.mnuPacketFlags_Click);
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
			this.instantParseOut.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
			this.instantParseInput.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.instantParseInput.Location = new System.Drawing.Point(8, 48);
			this.instantParseInput.Multiline = true;
			this.instantParseInput.Name = "instantParseInput";
			this.instantParseInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.instantParseInput.Size = new System.Drawing.Size(568, 112);
			this.instantParseInput.TabIndex = 7;
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
			this.li_unknownPacketsCount.Size = new System.Drawing.Size(100, 20);
			this.li_unknownPacketsCount.TabIndex = 7;
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
			this.li_changesLabel.Size = new System.Drawing.Size(100, 23);
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
			this.li_clientVersion.Size = new System.Drawing.Size(100, 20);
			this.li_clientVersion.TabIndex = 1;
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
			this.li_packetsCount.Size = new System.Drawing.Size(100, 20);
			this.li_packetsCount.TabIndex = 1;
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
			this.logDataDisableUpdatesCheckBox.Size = new System.Drawing.Size(104, 24);
			this.logDataDisableUpdatesCheckBox.TabIndex = 3;
			this.logDataDisableUpdatesCheckBox.Text = "disable updates";
			this.logDataDisableUpdatesCheckBox.CheckedChanged += new System.EventHandler(this.logDataDisableUpdatesCheckBox_CheckedChanged);
			//
			// logDataFindButton
			//
			this.logDataFindButton.Location = new System.Drawing.Point(8, 40);
			this.logDataFindButton.Name = "logDataFindButton";
			this.logDataFindButton.Size = new System.Drawing.Size(75, 23);
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
			this.logDataFindTextBox.WordWrap = false;
			this.logDataFindTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.logDataFindText_KeyPress);
			//
			// logDataText
			//
			this.logDataText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.logDataText.DetectUrls = false;
			this.logDataText.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
			// openFilterDialog
			//
			this.openFilterDialog.Filter = "Filters (*.flt)|*.flt";
			this.openFilterDialog.RestoreDirectory = true;
			//
			// saveFilterDialog
			//
			this.saveFilterDialog.Filter = "Filters (*.flt)|*.flt";
			this.saveFilterDialog.RestoreDirectory = true;
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
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.mainFormTabs.ResumeLayout(false);
			this.instantParseTab.ResumeLayout(false);
			this.instantResultGroupBox1.ResumeLayout(false);
			this.inputDataGroupBox.ResumeLayout(false);
			this.inputDataGroupBox.PerformLayout();
			this.logInfoTab.ResumeLayout(false);
			this.logInfoTab.PerformLayout();
			this.logDataTab.ResumeLayout(false);
			this.logDataTab.PerformLayout();
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

		private readonly LogManager m_currentLogs = new LogManager();

		/// <summary>
		/// Gets or sets the current log.
		/// </summary>
		/// <value>The current log.</value>
		public LogManager LogManager
		{
			get { return m_currentLogs; }
		}

		/// <summary>
		/// Called when packet logs change - updates UI.
		/// </summary>
		/// <param name="logManager">The log manager.</param>
		private void OnPacketLogsChanged(LogManager logManager)
		{
			UpdateLogDataTab();
			UpdateLogInfoTab();
			UpdateCaption();
			menuSaveFile.Enabled = (m_currentLogs.Logs.Count > 0);
			GC.Collect();
		}

		private ArrayList m_logReaders = new ArrayList();
		private ArrayList m_logWriters = new ArrayList();
		private ArrayList m_logFilters = new ArrayList();
		private ArrayList m_logActions = new ArrayList();
		private SortedList m_filterMenuItemsByPriority = new SortedList();
		private SortedList m_actionMenuItemsByPriority = new SortedList();

		/// <summary>
		/// Handles the Load event of the MainForm control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void MainForm_Load(object sender, EventArgs e)
		{
			// Create temp data storage
			SortedList readers = new SortedList();
			SortedList writers = new SortedList();
			SortedList filters = new SortedList();
			SortedList actions = new SortedList();
			Hashtable readerFilterStrings = new Hashtable();
			Hashtable writerFilterStrings = new Hashtable();
			m_filterMenuItemsByPriority.Clear();
			m_actionMenuItemsByPriority.Clear();

			// Find handlers
			FindAllHandlers(m_actionMenuItemsByPriority, actions, m_filterMenuItemsByPriority, filters, readerFilterStrings, readers, writerFilterStrings, writers);


			//
			// Initialize UI with found handlers
			//

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
        	openFolderLogDialog.Description = "Select the directory that you want to use as Open logs";
        	openFolderLogDialog.ShowNewFolderButton = false;

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

			// Filters menu
			if (filters.Count > 0)
			{
				CreateMenuFilters(m_filterMenuItemsByPriority, filters);

				// Add event handlers
				FilterManager.FilterAddedEvent		+= new FilterAction(OnFilterAdded);
				FilterManager.FilterRemovedEvent	+= new FilterAction(OnFilterRemoved);
			}

			// Actions menu
			if (actions.Count > 0)
			{
				CreateMenuActions(m_actionMenuItemsByPriority, actions);
			}

			// Updates
			UpdateRecentFilesMenu();
			UpdateCaption();

			// Settings
			LoadSettings();
		}

		/// <summary>
		/// Handles the FormClosing event of the MainForm control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			// Settings
			SaveSettings();
		}

		/// <summary>
		/// Finds all handlers.
		/// </summary>
		/// <param name="actionMenuItems">The action menu items.</param>
		/// <param name="actions">The action names.</param>
		/// <param name="filterMenuItems">The filter menu items.</param>
		/// <param name="filters">The filter names.</param>
		/// <param name="readerFilterStrings">The reader filter strings.</param>
		/// <param name="readers">The reader names.</param>
		/// <param name="writerFilterStrings">The writer filter strings.</param>
		/// <param name="writers">The writer names.</param>
		private void FindAllHandlers(IDictionary actionMenuItems, SortedList actions, IDictionary filterMenuItems, SortedList filters, Hashtable readerFilterStrings, SortedList readers, Hashtable writerFilterStrings, SortedList writers)
		{
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
		}

		/// <summary>
		/// Creates the actions menu.
		/// </summary>
		/// <param name="actionMenuItems">The action menu items.</param>
		/// <param name="actions">The actions.</param>
		private void CreateMenuActions(IDictionary actionMenuItems, SortedList actions)
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

		/// <summary>
		/// Creates the filters menu.
		/// </summary>
		/// <param name="filterMenuItems">The filter menu items.</param>
		/// <param name="filters">The filters.</param>
		private void CreateMenuFilters(IDictionary filterMenuItems, SortedList filters)
		{
			ArrayList menu = new ArrayList();

			// "Load filters" option
			m_filterMenuFiltersLoad = new MenuItem("&Load filters...");
			m_filterMenuFiltersLoad.Click += delegate(object sender, EventArgs e)
     				{
						if (DialogResult.OK == openFilterDialog.ShowDialog(this))
						{
							saveFilterDialog.FileName = openFilterDialog.FileName;
							FilterManager.LoadFilters(openFilterDialog.FileName, m_logFilters);
							if (!FilterManager.IgnoreFilters)// try fix update filters while ignore filter is ON
								UpdateLogDataTab();
						}
     				};
			menu.Add(m_filterMenuFiltersLoad);

			// "Save filters" option
			m_filterMenuFiltersSave = new MenuItem("&Save filters...");
			m_filterMenuFiltersSave.Click += delegate(object sender, EventArgs e)
                 	{
						// "Save" menu item
						if (DialogResult.OK == saveFilterDialog.ShowDialog(this))
						{
							openFilterDialog.FileName = saveFilterDialog.FileName;
							FilterManager.SaveFilters(saveFilterDialog.FileName);
						}
					};
			menu.Add(m_filterMenuFiltersSave);

			menu.Add(new MenuItem("-"));

			// "Combine" option
			m_filterMenuCombineFilters = new MenuItem("Combine filters", new EventHandler(FilterClick_Event));
			m_filterMenuCombineFilters.Checked = FilterManager.CombineFilters;
			FilterManager.CombineFiltersChangedEvent += delegate(bool newValue)
				   	{
				   		m_filterMenuCombineFilters.Checked = newValue;
						if (!FilterManager.IgnoreFilters)// try fix update filters while ignore filter is ON
							UpdateLogDataTab();
				   	};
			menu.Add(m_filterMenuCombineFilters);

			// "Invert" option
			m_filterMenuInvertCheck = new MenuItem("Invert check", new EventHandler(FilterClick_Event));
			m_filterMenuInvertCheck.Checked = false;
			FilterManager.InvertCheckChangedEvent += delegate(bool newValue)
				   	{
				   		m_filterMenuInvertCheck.Checked = newValue;
						if (!FilterManager.IgnoreFilters)// try fix update filters while ignore filter is ON
							UpdateLogDataTab();
					};
			menu.Add(m_filterMenuInvertCheck);

			// "Ignore" option
			m_filterMenuIgnoreFilters = new MenuItem("Ignore filters", new EventHandler(FilterClick_Event));
			m_filterMenuIgnoreFilters.Checked = FilterManager.IgnoreFilters;
			m_filterMenuIgnoreFilters.ShowShortcut = true;
			m_filterMenuIgnoreFilters.Shortcut = System.Windows.Forms.Shortcut.AltBksp;
			FilterManager.IgnoreFiltersChangedEvent += delegate(bool newValue)
				   	{
				   		m_filterMenuIgnoreFilters.Checked = newValue;
						UpdateLogDataTab();
				   	};
			menu.Add(m_filterMenuIgnoreFilters);


			menu.Add(new MenuItem("-"));

			// Save count of static elements for proper index calculation
			m_filterMenuStaticElementsCount = menu.Count;

			// Add dynamically loaded handlers in proper order
			foreach (DictionaryEntry entry in filters)
			{
				int position = (int)entry.Key;
				ILogFilter filter = (ILogFilter)entry.Value;
				menu.Add(filterMenuItems[position]);
				m_logFilters.Add(filter);
			}

			mainMenu1.MenuItems.Add("F&ilters", (MenuItem[])menu.ToArray(typeof (MenuItem)));
		}

		/// <summary>
		/// Updates the caption.
		/// </summary>
		private void UpdateCaption()
		{
			string caption = "packet log converter v" + Assembly.GetExecutingAssembly().GetName().Version;
			string streamNames = LogManager.GetStreamNames();
			if (!string.IsNullOrEmpty(streamNames))
			{
				caption += ": " + streamNames;
			}
			Text = caption;
		}

		#region open files

		private void LoadFiles(ILogReader reader, ICollection<PacketLog> logs, string[] files, ProgressCallback progress)
		{
			foreach (string fileName in files)
			{
				try
				{
					// Create new log for each file
					PacketLog log = new PacketLog();

					m_progress.SetDescription("Loading file: " + fileName + "...");

					// Check if file exists
					FileInfo fileInfo = new FileInfo(fileName);
					if (!fileInfo.Exists)
					{
						Log.Info("File \"" + fileInfo.FullName + "\" doesn't exist, ignored.");
						continue;
					}

					// Add all packets
					using(FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						log.AddRange(reader.ReadLog(new BufferedStream(stream, 64*1024), progress));
					}

					// Initialize log
#warning TODO: Unlink log poperties from log manager and make a list of all logs and their properties in log info tab
					m_progress.SetDescription("Initializing log and packets...");
					log.Init(LogManager, 3, progress);

					// Set stream name
					log.StreamName = fileInfo.FullName;

					AddRecentFile(fileInfo.FullName);
					logs.Add(log);
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
			public IList<PacketLog> Logs;
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
					LogManager.ClearLogs();
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
			data.Logs = new List<PacketLog>();

			LogManager.IgnoreVersionChanges = false;
			float version;
			Util.ParseFloat(li_clientVersion.Text, out version, -1);

			LogManager.Version = version;

			if (li_ignoreVersionChanges.Checked)
			{
				LogManager.IgnoreVersionChanges = true;
			}

			LoadFiles(data.Reader, data.Logs, data.Files, progress);
		}

		private void OpenFileFinishedCallback(object state)
		{
			OpenData data = (OpenData)state;

			// Count packets
			LogManager tempManager = new LogManager();
			tempManager.AddLogRange(LogManager.Logs);
			tempManager.AddLogRange(data.Logs);

			if (tempManager.CountPackets() > 100000)
			{
				logDataDisableUpdatesCheckBox.Checked = true;
				mainFormTabs.SelectedTab = logInfoTab;
			}
			else
			{
				mainFormTabs.SelectedTab = logDataTab;
			}
			Refresh();

			// Add loadedlogs to current list
			LogManager.AddLogRange(data.Logs);

			// Update current version
			li_clientVersion.Text = LogManager.Version.ToString();
			logDataText.Focus();

			LogReaderDelegate e = FilesLoaded;
			if (e != null)
				e(data.Reader);
		}

		private void menuOpenAnother_Click(object sender, System.EventArgs e)
		{
			if (m_logReaders.Count > 0)
			{
				openAnotherLogDialog.FileName = openLogDialog.FileName;
				if (openAnotherLogDialog.ShowDialog() == DialogResult.OK)
				{
					try
					{
						// Disable log text updates if log is too long
						if (LogManager.CountPackets() > 100000)
						{
							logDataDisableUpdatesCheckBox.Checked = true;
						}
						OpenData data = new OpenData();
						data.Files = openAnotherLogDialog.FileNames;
						data.Reader = (ILogReader) m_logReaders[openAnotherLogDialog.FilterIndex - 1];
//						LogManager = null;
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

		private static ArrayList ParseDirectory(DirectoryInfo path, string filter, bool deep)
		{
			ArrayList files = new ArrayList();
    		if (!path.Exists)
				return files;
    		files.AddRange(path.GetFiles(filter));
			if (deep)
			{
				foreach (DirectoryInfo subdir in path.GetDirectories())
					files.AddRange(ParseDirectory(subdir, filter, deep));
			}
			return files;
		}

		private void menuOpenFolder_Click(object sender, System.EventArgs e)
		{
			if (m_logReaders.Count > 0)
			{
				// Show the FolderBrowserDialog.
				openFolderLogDialog.SelectedPath = openLogDialog.InitialDirectory;
				if ( openFolderLogDialog.ShowDialog() == DialogResult.OK )
				{
					openLogDialog.InitialDirectory = openFolderLogDialog.SelectedPath;
					// Start loads logs
					ArrayList filesInDir = ParseDirectory(new DirectoryInfo(openFolderLogDialog.SelectedPath), "*.log", true);
					if (filesInDir.Count > 0)
					{
						try
						{
							int i = 0;
							OpenData data = new OpenData();
//							data.Files = (string[])ParseDirectory(new DirectoryInfo(openFolderLogDialog.SelectedPath), "*.log", true).ToArray(typeof(string));
							data.Files = new string[filesInDir.Count];
							foreach(FileInfo s in filesInDir)
								data.Files[i++] = s.FullName;
							data.Reader = (ILogReader) m_logReaders[openLogDialog.FilterIndex - 1];
							LogManager.ClearLogs();
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
					filesInDir = null;
				}
			}
			else
				Log.Info("No log readers found.");
		}

		#endregion

		/// <summary>
		/// Handles the Click event of the menuSaveFile control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void menuSaveFile_Click(object sender, EventArgs e)
		{
			if (LogManager == null)
			{
				Log.Info("Nothing to save.");
				return;
			}

			if (m_logWriters.Count > 0)
				saveLogDialog.ShowDialog();
			else
				Log.Info("No log writers found.");
		}

		/// <summary>
		/// Handles the FileOk event of the saveLogDialog control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
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

		/// <summary>
		/// Saves the file proc.
		/// </summary>
		/// <param name="callback">The callback.</param>
		/// <param name="state">The state.</param>
		private void SaveFileProc(ProgressCallback callback, object state)
		{
			// Notify filter manager
			FilterManager.LogFilteringStarted(this);

			try
			{
				ILogWriter writer = (ILogWriter)m_logWriters[saveLogDialog.FilterIndex - 1];
				using (FileStream stream = new FileStream(saveLogDialog.FileName, FileMode.Create))
				{
					writer.WriteLog(this, stream, callback);
				}
			}
			finally
			{
				// Notify filter manager
				FilterManager.LogFilteringStopped(this);
			}
		}

		/// <summary>
		/// Handles the Click event of the menuExitApp control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void menuExitApp_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		#endregion

		#region Registry

		const string RegKeysPath = @"Software\DawnOfLight\PacketLogConverter\";
		const string FilterFolder = "FilterFolder";
		const string LogsFolder = "LogsFolder";

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

		/// <summary>
		/// Saves the settings to registry.
		/// </summary>
		private void SaveSettings()
		{
			try
			{
				RegistryKey key = Registry.CurrentUser.CreateSubKey(RegKeysPath);

				if (key != null)
				{
					using (key)
					{
						// Filter folder
						SaveFileDialogFolder(key, FilterFolder, openFilterDialog);

						// Logs folder
						SaveFileDialogFolder(key, LogsFolder, openLogDialog);
					}
				}
			}
			catch (Exception e)
			{
				Log.Error(e.Message, e);
			}
		}

		/// <summary>
		/// Saves the file dialog folder.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="regKeyName">Name of the reg key.</param>
		/// <param name="dialog">The dialog.</param>
		private void SaveFileDialogFolder(RegistryKey key, string regKeyName, FileDialog dialog)
		{
			string fileName = dialog.FileName;
			if (fileName != null && fileName.Length > 0)
			{
				FileInfo fileInfo = new FileInfo(fileName);
				key.SetValue(regKeyName, fileInfo.DirectoryName);
			}
		}

		/// <summary>
		/// Loads the settings from registry.
		/// </summary>
		private void LoadSettings()
		{
			try
			{
				RegistryKey key = Registry.CurrentUser.CreateSubKey(RegKeysPath);

				if (key != null)
				{
					using (key)
					{
						// Filters folder
						string filterFolder = key.GetValue(FilterFolder) as string;
						LoadFolderDialog(filterFolder, openFilterDialog);
						LoadFolderDialog(filterFolder, saveFilterDialog);

						// Logs folder
						string logsFolder = key.GetValue(LogsFolder) as string;
						LoadFolderDialog(logsFolder, openLogDialog);
						LoadFolderDialog(logsFolder, saveLogDialog);
						LoadFolderDialog(logsFolder, openAnotherLogDialog);
					}
				}
			}
			catch (Exception e)
			{
				Log.Error(e.Message, e);
			}
		}

		/// <summary>
		/// Loads the folder dialog.
		/// </summary>
		/// <param name="dialog">The dialog.</param>
		/// <param name="folder">The folder.</param>
		private void LoadFolderDialog(string folder, FileDialog dialog)
		{
			if (folder != null && folder.Length > 0)
			{
				dialog.InitialDirectory = folder;
			}
		}

		private void RecentFileMenuItem_Click(object sender, EventArgs e)
		{
//			if (m_logReaders.Count <= 0)
//				return;

			MenuItem item = (MenuItem) sender;

			LogManager.ClearLogs();
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

				if (LogManager == null)
				{
					logDataText.Clear();
					return;
				}

//				logDataText.Clear();
				PacketLocation selectedPacket = PacketLocation.UNKNOWN;
				if (logDataText.TextLength > 0)
				{
					selectedPacket = LogManager.GetPacketIndexByTextIndex(logDataText.SelectionStart);
				}
				int packetsCount = 0;
				int packetsCountInTCP = 0;
				int packetsCountOutTCP = 0;
				int packetsCountInUDP = 0;
				int packetsCountOutUDP = 0;
				bool timeDiff = packetTimeDiffMenuItem.Checked;
				bool showPacketSequence = mnuPacketSequence.Checked;
				TimeSpan baseTime = new TimeSpan(0);

				// Notify filter manager that log filtering starts
				FilterManager.LogFilteringStarted(this);

				StringBuilder text = new StringBuilder();
				IList<PacketLog> logs = LogManager.Logs;
				foreach (PacketLog log in logs)
				{
					foreach (Packet pak in log)
					{
						int pakIndex = 0;
						if (showPacketSequence)
						{
							if (pak.Protocol == ePacketProtocol.TCP)
							{
								if (pak.Direction == ePacketDirection.ClientToServer)
								{
									pakIndex = ++packetsCountOutTCP;
								}
								else
								{
									pakIndex = ++packetsCountInTCP;
								}
							}
							else if (pak.Protocol == ePacketProtocol.UDP)
							{
								if (pak.Direction == ePacketDirection.ClientToServer)
								{
									pakIndex = ++packetsCountOutUDP;
								}
								else
								{
									pakIndex = ++packetsCountInUDP;
								}
							}
						}
						if (FilterManager.IsPacketIgnored(pak))
						{
							pak.LogTextIndex = -1;
							continue;
						}
						pak.LogTextIndex = text.Length;
						++packetsCount;

						if (showPacketSequence)
						{
							text.AppendFormat("{0}:{1,-5} ", pak.Protocol, pakIndex);
						}

						// main description
						text.Append(pak.ToHumanReadableString(baseTime, mnuPacketFlags.Checked));
						text.Append('\n');
						if (timeDiff)
							baseTime = pak.Time;
					}
				}

				newTabName = string.Format("{0} ({1}{2:N0} packets)", newTabName, (timeDiff ? "time diff, " : ""), packetsCount);
				logDataTab.Text = newTabName;

				logDataText.SelectionIndent = 4;
				logDataText.Text = text.ToString();

				// Restore previously selected packet if it is visible
				int restoreIndex = -1;
				if (PacketLocation.UNKNOWN != selectedPacket && logs.Count > selectedPacket.LogIndex
					&& logs[selectedPacket.LogIndex].Count > selectedPacket.PacketIndex)
				{
					restoreIndex = logs[selectedPacket.LogIndex][selectedPacket.PacketIndex].LogTextIndex;
				}
				if (restoreIndex >= 0)
				{
					logDataText.SelectionStart = restoreIndex;
				}
			}
			catch (Exception e)
			{
				Log.Error("updating data tab", e);
			}
			finally
			{
				// Notify filter manager that filtering is finished
				FilterManager.LogFilteringStopped(this);
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
				if (LogManager == null)
					return;

				Point clickPoint = new Point(e.X, e.Y);
				m_logDataClickIndex = logDataText.GetCharIndexFromPosition(clickPoint);
				m_logActionsMenu.Show(logDataText, clickPoint);
			}
		}

		private void LogActionClick_Event(object sender, EventArgs e)
		{
			logDataText.Invalidate();
			if (LogManager == null)
				return;
			MenuItem menu = sender as MenuItem;
			if (menu == null) return;
			if (menu.Index > m_logActions.Count)
				return;

			// Find log and packet indices
			PacketLocation packetLocation = LogManager.GetPacketIndexByTextIndex(m_logDataClickIndex);
			if (PacketLocation.UNKNOWN == packetLocation)
				return;

			try
			{
				ILogAction action = (ILogAction)m_logActions[menu.Index];
				if (action.Activate(this, packetLocation))
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

		private void mnuPacketFlags_Click(object sender, System.EventArgs e)
		{
			mnuPacketFlags.Checked = !mnuPacketFlags.Checked;
			UpdateLogDataTab();
		}

		private void mnuPacketSequence_Click(object sender, System.EventArgs e)
		{
			mnuPacketSequence.Checked = !mnuPacketSequence.Checked;
			UpdateLogDataTab();
		}

		#endregion

		#region Log info tab

		private void UpdateLogInfoTab()
		{
			if (LogManager != null)
			{
				li_packetsCount.Text = LogManager.CountPackets().ToString("N0");
				li_unknownPacketsCount.Text = LogManager.CountUnknownPackets().ToString("N0");
				li_clientVersion.Text = LogManager.Version.ToString();
				li_changesLabel.Text = "";
			}
			else
			{
				li_packetsCount.Text = string.Empty;
//				li_clientVersion.Text = string.Empty; // leave unchanged
				li_unknownPacketsCount.Text = string.Empty;
				li_changesLabel.Text = string.Empty;
			}
		}

		private void li_applyButton_Click(object sender, EventArgs e)
		{
			if (LogManager == null)
			{
				Log.Info("Nothing loaded.");
				return;
			}

			LogManager.IgnoreVersionChanges = false;
			float version;
			Util.ParseFloat(li_clientVersion.Text, out version, -1);
			LogManager.Version = version;
			LogManager.IgnoreVersionChanges = li_ignoreVersionChanges.Checked;

			// Reinitialize in another thread
			m_progress.SetDescription("Reinitializing log and packets...");
			m_progress.Start(new WorkCallback(InitLog), null);
		}

		private void InitLog(ProgressCallback callback, object state)
		{
			LogManager.InitLogs(3, callback);

			// Update log info
			Invoke((MethodInvoker)delegate()
			       	{
			       		UpdateLogInfoTab();
			       		UpdateLogDataTab();
			       	});
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
			float ver;
			Util.ParseFloat(instantVersion.Text, out ver, -1);

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
				result.Append(pak.GetPacketDataString(true));
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

		private MenuItem m_filterMenuCombineFilters;
		private MenuItem m_filterMenuInvertCheck;
		private MenuItem m_filterMenuIgnoreFilters;
		private MenuItem m_filterMenuFiltersLoad;
		private MenuItem m_filterMenuFiltersSave;
		private int m_filterMenuStaticElementsCount;

		/// <summary>
		/// Handles the Event event of the Filter menu.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		private void FilterClick_Event(object sender, EventArgs e)
		{
			MenuItem menu = sender as MenuItem;
			if (menu == null) return;

			try
			{
				//
				// Static elements handlers
				//

				// "Combine" menu item
				if (menu == m_filterMenuCombineFilters)
				{
					FilterManager.CombineFilters = !FilterManager.CombineFilters;
					return;
				}

				// "Invert" menu item
				if (menu == m_filterMenuInvertCheck)
				{
					FilterManager.InvertCheck = !FilterManager.InvertCheck;
					return;
				}

				// "Ignore" menu item
				if (menu == m_filterMenuIgnoreFilters)
				{
					FilterManager.IgnoreFilters = !FilterManager.IgnoreFilters;
					return;
				}


				//
				// Dynamic elements handlers
				//

				int index = menu.Index - m_filterMenuStaticElementsCount;
				if (index >= m_logFilters.Count || index < 0)
					return;

				bool update = false;
				int oldFilters = FilterManager.FiltersCount;

				ILogFilter filter = (ILogFilter)m_logFilters[index];
				update |= filter.ActivateFilter(); // changes to the filter

				update |= oldFilters != FilterManager.FiltersCount;

				if (update && !FilterManager.IgnoreFilters)// try fix update filters while ignore filter is ON
				{
					UpdateLogDataTab();
				}
			}
			catch (Exception e1)
			{
				Log.Error("activating filter", e1);
			}
		}

		/// <summary>
		/// Called when filter is added to filter manager.
		/// </summary>
		/// <param name="filter">The filter.</param>
		private void OnFilterAdded(ILogFilter filter)
		{
			ChangeFilterMenuCheckedState(filter, true);
		}

		/// <summary>
		/// Called when filter is removed from filter manager.
		/// </summary>
		/// <param name="filter">The filter.</param>
		private void OnFilterRemoved(ILogFilter filter)
		{
			ChangeFilterMenuCheckedState(filter, false);
		}

		/// <summary>
		/// Changes the state of the filter menu checked.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <param name="newState">if set to <c>true</c> menu item is checked.</param>
		private void ChangeFilterMenuCheckedState(ILogFilter filter, bool newState)
		{
			// Get index of filter
			int index = m_logFilters.IndexOf(filter);

			if (index >= m_logFilters.Count || index < 0)
				return;

			// Get menu item, make it checked
			MenuItem menu = (MenuItem) m_filterMenuItemsByPriority.GetByIndex(index);
			if (null != menu)
			{
				menu.Checked = newState;
			}
		}

		#endregion
	}
}
