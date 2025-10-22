using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using CCWin;
using CCWin.SkinControl;
using MessageStruct;
using NationalInstruments.TestStand.Interop.API;
using NationalInstruments.TestStand.Interop.UI;
using NationalInstruments.TestStand.Interop.UI.Ax;
using NationalInstruments.TestStand.Interop.UI.Support;
using NationalInstruments.TestStand.Utility;

namespace FTP2017
{
	public class FormMain : CCSkinMain
	{
		private const UIMessageCodes UIMsg_PostResultList = (UIMessageCodes)10001;

		private const UIMessageCodes UIMsg_ChangeSequence = (UIMessageCodes)10002;

		private const UIMessageCodes UIMsg_PreGetStationInfo = (UIMessageCodes)10003;

		private const UIMessageCodes UIMsg_PostGetStationInfo = (UIMessageCodes)10004;

		private const UIMessageCodes UIMsg_SetSerialNumber = (UIMessageCodes)10005;

		private const UIMessageCodes UIMsg_GetUICommand = (UIMessageCodes)10006;

		private const UIMessageCodes UIMsg_GetLatestSequenceContext = (UIMessageCodes)10007;

		private const UIMessageCodes UIMsg_DisplayTextOnUI = (UIMessageCodes)10008;

		private const UIMessageCodes UIMsg_ChangeLogFileFolder = (UIMessageCodes)10009;

		private const UIMessageCodes UIMsg_ChangeProjectName = (UIMessageCodes)10010;

		private const UIMessageCodes UIMsg_DisableStartButton = (UIMessageCodes)10011;

		private const UIMessageCodes UIMsg_PrePreUUT = (UIMessageCodes)10012;

		private const int WM_QUERYENDSESSION = 17;

		private IContainer components = null;

		private AxSequenceFileViewMgr axSequenceFileViewMgr;

		private AxExecutionViewMgr axExecutionViewMgr;

		private AxApplicationMgr axApplicationMgr;

		private System.Windows.Forms.Label label_DUTResult;

		private PictureBox pictureBox_Logo;

		private System.Windows.Forms.Label label_Total;

		private System.Windows.Forms.Label label_Fail;

		private System.Windows.Forms.Label label9;

		private System.Windows.Forms.Label label8;

		private System.Windows.Forms.Label label7;

		private System.Windows.Forms.Label label_Pass;

		private System.Windows.Forms.Timer timer_Main;

		private TextBox textBox_SerialNumber;

		private SkinLabel skinLabel4;

		private SkinLabel skinLabel3;

		private SkinLabel skinLabel2;

		private SkinLabel skinLabel1;

		private SkinWaterTextBox TextBox_Station;

		private SkinWaterTextBox TextBox_Version;

		private SkinWaterTextBox TextBox_Sequence;

		private SkinWaterTextBox TextBox_Project;

		private SkinLabel skinLabel5;

		private TableLayoutPanel tableLayoutPanel1;

		private Panel panel1;

		private Panel panel2;

		private Panel panel3;

		private Panel panel_Exe;

		private MenuStrip menuStrip1;

		private ToolStripMenuItem fileToolStripMenuItem;

		private ToolStripMenuItem selectSequenceToolStripMenuItem;

		private ToolStripMenuItem exitToolStripMenuItem;

		private ToolStripMenuItem debugToolsToolStripMenuItem;

		private ToolStripMenuItem helpToolStripMenuItem;

		private ToolStripMenuItem userToolStripMenuItem;

		private ToolStripMenuItem loginToolStripMenuItem;

		private ToolStripMenuItem logoutToolStripMenuItem;

		private ToolStripMenuItem closeSequenceToolStripMenuItem;

		private Panel panel6;

		private ToolStripMenuItem configToolStripMenuItem;

		private ToolStripMenuItem reTestDUTToolStripMenuItem;

		private TableLayoutPanel tableLayoutPanel_DUTList;

		private ImageList imageList_BackImage;

		private System.Windows.Forms.Label label_PassRate;

		private System.Windows.Forms.Label label1;

		private ToolStripMenuItem mESToolStripMenuItem;

		private System.Windows.Forms.Label label_MES;

		private System.Windows.Forms.Label label2;

		private ToolStripMenuItem languageToolStripMenuItem;

		private ToolStripMenuItem chineseToolStripMenuItem;

		private ToolStripMenuItem englishToolStripMenuItem;

		private ToolStripMenuItem vietNameToolStripMenuItem;

		private ToolStripMenuItem stationOptionToolStripMenuItem;

		private ToolStripMenuItem instrumentsSourceToolStripMenuItem;

		private System.Windows.Forms.Label label_Time;

		private System.Windows.Forms.Label label_Date;

		private string SerialNumber = "";

		private List<SequenceSetting> SeqSettingList = new List<SequenceSetting>(32);

		private int TotalStepCount = 0;

		private int SelectedSeqIndex = -1;

		private int DefauseSequenceIndex = -1;

		private bool IsExitApp = false;

		private bool IsChangeSequence = false;

		private bool ExecutionIsEnd = false;

		private int PassDUTCount = 0;

		private int TotalDUTCount = 0;

		private List<Image> BackgroundImageList = new List<Image>(16);

		private bool SessionEnding = false;

		private NextActionAfterEndExecution NextAction = NextActionAfterEndExecution.None;

		private List<SequenceContext> UUTSeqContextList = new List<SequenceContext>(32);

		private List<DUT_SocketUI> DUT_SocketList = new List<DUT_SocketUI>(32);

		private SequenceContext ModelTopSeqContext;

