using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using FTP2017.Properties;

namespace FTP2017
{
	[Serializable]
	public class CleanupUI : Form, ISerializable
	{
		private int TimerCount = 0;

		private static string IniFilePath = Application.StartupPath + "\\Config\\StationConfig.cfg";

		private static object LockedObj = new object();

		private IContainer components = null;

		private Timer timer1;

		private Label label1;

		private Label label2;

		private Label label3;

		public CleanupUI()
		{
			InitializeComponent();
		}

		public CleanupUI(SerializationInfo info, StreamingContext context)
		{
			base.Name = info.GetString("Name ");
			base.Size = (Size)info.GetValue("Size ", typeof(Size));
			base.Location = (Point)info.GetValue("Location ", typeof(Point));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Name ", base.Name);
			info.AddValue("Size ", base.Size);
			info.AddValue("Location ", base.Location);
		}

		public static void CloseMe()
		{
			IniWriteValue("Config", "SplashScreenFlag", "0");
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			TimerCount++;
			if (TimerCount >= 300)
			{
				Close();
			}
			if ("0" == IniReadValue("Config", "SplashScreenFlag"))
			{
				Close();
			}
		}

		private void StartupUI_Load(object sender, EventArgs e)
		{
			IniWriteValue("Config", "SplashScreenFlag", "1");
		}

		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

		[DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
		private static extern uint GetPrivateProfileStringA(string section, string key, string def, byte[] retVal, int size, string filePath);

		private static void IniWriteValue(string Section, string Key, string Value)
		{
			lock (LockedObj)
			{
				WritePrivateProfileString(Section, Key, Value, IniFilePath);
			}
		}

		private static string IniReadValue(string Section, string Key)
		{
			lock (LockedObj)
			{
				StringBuilder temp = new StringBuilder(500);
				int i = GetPrivateProfileString(Section, Key, "", temp, 500, IniFilePath);
				return temp.ToString();
			}
		}

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
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			base.SuspendLayout();
			this.timer1.Enabled = true;
			this.timer1.Tick += new System.EventHandler(timer1_Tick);
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 27.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
			this.label1.Location = new System.Drawing.Point(177, 76);
			this.label1.Margin = new System.Windows.Forms.Padding(3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(230, 50);
			this.label1.TabIndex = 0;
			this.label1.Text = "Please wait";
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 18f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
			this.label2.Location = new System.Drawing.Point(180, 132);
			this.label2.Margin = new System.Windows.Forms.Padding(3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(198, 31);
			this.label2.TabIndex = 0;
			this.label2.Text = "FTP Exiting ......";
			this.label3.Image = FTP2017.Properties.Resources.Loading;
			this.label3.Location = new System.Drawing.Point(35, 59);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(126, 120);
			this.label3.TabIndex = 1;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Khaki;
			base.ClientSize = new System.Drawing.Size(464, 261);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "CleanupUI";
			base.Opacity = 0.0;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "StartupUI";
			base.Load += new System.EventHandler(StartupUI_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