		private SequenceContext RequestsSeqContext;

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.TextBox_Version = new CCWin.SkinControl.SkinWaterTextBox();
            this.TextBox_Sequence = new CCWin.SkinControl.SkinWaterTextBox();
            this.TextBox_Project = new CCWin.SkinControl.SkinWaterTextBox();
            this.TextBox_Station = new CCWin.SkinControl.SkinWaterTextBox();
            this.skinLabel5 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel4 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel3 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel2 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.textBox_SerialNumber = new System.Windows.Forms.TextBox();
            this.label_Total = new System.Windows.Forms.Label();
            this.label_Fail = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label_Pass = new System.Windows.Forms.Label();
            this.label_DUTResult = new System.Windows.Forms.Label();
            this.timer_Main = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox_Logo = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label_MES = new System.Windows.Forms.Label();
            this.label_PassRate = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel_Exe = new System.Windows.Forms.Panel();
            this.label_Time = new System.Windows.Forms.Label();
            this.label_Date = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectSequenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeSequenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reTestDUTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stationOptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mESToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chineseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vietNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.instrumentsSourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel_DUTList = new System.Windows.Forms.TableLayoutPanel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.imageList_BackImage = new System.Windows.Forms.ImageList(this.components);
            this.axSequenceFileViewMgr = new NationalInstruments.TestStand.Interop.UI.Ax.AxSequenceFileViewMgr();
            this.axExecutionViewMgr = new NationalInstruments.TestStand.Interop.UI.Ax.AxExecutionViewMgr();
            this.axApplicationMgr = new NationalInstruments.TestStand.Interop.UI.Ax.AxApplicationMgr();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Logo)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel_Exe.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axSequenceFileViewMgr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axExecutionViewMgr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axApplicationMgr)).BeginInit();
            this.SuspendLayout();
            // 
            // TextBox_Version
            // 
            this.TextBox_Version.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBox_Version.BackColor = System.Drawing.SystemColors.Control;
            this.TextBox_Version.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBox_Version.Enabled = false;
            this.TextBox_Version.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TextBox_Version.Location = new System.Drawing.Point(112, 64);
            this.TextBox_Version.Name = "TextBox_Version";
            this.TextBox_Version.Size = new System.Drawing.Size(243, 30);
            this.TextBox_Version.TabIndex = 32;
            this.TextBox_Version.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.TextBox_Version.WaterText = "";
            // 
            // TextBox_Sequence
            // 
            this.TextBox_Sequence.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBox_Sequence.BackColor = System.Drawing.SystemColors.Control;
            this.TextBox_Sequence.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBox_Sequence.Enabled = false;
            this.TextBox_Sequence.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TextBox_Sequence.Location = new System.Drawing.Point(112, 34);
            this.TextBox_Sequence.Name = "TextBox_Sequence";
            this.TextBox_Sequence.Size = new System.Drawing.Size(243, 30);
            this.TextBox_Sequence.TabIndex = 32;
            this.TextBox_Sequence.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.TextBox_Sequence.WaterText = "";
            // 
            // TextBox_Project
            // 
            this.TextBox_Project.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBox_Project.BackColor = System.Drawing.SystemColors.Control;
            this.TextBox_Project.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBox_Project.Enabled = false;
            this.TextBox_Project.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TextBox_Project.Location = new System.Drawing.Point(112, 94);
            this.TextBox_Project.Name = "TextBox_Project";
            this.TextBox_Project.Size = new System.Drawing.Size(243, 30);
            this.TextBox_Project.TabIndex = 32;
            this.TextBox_Project.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.TextBox_Project.WaterText = "";
            // 
            // TextBox_Station
            // 
            this.TextBox_Station.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBox_Station.BackColor = System.Drawing.SystemColors.Control;
            this.TextBox_Station.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBox_Station.Enabled = false;
            this.TextBox_Station.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TextBox_Station.Location = new System.Drawing.Point(112, 4);
            this.TextBox_Station.Name = "TextBox_Station";
            this.TextBox_Station.Size = new System.Drawing.Size(243, 30);
            this.TextBox_Station.TabIndex = 32;
            this.TextBox_Station.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.TextBox_Station.WaterText = "";
            // 
            // skinLabel5
            // 
            this.skinLabel5.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.Anamorphosis;
            this.skinLabel5.AutoSize = true;
            this.skinLabel5.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel5.BorderColor = System.Drawing.Color.White;
            this.skinLabel5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel5.ForeColor = System.Drawing.Color.Black;
            this.skinLabel5.Location = new System.Drawing.Point(8, 130);
            this.skinLabel5.Name = "skinLabel5";
            this.skinLabel5.Size = new System.Drawing.Size(122, 27);
            this.skinLabel5.TabIndex = 31;
            this.skinLabel5.Text = "Serial Num:";
            // 
            // skinLabel4
            // 
            this.skinLabel4.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.Anamorphosis;
            this.skinLabel4.AutoSize = true;
            this.skinLabel4.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel4.BorderColor = System.Drawing.Color.White;
            this.skinLabel4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel4.ForeColor = System.Drawing.Color.Black;
            this.skinLabel4.Location = new System.Drawing.Point(8, 67);
            this.skinLabel4.Name = "skinLabel4";
            this.skinLabel4.Size = new System.Drawing.Size(89, 27);
            this.skinLabel4.TabIndex = 31;
            this.skinLabel4.Text = "Version:";
            // 
            // skinLabel3
            // 
            this.skinLabel3.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.Anamorphosis;
            this.skinLabel3.AutoSize = true;
            this.skinLabel3.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel3.BorderColor = System.Drawing.Color.White;
            this.skinLabel3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel3.ForeColor = System.Drawing.Color.Black;
            this.skinLabel3.Location = new System.Drawing.Point(8, 97);
            this.skinLabel3.Name = "skinLabel3";
            this.skinLabel3.Size = new System.Drawing.Size(79, 27);
            this.skinLabel3.TabIndex = 31;
            this.skinLabel3.Text = "Model:";
            // 
            // skinLabel2
            // 
            this.skinLabel2.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.Anamorphosis;
            this.skinLabel2.AutoSize = true;
            this.skinLabel2.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel2.BorderColor = System.Drawing.Color.White;
            this.skinLabel2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel2.ForeColor = System.Drawing.Color.Black;
            this.skinLabel2.Location = new System.Drawing.Point(8, 37);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new System.Drawing.Size(109, 27);
            this.skinLabel2.TabIndex = 31;
            this.skinLabel2.Text = "Sequence:";
            // 
            // skinLabel1
            // 
            this.skinLabel1.ArtTextStyle = CCWin.SkinControl.ArtTextStyle.Anamorphosis;
            this.skinLabel1.AutoSize = true;
            this.skinLabel1.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel1.BorderColor = System.Drawing.Color.White;
            this.skinLabel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel1.ForeColor = System.Drawing.Color.Black;
            this.skinLabel1.Location = new System.Drawing.Point(8, 7);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new System.Drawing.Size(84, 27);
            this.skinLabel1.TabIndex = 31;
            this.skinLabel1.Text = "Station:";
            // 
            // textBox_SerialNumber
            // 
            this.textBox_SerialNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_SerialNumber.Font = new System.Drawing.Font("宋体", 15F);
            this.textBox_SerialNumber.Location = new System.Drawing.Point(112, 127);
            this.textBox_SerialNumber.Name = "textBox_SerialNumber";
            this.textBox_SerialNumber.Size = new System.Drawing.Size(243, 36);
            this.textBox_SerialNumber.TabIndex = 29;
            this.textBox_SerialNumber.TextChanged += new System.EventHandler(this.textBox_SerialNumber_TextChanged);
            // 
            // label_Total
            // 
            this.label_Total.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Total.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_Total.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Total.ForeColor = System.Drawing.Color.White;
            this.label_Total.Location = new System.Drawing.Point(72, 66);
            this.label_Total.Name = "label_Total";
            this.label_Total.Size = new System.Drawing.Size(59, 23);
            this.label_Total.TabIndex = 0;
            this.label_Total.Text = "0";
            this.label_Total.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_Fail
            // 
            this.label_Fail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Fail.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_Fail.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Fail.ForeColor = System.Drawing.Color.White;
            this.label_Fail.Location = new System.Drawing.Point(72, 36);
            this.label_Fail.Name = "label_Fail";
            this.label_Fail.Size = new System.Drawing.Size(59, 23);
            this.label_Fail.TabIndex = 0;
            this.label_Fail.Text = "0";
            this.label_Fail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(12, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 23);
            this.label9.TabIndex = 0;
            this.label9.Text = "Total:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(12, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 23);
            this.label8.TabIndex = 0;
            this.label8.Text = "Fail:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(12, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 23);
            this.label7.TabIndex = 0;
            this.label7.Text = "Pass:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Pass
            // 
            this.label_Pass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Pass.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_Pass.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Pass.ForeColor = System.Drawing.Color.White;
            this.label_Pass.Location = new System.Drawing.Point(72, 6);
            this.label_Pass.Name = "label_Pass";
            this.label_Pass.Size = new System.Drawing.Size(59, 23);
            this.label_Pass.TabIndex = 0;
            this.label_Pass.Text = "0";
            this.label_Pass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_DUTResult
            // 
            this.label_DUTResult.BackColor = System.Drawing.SystemColors.Control;
            this.label_DUTResult.Font = new System.Drawing.Font("宋体", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_DUTResult.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label_DUTResult.Location = new System.Drawing.Point(-2, 150);
            this.label_DUTResult.Name = "label_DUTResult";
            this.label_DUTResult.Size = new System.Drawing.Size(198, 10);
            this.label_DUTResult.TabIndex = 26;
            this.label_DUTResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer_Main
            // 
            this.timer_Main.Enabled = true;
            this.timer_Main.Interval = 1000;
            this.timer_Main.Tick += new System.EventHandler(this.timer_Main_Tick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.05776F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.68918F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.87505F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.378F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel_Exe, 3, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(16, 64);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(988, 170);
            this.tableLayoutPanel1.TabIndex = 34;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.pictureBox_Logo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(241, 164);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox_Logo
            // 
            this.pictureBox_Logo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox_Logo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_Logo.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_Logo.Name = "pictureBox_Logo";
            this.pictureBox_Logo.Size = new System.Drawing.Size(237, 160);
            this.pictureBox_Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Logo.TabIndex = 27;
            this.pictureBox_Logo.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.TextBox_Version);
            this.panel2.Controls.Add(this.TextBox_Project);
            this.panel2.Controls.Add(this.TextBox_Station);
            this.panel2.Controls.Add(this.TextBox_Sequence);
            this.panel2.Controls.Add(this.textBox_SerialNumber);
            this.panel2.Controls.Add(this.skinLabel1);
            this.panel2.Controls.Add(this.skinLabel2);
            this.panel2.Controls.Add(this.skinLabel5);
            this.panel2.Controls.Add(this.skinLabel3);
            this.panel2.Controls.Add(this.skinLabel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(250, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(376, 164);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.label_Pass);
            this.panel3.Controls.Add(this.label_MES);
            this.panel3.Controls.Add(this.label_PassRate);
            this.panel3.Controls.Add(this.label_Total);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.label_Fail);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(632, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(150, 164);
            this.panel3.TabIndex = 2;
            // 
            // label_MES
            // 
            this.label_MES.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_MES.BackColor = System.Drawing.Color.Green;
            this.label_MES.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_MES.ForeColor = System.Drawing.Color.Black;
            this.label_MES.Location = new System.Drawing.Point(72, 128);
            this.label_MES.Name = "label_MES";
            this.label_MES.Size = new System.Drawing.Size(59, 23);
            this.label_MES.TabIndex = 0;
            this.label_MES.Text = "ON";
            this.label_MES.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_MES.Click += new System.EventHandler(this.Label_MES_Click);
            // 
            // label_PassRate
            // 
            this.label_PassRate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_PassRate.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_PassRate.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_PassRate.ForeColor = System.Drawing.Color.White;
            this.label_PassRate.Location = new System.Drawing.Point(72, 96);
            this.label_PassRate.Name = "label_PassRate";
            this.label_PassRate.Size = new System.Drawing.Size(59, 23);
            this.label_PassRate.TabIndex = 0;
            this.label_PassRate.Text = "0";
            this.label_PassRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(12, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "MES:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(12, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "PR(%):";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel_Exe
            // 
            this.panel_Exe.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_Exe.Controls.Add(this.label_Time);
            this.panel_Exe.Controls.Add(this.label_Date);
            this.panel_Exe.Controls.Add(this.label_DUTResult);
            this.panel_Exe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Exe.Location = new System.Drawing.Point(788, 3);
            this.panel_Exe.Name = "panel_Exe";
            this.panel_Exe.Size = new System.Drawing.Size(197, 164);
            this.panel_Exe.TabIndex = 3;
            // 
            // label_Time
            // 
            this.label_Time.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Time.Font = new System.Drawing.Font("宋体", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Time.ForeColor = System.Drawing.Color.Black;
            this.label_Time.Location = new System.Drawing.Point(38, 83);
            this.label_Time.Name = "label_Time";
            this.label_Time.Size = new System.Drawing.Size(129, 27);
            this.label_Time.TabIndex = 27;
            this.label_Time.Text = "10:28:26";
            this.label_Time.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Date
            // 
            this.label_Date.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Date.Font = new System.Drawing.Font("宋体", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Date.ForeColor = System.Drawing.Color.Black;
            this.label_Date.Location = new System.Drawing.Point(24, 44);
            this.label_Date.Name = "label_Date";
            this.label_Date.Size = new System.Drawing.Size(157, 27);
            this.label_Date.TabIndex = 27;
            this.label_Date.Text = "2022-10-20";
            this.label_Date.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.userToolStripMenuItem,
            this.debugToolsToolStripMenuItem,
            this.configToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(4, 28);
            this.menuStrip1.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1012, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectSequenceToolStripMenuItem,
            this.closeSequenceToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 21);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // selectSequenceToolStripMenuItem
            // 
            this.selectSequenceToolStripMenuItem.Name = "selectSequenceToolStripMenuItem";
            this.selectSequenceToolStripMenuItem.Size = new System.Drawing.Size(205, 26);
            this.selectSequenceToolStripMenuItem.Text = "Select Sequence";
            this.selectSequenceToolStripMenuItem.Click += new System.EventHandler(this.selectSequenceToolStripMenuItem_Click);
            // 
            // closeSequenceToolStripMenuItem
            // 
            this.closeSequenceToolStripMenuItem.Name = "closeSequenceToolStripMenuItem";
            this.closeSequenceToolStripMenuItem.Size = new System.Drawing.Size(205, 26);
            this.closeSequenceToolStripMenuItem.Text = "Close Sequence";
            this.closeSequenceToolStripMenuItem.Click += new System.EventHandler(this.closeSequenceToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(205, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // userToolStripMenuItem
            // 
            this.userToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loginToolStripMenuItem,
            this.logoutToolStripMenuItem});
            this.userToolStripMenuItem.Name = "userToolStripMenuItem";
            this.userToolStripMenuItem.Size = new System.Drawing.Size(54, 21);
            this.userToolStripMenuItem.Text = "User";
            // 
            // loginToolStripMenuItem
            // 
            this.loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            this.loginToolStripMenuItem.Size = new System.Drawing.Size(137, 26);
            this.loginToolStripMenuItem.Text = "Login";
            this.loginToolStripMenuItem.Click += new System.EventHandler(this.loginOutToolStripMenuItem_Click);
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(137, 26);
            this.logoutToolStripMenuItem.Text = "Logout";
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.loginOutToolStripMenuItem_Click);
            // 
            // debugToolsToolStripMenuItem
            // 
            this.debugToolsToolStripMenuItem.Name = "debugToolsToolStripMenuItem";
            this.debugToolsToolStripMenuItem.Size = new System.Drawing.Size(61, 21);
            this.debugToolsToolStripMenuItem.Text = "Tools";
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reTestDUTToolStripMenuItem,
            this.stationOptionToolStripMenuItem,
            this.mESToolStripMenuItem,
            this.languageToolStripMenuItem,
            this.instrumentsSourceToolStripMenuItem});
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(93, 21);
            this.configToolStripMenuItem.Text = "Configure";
            // 
            // reTestDUTToolStripMenuItem
            // 
            this.reTestDUTToolStripMenuItem.Name = "reTestDUTToolStripMenuItem";
            this.reTestDUTToolStripMenuItem.Size = new System.Drawing.Size(199, 26);
            this.reTestDUTToolStripMenuItem.Text = "Reset Counter";
            this.reTestDUTToolStripMenuItem.Click += new System.EventHandler(this.reTestDUTToolStripMenuItem_Click);
            // 
            // stationOptionToolStripMenuItem
            // 
            this.stationOptionToolStripMenuItem.Name = "stationOptionToolStripMenuItem";
            this.stationOptionToolStripMenuItem.Size = new System.Drawing.Size(199, 26);
            this.stationOptionToolStripMenuItem.Text = "Station Options";
            this.stationOptionToolStripMenuItem.Click += new System.EventHandler(this.stationOptionToolStripMenuItem_Click);
            // 
            // mESToolStripMenuItem
            // 
            this.mESToolStripMenuItem.Checked = true;
            this.mESToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mESToolStripMenuItem.Name = "mESToolStripMenuItem";
            this.mESToolStripMenuItem.Size = new System.Drawing.Size(199, 26);
            this.mESToolStripMenuItem.Text = "MES";
            this.mESToolStripMenuItem.Click += new System.EventHandler(this.mESToolStripMenuItem_Click);
            // 
            // languageToolStripMenuItem
            // 
            this.languageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chineseToolStripMenuItem,
            this.englishToolStripMenuItem,
            this.vietNameToolStripMenuItem});
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            this.languageToolStripMenuItem.Size = new System.Drawing.Size(199, 26);
            this.languageToolStripMenuItem.Text = "Language";
            this.languageToolStripMenuItem.Click += new System.EventHandler(this.languageToolStripMenuItem_Click);
            // 
            // chineseToolStripMenuItem
            // 
            this.chineseToolStripMenuItem.Name = "chineseToolStripMenuItem";
            this.chineseToolStripMenuItem.Size = new System.Drawing.Size(155, 26);
            this.chineseToolStripMenuItem.Text = "Chinese";
            this.chineseToolStripMenuItem.Click += new System.EventHandler(this.languageToolStripMenuItem_Click);
            // 
            // englishToolStripMenuItem
            // 
            this.englishToolStripMenuItem.Checked = true;
            this.englishToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            this.englishToolStripMenuItem.Size = new System.Drawing.Size(155, 26);
            this.englishToolStripMenuItem.Text = "English";
            this.englishToolStripMenuItem.Click += new System.EventHandler(this.languageToolStripMenuItem_Click);
            // 
            // vietNameToolStripMenuItem
            // 
            this.vietNameToolStripMenuItem.Name = "vietNameToolStripMenuItem";
            this.vietNameToolStripMenuItem.Size = new System.Drawing.Size(155, 26);
            this.vietNameToolStripMenuItem.Text = "ViệtName";
            this.vietNameToolStripMenuItem.Click += new System.EventHandler(this.textBox_SerialNumber_TextChanged);
            // 
            // instrumentsSourceToolStripMenuItem
            // 
            this.instrumentsSourceToolStripMenuItem.Name = "instrumentsSourceToolStripMenuItem";
            this.instrumentsSourceToolStripMenuItem.Size = new System.Drawing.Size(199, 26);
            this.instrumentsSourceToolStripMenuItem.Text = "Instruments List";
            this.instrumentsSourceToolStripMenuItem.Click += new System.EventHandler(this.instrumentsSourceToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(56, 21);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // tableLayoutPanel_DUTList
            // 
            this.tableLayoutPanel_DUTList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel_DUTList.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel_DUTList.ColumnCount = 2;
            this.tableLayoutPanel_DUTList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_DUTList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_DUTList.Location = new System.Drawing.Point(9, 212);
            this.tableLayoutPanel_DUTList.Name = "tableLayoutPanel_DUTList";
            this.tableLayoutPanel_DUTList.RowCount = 1;
            this.tableLayoutPanel_DUTList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_DUTList.Size = new System.Drawing.Size(994, 471);
            this.tableLayoutPanel_DUTList.TabIndex = 0;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.SteelBlue;
            this.panel6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel6.Controls.Add(this.tableLayoutPanel_DUTList);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(4, 28);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1012, 689);
            this.panel6.TabIndex = 36;
            // 
            // imageList_BackImage
            // 
            this.imageList_BackImage.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList_BackImage.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList_BackImage.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // axSequenceFileViewMgr
            // 
            this.axSequenceFileViewMgr.Enabled = true;
            this.axSequenceFileViewMgr.Location = new System.Drawing.Point(71, 682);
            this.axSequenceFileViewMgr.Name = "axSequenceFileViewMgr";
            this.axSequenceFileViewMgr.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axSequenceFileViewMgr.OcxState")));
            this.axSequenceFileViewMgr.Size = new System.Drawing.Size(32, 32);
            this.axSequenceFileViewMgr.TabIndex = 20;
            // 
            // axExecutionViewMgr
            // 
            this.axExecutionViewMgr.Enabled = true;
            this.axExecutionViewMgr.Location = new System.Drawing.Point(127, 682);
            this.axExecutionViewMgr.Name = "axExecutionViewMgr";
            this.axExecutionViewMgr.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axExecutionViewMgr.OcxState")));
            this.axExecutionViewMgr.Size = new System.Drawing.Size(32, 32);
            this.axExecutionViewMgr.TabIndex = 21;
            // 
            // axApplicationMgr
            // 
            this.axApplicationMgr.Enabled = true;
            this.axApplicationMgr.Location = new System.Drawing.Point(15, 682);
            this.axApplicationMgr.Name = "axApplicationMgr";
            this.axApplicationMgr.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axApplicationMgr.OcxState")));
            this.axApplicationMgr.Size = new System.Drawing.Size(32, 32);
            this.axApplicationMgr.TabIndex = 19;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1020, 721);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.axSequenceFileViewMgr);
            this.Controls.Add(this.axExecutionViewMgr);
            this.Controls.Add(this.axApplicationMgr);
            this.Controls.Add(this.panel6);
            this.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FTest Platform 2017";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Logo)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel_Exe.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axSequenceFileViewMgr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axExecutionViewMgr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axApplicationMgr)).EndInit();
            this.ResumeLayout(false);

		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetForegroundWindow", ExactSpelling = true)]
		public static extern IntPtr GetF();

		[DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
		public static extern bool SetF(IntPtr hWnd);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern long SetWindowPos(IntPtr hwnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern long GetWindowLong(IntPtr hwnd, int nIndex);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr SetWindowLong(IntPtr hwnd, int nIndex, long dwNewLong);

		public FormMain()
		{
			InitializeComponent();
			axExecutionViewMgr.StepGroupMode = StepGroupModes.StepGroupMode_AllGroups;
			axExecutionViewMgr.RunStateChanged += axExecutionViewMgr_RunStateChanged;
			axApplicationMgr.DisplayExecution += axApplicationMgr_DisplayExecution;
			axApplicationMgr.UIMessageEvent += axApplicationMgr_UIMessageEvent;
			axApplicationMgr.UserChanged += axApplicationMgr_UserChanged;
			axApplicationMgr.ReloadSequenceFilesOnStart = ReloadFiles.ReloadFile_None;
			axApplicationMgr.Start();
			NextAction = NextActionAfterEndExecution.None;
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			try
			{
				if (base.Handle != GetF())
				{
					SetF(base.Handle);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void axApplicationMgr_UserChanged(object sender, _ApplicationMgrEvents_UserChangedEvent e)
		{
			if (e.user != null)
			{
				Text = "FTest Platform 2017 -- " + ((User)e.user).LoginName;
				foreach (DUT_SocketUI item in DUT_SocketList)
				{
					item.Enabled = true;
				}
				selectSequenceToolStripMenuItem.Enabled = true;
				base.WindowState = FormWindowState.Maximized;
				Application.DoEvents();
				if (TextBox_Sequence.Text == "" && 0 == LoadSequenceListToMemory(enableDefaultSequence: true))
				{
					NextAction = NextActionAfterEndExecution.ReloadSequence;
					LaunchSequenceByIndex(SelectedSeqIndex);
					GetTestedDUTCount();
				}
				return;
			}
			Text = "FTest Platform 2017";
			foreach (DUT_SocketUI item in DUT_SocketList)
			{
				item.Enabled = false;
			}
			selectSequenceToolStripMenuItem.Enabled = false;
		}

		private void axApplicationMgr_UIMessageEvent(object sender, _ApplicationMgrEvents_UIMessageEventEvent e)
		{
			try
			{
                switch (((UIMessage)e.uiMsg).Event)
                {
                    case UIMessageCodes.UIMsg_EndExecution:
                        ExecutionIsEnd = true;
                        if (((UIMessage)e.uiMsg).Execution.ClientFile != null)
                        {
                            string clientFilePath = ((UIMessage)e.uiMsg).Execution.ClientFile.Path;
                            if (((UIMessage)e.uiMsg).Execution.ClientFile.CanUnload)
                            {
                                System.Threading.Thread thread = new System.Threading.Thread(NextActionThread);
                                thread.IsBackground = true;
                                thread.Start();
                            }
                        }
                        break;
                    case UIMessageCodes.UIMsg_ModelState_Identified:
                        SerialNumber = ((UIMessage)e.uiMsg).StringData;
                        break;
                    case UIMessageCodes.UIMsg_ModelState_Initializing:
                        if (!(((UIMessage)e.uiMsg).NumericData < 0.0))
                        {
                            break;
                        }
                        tableLayoutPanel_DUTList.Visible = true;
                        {
                            foreach (DUT_SocketUI item in DUT_SocketList)
                            {
                                item.Initializing();
                            }
                            break;
                        }
                    case UIMessageCodes.UIMsg_ModelState_TestingComplete:
                        {
                            int socketIndex = (int)((UIMessage)e.uiMsg).NumericData;
                            if (socketIndex < 0)
                            {
                                break;
                            }
                            string logfileFolderPath = Path.Combine(SeqSettingList[SelectedSeqIndex].LogFilePath, DateTime.Now.ToString("yyyyMMdd"));
                            if (!Directory.Exists(logfileFolderPath))
                            {
                                for (int i = -1; i > -366; i--)
                                {
                                    logfileFolderPath = Path.Combine(SeqSettingList[SelectedSeqIndex].LogFilePath, DateTime.Now.AddDays(i).ToString("yyyyMMdd"));
                                    if (Directory.Exists(logfileFolderPath))
                                    {
                                        logfileFolderPath = Path.Combine(logfileFolderPath, "DUTCount.log");
                                        StreamWriter sw = new StreamWriter(logfileFolderPath);
                                        sw.WriteLine("[DUTCount]");
                                        sw.WriteLine("PASS\t\t=" + PassDUTCount);
                                        sw.WriteLine("Fail\t\t=" + (TotalDUTCount - PassDUTCount));
                                        sw.WriteLine("Total\t\t=" + TotalDUTCount);
                                        sw.Close();
                                    }
                                }
                            }
                            DUT_SocketList[socketIndex].TestingComplete(((UIMessage)e.uiMsg).StringData);
                            TotalDUTCount++;
                            label_Total.Text = TotalDUTCount.ToString();
                            if (((UIMessage)e.uiMsg).StringData == "Passed")
                            {
                                PassDUTCount++;
                            }
                            label_Pass.Text = PassDUTCount.ToString();
                            label_Fail.Text = (TotalDUTCount - PassDUTCount).ToString();
                            label_PassRate.Text = Math.Round((double)PassDUTCount * 1.0 / (double)TotalDUTCount * 100.0, 1).ToString();
                            SetDUTCount(label_Pass.Text, label_Total.Text);
                            UUTSeqContextList[socketIndex].AsPropertyObject().SetValString("Parameters.TestSocket.UUT.BatchSerialNumber", 1, DUT_SocketList[socketIndex].LogfileFullPath);
                            textBox_SerialNumber.Focus();
                            break;
                        }
                    case UIMessageCodes.UIMsg_ModelState_Waiting:
                        if (((UIMessage)e.uiMsg).StringData.ToLower().Contains("true"))
                        {
                            DUT_SocketList[(int)((UIMessage)e.uiMsg).NumericData].InitializFinished("PreUUTLoop Error");
                            break;
                        }
                        DUT_SocketList[(int)((UIMessage)e.uiMsg).NumericData].InitializFinished("Ready");
                        UUTSeqContextList[(int)((UIMessage)e.uiMsg).NumericData] = (SequenceContext)(dynamic)((UIMessage)e.uiMsg).ActiveXData;
                        UUTSeqContextList[(int)((UIMessage)e.uiMsg).NumericData].AsPropertyObject().SetValString("FileGlobals.StationSetting.LogFilePath", 1, SeqSettingList[SelectedSeqIndex].LogFilePath);
                        UUTSeqContextList[(int)((UIMessage)e.uiMsg).NumericData].AsPropertyObject().SetValString("FileGlobals.StationSetting.ProjectName", 1, SeqSettingList[SelectedSeqIndex].ProjectName);
                        UUTSeqContextList[(int)((UIMessage)e.uiMsg).NumericData].AsPropertyObject().SetValString("FileGlobals.StationSetting.Station", 1, SeqSettingList[SelectedSeqIndex].StationName);
                        UUTSeqContextList[(int)((UIMessage)e.uiMsg).NumericData].AsPropertyObject().SetValNumber("FileGlobals.StationSetting.SocketNumber", 1, SeqSettingList[SelectedSeqIndex].SocketNumber);
                        LoadExeInterface(SeqSettingList[SelectedSeqIndex].ExePath);
                        
					break;
				case UIMessageCodes.UIMsg_StartFileExecution:
				{
					tableLayoutPanel_DUTList.Visible = true;
					SequenceFile sf = (SequenceFile)(dynamic)((UIMessage)e.uiMsg).ActiveXData;
					string str = sf.Path;
					str = sf.ModelPath;
					break;
				}
				case UIMessageCodes.UIMsg_TerminatingExecution:
					ExecutionIsEnd = true;
					break;
				case (UIMessageCodes)10003:
					tableLayoutPanel_DUTList.Visible = true;
					ModelTopSeqContext = (SequenceContext)(dynamic)((UIMessage)e.uiMsg).ActiveXData;
					ModelTopSeqContext.AsPropertyObject().SetValNumber("FileGlobals.SelectedSequenceIndex", 1, SelectedSeqIndex);
					ModelTopSeqContext.AsPropertyObject().SetValNumber("Locals.ModelData.ModelOptions.NumTestSockets", 1, SeqSettingList[SelectedSeqIndex].SocketNumber);
					break;
				case (UIMessageCodes)10004:
				{
					bool bb = ModelTopSeqContext.IsProcessModel;
					ModelTopSeqContext.AsPropertyObject().SetValBoolean("Locals.ModelData.ModelOptions.ParallelModel_ShowUUTDlg", 1, newValue: false);
					break;
				}
				case (UIMessageCodes)10005:
					DUT_SocketList[(int)((UIMessage)e.uiMsg).NumericData].SerialNumber = ((UIMessage)e.uiMsg).StringData;
					DUT_SocketList[(int)((UIMessage)e.uiMsg).NumericData].TestStatusLable = ((UIMessage)e.uiMsg).StringData + " Testing";
					UUTSeqContextList[(int)((UIMessage)e.uiMsg).NumericData].AsPropertyObject().SetValString("Parameters.TestSocket.UUT.SerialNumber", 1, ((UIMessage)e.uiMsg).StringData);
					break;
				case (UIMessageCodes)10001:
				{
					StepResultStruct stepResult = GetStepResultFromSequence((SequenceContext)(dynamic)((UIMessage)e.uiMsg).ActiveXData);
					DUT_SocketList[(int)((UIMessage)e.uiMsg).NumericData].Update_listView_ResultList(stepResult);
					break;
				}
				case (UIMessageCodes)10002:
					if (SelectedSeqIndex != (int)((UIMessage)e.uiMsg).NumericData)
					{
						NextAction = NextActionAfterEndExecution.ReloadSequence;
						SendRequestToModelSequence("Stop All TestSockets", -1);
						SendRequestToModelSequence("Exit", -1);
						SelectedSeqIndex = (int)((UIMessage)e.uiMsg).NumericData;
					}
					break;
				case (UIMessageCodes)10006:
				{
					SequenceContext tempSeqc = (SequenceContext)(dynamic)((UIMessage)e.uiMsg).ActiveXData;
					PropertyObject ControllerRequest = ModelTopSeqContext.Engine.GetTypeDefinition("NI_ParallelControllerRequest");
					ControllerRequest.SetValString("Request", 0, "Continue TestSocket");
					ControllerRequest.SetValNumber("TestSocketIndex", 0, 0.0);
					dynamic queue = tempSeqc.AsPropertyObject().GetValVariant("Parameters.ModelData.DialogRequestQueue", 0);
					int x = 100;
					int y = 100;
					queue.Enqueue(false, 0, ControllerRequest, false, -1.0, tempSeqc, false, ref x, ref y);
					break;
				}
				case (UIMessageCodes)10007:
					RequestsSeqContext = (SequenceContext)(dynamic)((UIMessage)e.uiMsg).ActiveXData;
					break;
				case (UIMessageCodes)10008:
					if (((UIMessage)e.uiMsg).NumericData == 0.0)
					{
						label_DUTResult.Text = ((UIMessage)e.uiMsg).StringData;
						label_DUTResult.BackColor = SystemColors.Control;
					}
					else if (((UIMessage)e.uiMsg).NumericData > 0.0)
					{
						label_DUTResult.Text = ((UIMessage)e.uiMsg).StringData;
						label_DUTResult.BackColor = SystemColors.Control;
					}
					else
					{
						label_DUTResult.Text = ((UIMessage)e.uiMsg).StringData;
						label_DUTResult.BackColor = Color.Red;
						textBox_SerialNumber.SelectAll();
					}
					break;
				case (UIMessageCodes)10009:
					DUT_SocketList[(int)((UIMessage)e.uiMsg).NumericData].DUTSetting.LogFilePath = ((UIMessage)e.uiMsg).StringData;
					UUTSeqContextList[(int)((UIMessage)e.uiMsg).NumericData].AsPropertyObject().SetValString("FileGlobals.StationSetting.LogFilePath", 1, SeqSettingList[SelectedSeqIndex].LogFilePath);
					break;
				case (UIMessageCodes)10010:
					DUT_SocketList[(int)((UIMessage)e.uiMsg).NumericData].DUTSetting.ProjectName = ((UIMessage)e.uiMsg).StringData;
					UUTSeqContextList[(int)((UIMessage)e.uiMsg).NumericData].AsPropertyObject().SetValString("FileGlobals.StationSetting.ProjectName", 1, SeqSettingList[SelectedSeqIndex].ProjectName);
					TextBox_Project.Text = SeqSettingList[0].CustomerName + "-" + SeqSettingList[0].ProjectName;
					Application.DoEvents();
					break;
				case (UIMessageCodes)10011:
					if (((UIMessage)e.uiMsg).StringData.ToLower().Contains("true"))
					{
						textBox_SerialNumber.Enabled = true;
						foreach (DUT_SocketUI item in DUT_SocketList)
						{
							item.SetSartBtnEnable(isEnable: true);
						}
					}
					else
					{
						textBox_SerialNumber.Enabled = false;
						foreach (DUT_SocketUI item in DUT_SocketList)
						{
							item.SetSartBtnEnable(isEnable: false);
						}
					}
					Application.DoEvents();
					break;
				case (UIMessageCodes)10012:
				{
					int dutIndex = (int)((UIMessage)e.uiMsg).NumericData;
					if (DUT_SocketList[dutIndex].SerialNumber == "")
					{
						DUT_SocketList[dutIndex].SerialNumber = textBox_SerialNumber.Text;
					}
					UUTSeqContextList[dutIndex].AsPropertyObject().SetValString("Parameters.TestSocket.UUT.SerialNumber", 1, DUT_SocketList[dutIndex].SerialNumber);
					DUT_SocketList[(int)((UIMessage)e.uiMsg).NumericData].BeginTesting();
					textBox_SerialNumber.Text = "";
					break;
				}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void axExecutionViewMgr_RunStateChanged(object sender, _ExecutionViewMgrEvents_RunStateChangedEvent e)
		{
			if (e.newRunState == ExecutionRunStates.ExecRunState_Stopped && IsChangeSequence)
			{
			}
		}

		private void axApplicationMgr_DisplayExecution(object sender, _ApplicationMgrEvents_DisplayExecutionEvent e)
		{
			if (e.reason == ExecutionDisplayReasons.ExecutionDisplayReason_Breakpoint || e.reason == ExecutionDisplayReasons.ExecutionDisplayReason_BreakOnRunTimeError)
			{
				Activate();
			}
			axExecutionViewMgr.Execution = e.exec;
		}

		private void SendRequestToModelSequence(string commandStr, int socketIndex)
		{
			try
			{
				PropertyObject pControllerRequest = ModelTopSeqContext.AsPropertyObject().GetPropertyObject("Locals.ControllerRequest", 0);
				pControllerRequest.SetValString("Request", 0, commandStr);
				pControllerRequest.SetValNumber("TestSocketIndex", 0, socketIndex);
				dynamic queue = ModelTopSeqContext.AsPropertyObject().GetValInterface("Locals.ModelData.DialogRequestQueue", 0);
				int x = 100;
				int y = 100;
				queue.Enqueue(false, 0, pControllerRequest, false, -1.0, ModelTopSeqContext, false, ref x, ref y);
				if (x != 0)
				{
					MessageBox.Show("Send Request To ModelSequence Error!");
				}
			}
			catch (Exception innerException)
			{
				throw new Exception("", innerException);
			}
		}

		private void InitModelSeqConnection()
		{
			int port = 2017;
			int MaxConnection = 10;
			Hashtable clientSessionTable = new Hashtable();
			object clientSessionLock = new object();
			IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);
			Socket socketLister = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			socketLister.Bind(localEndPoint);
			try
			{
				socketLister.Listen(MaxConnection);
				while (true)
				{
					bool flag = true;
					Socket clientSocket = socketLister.Accept();
					ClientSession clientSession = new ClientSession(clientSocket);
					lock (clientSessionLock)
					{
						if (!clientSessionTable.ContainsKey(clientSession.IP))
						{
							clientSessionTable.Add(clientSession.IP, clientSession);
						}
					}
					SocketConnection socketConnection = new SocketConnection(clientSocket);
					socketConnection.DataReceived += socketConnection_DataReceived;
					socketConnection.BeginReceiveData();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		public StepResultStruct GetStepResultFromSequence(SequenceContext sc)
		{
			StepResultStruct stepResult = new StepResultStruct();
			try
			{
				PropertyObject po = sc.Parameters;
				stepResult.StepName = po.GetValString("Result.TS.StepName", 0);
				stepResult.StepType = po.GetValString("Result.TS.StepType", 0);
				stepResult.StepGroup = po.GetValString("Result.TS.StepGroup", 0);
				stepResult.StartTime = po.GetValNumber("Result.TS.StartTime", 0);
				stepResult.TotalTime = po.GetValNumber("Result.TS.TotalTime", 0);
				stepResult.Status = po.GetValString("Result.Status", 0);
				switch (stepResult.StepType)
				{
				case "NumericLimitTest":
					try
					{
						stepResult.LimitsLow = po.GetValNumber("Result.Limits.Low", 0);
					}
					catch (Exception)
					{
						stepResult.LimitsLow = -999.0;
					}
					try
					{
						stepResult.LimitsHigh = po.GetValNumber("Result.Limits.High", 0);
					}
					catch (Exception)
					{
						stepResult.LimitsHigh = -999.0;
					}
					try
					{
						stepResult.Unit = po.GetValString("Result.Units", 0);
					}
					catch (Exception)
					{
						stepResult.Unit = "";
					}
					stepResult.NumericValue = po.GetValNumber("Result.Numeric", 0);
					stepResult.Comp = po.GetValString("Result.Comp", 0);
					break;
				case "PassFailTest":
					stepResult.PassFailValue = po.GetValBoolean("Result.PassFail", 0);
					break;
				case "StringValueTest":
					stepResult.StringValue = po.GetValString("Result.String", 0);
					stepResult.LimitsString = po.GetValString("Result.Limits.String", 0);
					stepResult.Comp = po.GetValString("Result.Comp", 0);
					break;
				}
			}
			catch (Exception ex4)
			{
				MessageBox.Show(ex4.ToString());
			}
			finally
			{
			}
			return stepResult;
		}

		private void socketConnection_DataReceived(byte[] dataBuffer)
		{
			try
			{
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private byte[] StepResultStructToBytes(StepResultStruct srs)
		{
			try
			{
				BinaryFormatter bf = new BinaryFormatter();
				MemoryStream memory = new MemoryStream();
				bf.Serialize(memory, srs);
				byte[] bytes = memory.GetBuffer();
				memory.Close();
				return bytes;
			}
			catch (Exception)
			{
				throw;
			}
		}

		private StepResultStruct BytesToStepResultStruct(byte[] bytes)
		{
			try
			{
				BinaryFormatter bf = new BinaryFormatter();
				MemoryStream memory = new MemoryStream(bytes);
				StepResultStruct srs = (StepResultStruct)bf.Deserialize(memory);
				memory.Close();
				return srs;
			}
			catch (Exception)
			{
				throw;
			}
		}

		private int LoadSequenceListToMemory(bool enableDefaultSequence)
		{
			try
			{
				SeqSettingList.Clear();
				IniFile seqIniFile = new IniFile();
				DefauseSequenceIndex = int.Parse(seqIniFile.IniReadValue("Config", "DefauseSequenceIndex"));
				int seqCount = int.Parse(seqIniFile.IniReadValue("Config", "SequenceCount"));
				for (int i = 0; i < seqCount; i++)
				{
					SequenceSetting ss = new SequenceSetting();
					ss.StepDisplayType = seqIniFile.IniReadValue("SequenceList_" + i, "StepDisplayType");
					ss.ProjectName = seqIniFile.IniReadValue("SequenceList_" + i, "ProjectName");
					ss.CustomerName = seqIniFile.IniReadValue("SequenceList_" + i, "CustomerName");
					ss.StationName = seqIniFile.IniReadValue("SequenceList_" + i, "StationName");
					ss.AutomationPath = seqIniFile.IniReadValue("SequenceList_" + i, "AutomationPath");
					ss.SequenceFile = seqIniFile.IniReadValue("SequenceList_" + i, "SequenceFile");
					ss.ConfigFile = seqIniFile.IniReadValue("SequenceList_" + i, "ConfigFile");
					ss.Version = seqIniFile.IniReadValue("SequenceList_" + i, "Version");
					ss.LogFilePath = seqIniFile.IniReadValue("SequenceList_" + i, "LogFile");
					ss.LogoFilePath = seqIniFile.IniReadValue("SequenceList_" + i, "LogoFilePath");
					ss.ExePath = seqIniFile.IniReadValue("SequenceList_" + i, "ExePath");
					ss.SerialNumberLen = int.Parse(seqIniFile.IniReadValue("SequenceList_" + i, "SerialNumberLen"));
					ss.SocketNumber = int.Parse(seqIniFile.IniReadValue("SequenceList_" + i, "SocketNumber"));
					SeqSettingList.Add(ss);
				}
				if (enableDefaultSequence && DefauseSequenceIndex >= 0 && DefauseSequenceIndex < seqCount)
				{
					SelectedSeqIndex = DefauseSequenceIndex;
					return 0;
				}
				if (seqCount == 1)
				{
					SelectedSeqIndex = 0;
					return 0;
				}
				SequenceSelection seqSelection = new SequenceSelection(SeqSettingList);
				if (seqSelection.ShowDialog(this) == DialogResult.OK)
				{
					SelectedSeqIndex = seqSelection.SeqIndex;
					return 0;
				}
				return -1;
			}
			catch (Exception innerException)
			{
				throw new Exception("", innerException);
			}
		}

		private void LaunchSequenceByIndex(int seqIndex)
		{
			try
			{
				int seqCount = SeqSettingList.Count;
				pictureBox_Logo.Load(SeqSettingList[seqIndex].LogoFilePath);
				if (seqIndex < SeqSettingList.Count && SeqSettingList.Count > 0)
				{
					SequenceFile sf_TargetSeq = (SequenceFile)axApplicationMgr.OpenSequenceFile(SeqSettingList[seqIndex].SequenceFile);
					TextBox_Station.Text = SeqSettingList[seqIndex].StationName;
					TextBox_Project.Text = SeqSettingList[seqIndex].CustomerName + "-" + SeqSettingList[seqIndex].ProjectName;
					TextBox_Version.Text = SeqSettingList[seqIndex].Version;
					TextBox_Sequence.Text = SeqSettingList[seqIndex].SequenceFile.Substring(SeqSettingList[seqIndex].SequenceFile.LastIndexOf("\\") + 1);
					Sequence seq = sf_TargetSeq.GetSequenceByName("MainSequence");
					tableLayoutPanel_DUTList.Visible = false;
					Application.DoEvents();
					UUTSeqContextList.Clear();
					DUT_SocketList.Clear();
					float dutWidthRatio = 1f / (float)SeqSettingList[seqIndex].SocketNumber;
					TotalStepCount = GetTotalStepNums(sf_TargetSeq, seq);
					tableLayoutPanel_DUTList.ColumnCount = SeqSettingList[seqIndex].SocketNumber;
					tableLayoutPanel_DUTList.ColumnStyles.Clear();
					tableLayoutPanel_DUTList.Controls.Clear();
					for (int socketIndex = 0; socketIndex < SeqSettingList[seqIndex].SocketNumber; socketIndex++)
					{
						DUT_SocketUI dutSocket = new DUT_SocketUI(socketIndex);
						dutSocket.TotalStepCount = TotalStepCount;
						dutSocket.DUTSetting = SeqSettingList[seqIndex];
						dutSocket.StartButtonClicked += dutSocket_StartButtonClicked;
						dutSocket.StopButtonClicked += dutSocket_StopButtonClicked;
						DUT_SocketList.Add(dutSocket);
						SequenceContext sc = null;
						UUTSeqContextList.Add(sc);
						tableLayoutPanel_DUTList.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (float)tableLayoutPanel_DUTList.Width * dutWidthRatio));
						tableLayoutPanel_DUTList.ColumnStyles[socketIndex].Width = (float)tableLayoutPanel_DUTList.Width * dutWidthRatio;
						tableLayoutPanel_DUTList.Controls.Add(dutSocket, socketIndex, 0);
						dutSocket.Dock = DockStyle.Fill;
					}
					axSequenceFileViewMgr.SequenceFile = (SequenceFile)sf_TargetSeq;
					Command cmd = (Command)axSequenceFileViewMgr.GetCommand(CommandKinds.CommandKind_ExecutionEntryPoints_Set);
					cmd.Execute(applicationHandlesError: false);
				}
			}
			catch (Exception innerException)
			{
				throw new Exception("", innerException);
			}
		}

		private void dutSocket_StopButtonClicked(int socketIndex)
		{
			try
			{
				SendRequestToModelSequence("Terminate TestSocket", socketIndex);
				UUTSeqContextList[socketIndex].AsPropertyObject().SetValBoolean("FileGlobals.StartRunning", 0, newValue: true);
			}
			catch (Exception)
			{
				throw;
			}
		}

		private void dutSocket_StartButtonClicked(int socketIndex)
		{
			try
			{
				SendRequestToModelSequence("Continue TestSocket", socketIndex);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void ReLoadSequenceByIndex(int seqIndex)
		{
			try
			{
				int seqCount = SeqSettingList.Count;
				if (seqCount > seqIndex)
				{
					SelectedSeqIndex = seqIndex;
				}
				else
				{
					SelectedSeqIndex = 0;
				}
				if (SelectedSeqIndex >= SeqSettingList.Count || SeqSettingList.Count <= 0)
				{
					return;
				}
				SequenceFile sf_TargetSeq = (SequenceFile)axApplicationMgr.OpenSequenceFile(SeqSettingList[SelectedSeqIndex].SequenceFile);
				TextBox_Station.Text = SeqSettingList[SelectedSeqIndex].StationName;
				TextBox_Project.Text = SeqSettingList[SelectedSeqIndex].CustomerName + "-" + SeqSettingList[SelectedSeqIndex].ProjectName;
				TextBox_Version.Text = SeqSettingList[SelectedSeqIndex].Version;
				TextBox_Sequence.Text = SeqSettingList[SelectedSeqIndex].SequenceFile.Substring(SeqSettingList[SelectedSeqIndex].SequenceFile.LastIndexOf("\\") + 1);
				Sequence seq = sf_TargetSeq.GetSequenceByName("MainSequence");
				TotalStepCount = GetTotalStepNums(sf_TargetSeq, seq);
				foreach (DUT_SocketUI item in DUT_SocketList)
				{
					item.TotalStepCount = TotalStepCount;
				}
				axSequenceFileViewMgr.SequenceFile = (SequenceFile)sf_TargetSeq;
				Command cmd = (Command)axSequenceFileViewMgr.GetCommand(CommandKinds.CommandKind_ExecutionEntryPoints_Set);
				cmd.Execute(applicationHandlesError: false);
			}
			catch (Exception)
			{
				throw;
			}
		}

		private int GetTotalStepNums(SequenceFile sf_TargetSeq, Sequence seq)
		{
			try
			{
				int totalStepNums = 0;
				int subSeqCount = sf_TargetSeq.NumSequences;
				int stepCount = seq.GetNumSteps(StepGroups.StepGroup_Main);
				for (int i = 0; i < stepCount; i++)
				{
					totalStepNums++;
					Step step = seq.GetStep(i, StepGroups.StepGroup_Main);
					if (!step.IsSequenceCall)
					{
						continue;
					}
					string descriptionStr = step.GetDescriptionEx();
					if (!descriptionStr.Contains("<Current File>"))
					{
						continue;
					}
					Sequence subSeq = null;
					for (int seqIndex = 0; seqIndex < subSeqCount; seqIndex++)
					{
						subSeq = sf_TargetSeq.GetSequence(seqIndex);
						if (descriptionStr == $"Call {subSeq.Name} in <Current File>")
						{
							break;
						}
					}
					totalStepNums += GetTotalStepNums(sf_TargetSeq, subSeq);
				}
				return totalStepNums;
			}
			catch (Exception)
			{
				throw;
			}
		}

		private int GetTotalStepNumsEx(SequenceFile sf_TargetSeq, Sequence seq)
		{
			try
			{
				int totalStepNums = 0;
				int subSeqCount = sf_TargetSeq.NumSequences;
				int stepCount = seq.GetNumSteps(StepGroups.StepGroup_Main);
				for (int i = 0; i < stepCount; i++)
				{
					totalStepNums++;
					Step step = seq.GetStep(i, StepGroups.StepGroup_Main);
					if (!step.IsSequenceCall)
					{
						continue;
					}
					string descriptionStr = step.GetDescriptionEx();
					if (!descriptionStr.Contains("<Current File>"))
					{
						continue;
					}
					Sequence subSeq = null;
					for (int seqIndex = 0; seqIndex < subSeqCount; seqIndex++)
					{
						subSeq = sf_TargetSeq.GetSequence(seqIndex);
						if (descriptionStr.Contains(subSeq.Name))
						{
							break;
						}
					}
					totalStepNums += GetTotalStepNums(sf_TargetSeq, subSeq);
				}
				return totalStepNums;
			}
			catch (Exception)
			{
				throw;
			}
		}

		private int SetDUTCount(string passCountStr, string totalCountStr)
		{
			try
			{
				IniFile seqIniFile = new IniFile();
				seqIniFile.IniWriteValue("DUTCount_" + SelectedSeqIndex, "PassCount", passCountStr);
				seqIniFile.IniWriteValue("DUTCount_" + SelectedSeqIndex, "TotalCount", totalCountStr);
				return 0;
			}
			catch (Exception innerException)
			{
				throw new Exception("", innerException);
			}
		}

		private int GetTestedDUTCount()
		{
			try
			{
				IniFile seqIniFile = new IniFile();
				PassDUTCount = int.Parse(seqIniFile.IniReadValue("DUTCount_" + SelectedSeqIndex, "PassCount"));
				TotalDUTCount = int.Parse(seqIniFile.IniReadValue("DUTCount_" + SelectedSeqIndex, "TotalCount"));
				label_Total.Text = TotalDUTCount.ToString();
				label_Pass.Text = PassDUTCount.ToString();
				label_Fail.Text = (TotalDUTCount - PassDUTCount).ToString();
				label_PassRate.Text = Math.Round((double)PassDUTCount * 1.0 / (double)TotalDUTCount * 100.0, 1).ToString();
				return 0;
			}
			catch (Exception innerException)
			{
				throw new Exception("", innerException);
			}
		}

		private int ResetDUTCount()
		{
			try
			{
				TotalDUTCount = 0;
				PassDUTCount = 0;
				IniFile seqIniFile = new IniFile();
				seqIniFile.IniWriteValue("DUTCount_" + SelectedSeqIndex, "PassCount", "0");
				seqIniFile.IniWriteValue("DUTCount_" + SelectedSeqIndex, "TotalCount", "0");
				label_Total.Text = "0";
				label_Pass.Text = "0";
				label_Fail.Text = "0";
				label_PassRate.Text = "0";
				return 0;
			}
			catch (Exception innerException)
			{
				throw new Exception("", innerException);
			}
		}

		private void LoadExeInterface(string exePath)
		{
			try
			{
				if (!(exePath == "") && File.Exists(exePath))
				{
					Process process = new Process();
					process.StartInfo.FileName = exePath;
					process.StartInfo.WorkingDirectory = Path.GetDirectoryName(exePath);
					process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
					process.Start();
					System.Threading.Thread.Sleep(100);
					SetWindowPos(process.MainWindowHandle, IntPtr.Zero, panel_Exe.Location.X, panel_Exe.Location.Y, panel_Exe.Width, panel_Exe.Height, 0);
					SetParent(process.MainWindowHandle, panel_Exe.Handle);
					ShowWindow(process.MainWindowHandle, 3);
					long style = GetWindowLong(process.MainWindowHandle, -16);
					style &= -12582913;
					SetWindowLong(process.MainWindowHandle, -16, style);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void timer_Main_Tick(object sender, EventArgs e)
		{
			try
			{
				label_Date.Text = DateTime.Now.ToString("yyyy-MM-dd");
				label_Time.Text = DateTime.Now.ToString("HH:mm:ss");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void listView_ResultList_SizeChanged(object sender, EventArgs e)
		{
		}

		private void skinButton_ResetCount_Click(object sender, EventArgs e)
		{
			try
			{
				ResetDUTCount();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!IsExitApp)
			{
				e.Cancel = true;
			}
			else if (!SessionEnding)
			{
				axApplicationMgr.ExitApplication += axApplicationMgr_ExitApplication;
				if (!axApplicationMgr.Shutdown())
				{
					e.Cancel = true;
				}
			}
			else
			{
				GC.Collect();
			}
		}

		private void axApplicationMgr_ExitApplication(object sender, EventArgs e)
		{
			SessionEnding = true;
			Environment.ExitCode = axApplicationMgr.ExitCode;
			Application.Exit();
			TSHelper.DoSynchronousGCForCOMObjectDestruction();
			Process.GetCurrentProcess().Kill();
		}

		private void loginOutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				Command appCMD = (Command)axApplicationMgr.GetCommand(CommandKinds.CommandKind_Login);
				appCMD.Execute(applicationHandlesError: true);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				IsExitApp = true;
				NextAction = NextActionAfterEndExecution.ExitApplication;
				if (((Executions)axApplicationMgr.Executions).NumIncomplete > 0)
				{
					SendRequestToModelSequence("Stop All TestSockets", -1);
					SendRequestToModelSequence("Exit", -1);
				}
				else
				{
					Application.Exit();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void NextActionThread()
		{
			try
			{
				switch (NextAction)
				{
				case NextActionAfterEndExecution.ReloadSequence:
					Invoke((EventHandler)delegate
					{
						LaunchSequenceByIndex(SelectedSeqIndex);
					});
					break;
				case NextActionAfterEndExecution.ExitApplication:
					System.Threading.Thread.Sleep(200);
					Application.Exit();
					break;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void axExecutionViewMgr_TerminationStateChanged(object sender, _ExecutionViewMgrEvents_TerminationStateChangedEvent e)
		{
			if (e.newTermState != ExecutionTerminationStates.ExecTermState_Terminating)
			{
			}
		}

		private void selectSequenceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (0 == LoadSequenceListToMemory(enableDefaultSequence: false))
				{
					NextAction = NextActionAfterEndExecution.ReloadSequence;
					if (((Executions)axApplicationMgr.Executions).NumIncomplete > 0)
					{
						SendRequestToModelSequence("Stop All TestSockets", -1);
						SendRequestToModelSequence("Exit", -1);
					}
					else
					{
						LaunchSequenceByIndex(SelectedSeqIndex);
					}
					GetTestedDUTCount();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void closeSequenceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (axExecutionViewMgr.Execution != null && ((Execution)axExecutionViewMgr.Execution).NumThreads > 0)
				{
					SendRequestToModelSequence("Stop All TestSockets", -1);
					UUTSeqContextList.Clear();
					tableLayoutPanel_DUTList.Controls.Clear();
					pictureBox_Logo.Image = null;
					TextBox_Station.Text = "";
					TextBox_Project.Text = "";
					TextBox_Version.Text = "";
					TextBox_Sequence.Text = "";
					ExecutionIsEnd = false;
				}
				TSHelper.DoSynchronousGCForCOMObjectDestruction();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void reTestDUTToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				ResetDUTCount();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void textBox_SerialNumber_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if (textBox_SerialNumber.Text != "" && textBox_SerialNumber.Text.Length >= SeqSettingList[SelectedSeqIndex].SerialNumberLen)
				{
					SendRequestToModelSequence("SerialNumber_" + textBox_SerialNumber.Text, -1);
				}
			}
			catch
			{
			}
		}

		private void mESToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ModelTopSeqContext != null)
			{
				if (!mESToolStripMenuItem.Checked)
				{
					mESToolStripMenuItem.Checked = true;
					label_MES.Text = "ON";
					label_MES.BackColor = Color.Green;
					ModelTopSeqContext.AsPropertyObject().SetValBoolean("Locals.ModelData.StationInfo.AdditionalData.MES_Enable", 1, newValue: true);
				}
				else
				{
					mESToolStripMenuItem.Checked = false;
					label_MES.Text = "OFF";
					label_MES.BackColor = Color.Yellow;
					ModelTopSeqContext.AsPropertyObject().SetValBoolean("Locals.ModelData.StationInfo.AdditionalData.MES_Enable", 1, newValue: false);
				}
			}
		}

		private void languageToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		private void stationOptionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Command cmd = (Command)axExecutionViewMgr.GetCommand(CommandKinds.CommandKind_ConfigureStationOptions);
			cmd.Execute(applicationHandlesError: false);
		}

		private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
		{
		}

		private void instrumentsSourceToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

        private void Label_MES_Click(object sender, EventArgs e)
        {

        }
    }
}
