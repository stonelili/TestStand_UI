using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using MessageStruct;

namespace FTP2017
{
	public class DUT_SocketUI : UserControl
	{
		private DateTime StartDateTime = DateTime.Now;

		private object LockObj_listview = new object();

		private int CurrentStepIndex = 0;

		private List<StepResultStruct> StepRsultList = new List<StepResultStruct>(8192);

		private int _SocketIndex = 0;

		private int _TotalStepCount = 0;

		private SequenceSetting _DUTSetting = new SequenceSetting();

		private int _CircleTime = 0;

		private string _SequencePath = "";

		private string _LogfileFullPath = "";

		private string _SerialNumber = "";

		private bool _IsRunning = false;

		private bool _StartBtnEnable = true;

		private double firstStepStartTime = 0.0;

		private IContainer components = null;

		private GroupBox groupBox_DUT;

		private Label label_CircleTime;

		private Label label_Status;

		private Button button_Stop;

		private Button button_Start;

		private Timer timer_CircleTime;

		private ImageList imageList_debug;

		private ContextMenuStrip contextMenuStrip_StepDebug;

		private ToolStripMenuItem goToStepToolStripMenuItem;

		private ToolStripMenuItem skipToolStripMenuItem;

		private ToolStripMenuItem breakpointToolStripMenuItem;

		private ColumnHeader columnHeader1;

		private ColumnHeader columnHeader6;

		private ProgressBar progressBar_Step;

		private DoubleBufferDataGridView dataGridView_ResultList;

		private TableLayoutPanel tableLayoutPanel1;

		private TableLayoutPanel tableLayoutPanel2;

		private Panel panel2;

		private Panel panel1;

		private Panel panel3;

		private DataGridViewTextBoxColumn Column1;

		private DataGridViewTextBoxColumn Column2;

		private DataGridViewTextBoxColumn Column3;

		private DataGridViewTextBoxColumn Column4;

		public int SocketIndex
		{
			get
			{
				return _SocketIndex;
			}
			set
			{
				_SocketIndex = value;
				groupBox_DUT.Text = "DUT " + _SocketIndex;
			}
		}

		public int TotalStepCount
		{
			get
			{
				return _TotalStepCount;
			}
			set
			{
				_TotalStepCount = value;
			}
		}

		public SequenceSetting DUTSetting
		{
			get
			{
				return _DUTSetting;
			}
			set
			{
				_DUTSetting = value;
			}
		}

		public int CircleTime
		{
			get
			{
				return _CircleTime;
			}
			set
			{
				_CircleTime = value;
				label_CircleTime.Text = _CircleTime + " S";
			}
		}

		public string SequencePath
		{
			get
			{
				return _SequencePath;
			}
			set
			{
				_SequencePath = value;
			}
		}

		public string LogfileFullPath
		{
			get
			{
				return _LogfileFullPath;
			}
			set
			{
				_LogfileFullPath = value;
			}
		}

		public string SerialNumber
		{
			get
			{
				return _SerialNumber;
			}
			set
			{
				_SerialNumber = value;
			}
		}

		public bool IsRunning
		{
			get
			{
				return _IsRunning;
			}
			set
			{
				_IsRunning = value;
			}
		}

		public bool StartBtnEnable
		{
			get
			{
				return _StartBtnEnable;
			}
			set
			{
				_StartBtnEnable = value;
			}
		}

		public string TestStatusLable
		{
			get
			{
				return label_Status.Text;
			}
			set
			{
				label_Status.Text = value;
			}
		}

		public event StartStopButtonClick StartButtonClicked;

		public event StartStopButtonClick StopButtonClicked;

		public event StartStopButtonClick DutIndexChanged;

		public DUT_SocketUI()
		{
			InitializeComponent();
		}

		public DUT_SocketUI(int socketIndex)
		{
			InitializeComponent();
			SocketIndex = socketIndex;
		}

		public void BeginTesting()
		{
			try
			{
				label_Status.Text = _SerialNumber + " Testing";
				StepRsultList.Clear();
				dataGridView_ResultList.Rows.Clear();
				button_Start.Enabled = false;
				button_Stop.Enabled = true;
				_CircleTime = 0;
				timer_CircleTime.Enabled = true;
				label_Status.ForeColor = Color.White;
				StartDateTime = DateTime.Now;
				CurrentStepIndex = 0;
				progressBar_Step.ForeColor = Color.Blue;
				progressBar_Step.Maximum = TotalStepCount;
				progressBar_Step.Value = CurrentStepIndex;
				_IsRunning = true;
			}
			catch (Exception innerException)
			{
				throw new Exception("", innerException);
			}
		}

		public void TestingComplete(string resultStatus)
		{
			try
			{
				label_Status.Text = _SerialNumber + " " + resultStatus;
				timer_CircleTime.Enabled = false;
				progressBar_Step.Maximum = CurrentStepIndex;
				progressBar_Step.Value = CurrentStepIndex;
				if (_StartBtnEnable)
				{
					button_Start.Enabled = true;
				}
				button_Stop.Enabled = false;
				if (resultStatus.ToLower().Contains("pass"))
				{
					TotalStepCount = CurrentStepIndex;
					label_Status.ForeColor = Color.Lime;
					progressBar_Step.ForeColor = Color.Lime;
				}
				else
				{
					label_Status.ForeColor = Color.Red;
					progressBar_Step.ForeColor = Color.Red;
				}
				_IsRunning = false;
				_SerialNumber = "";
			}
			catch (Exception innerException)
			{
				throw new Exception("", innerException);
			}
		}

		public void Initializing()
		{
			try
			{
				label_Status.Text = "Initializing";
				button_Start.Enabled = false;
			}
			catch (Exception innerException)
			{
				throw new Exception("", innerException);
			}
		}

		public void InitializFinished(string resultString)
		{
			try
			{
				label_Status.Text = resultString;
				if (resultString == "Ready" && _StartBtnEnable)
				{
					button_Start.Enabled = true;
				}
				dataGridView_ResultList.Rows.Clear();
			}
			catch (Exception innerException)
			{
				throw new Exception("", innerException);
			}
		}

		public void StopTest()
		{
			try
			{
				timer_CircleTime.Enabled = false;
				if (progressBar_Step.ForeColor == Color.Blue)
				{
					progressBar_Step.ForeColor = Color.Lime;
				}
			}
			catch (Exception innerException)
			{
				throw new Exception("", innerException);
			}
		}

		public void SetSartBtnEnable(bool isEnable)
		{
			try
			{
				if (!button_Stop.Enabled)
				{
					_StartBtnEnable = isEnable;
					button_Start.Enabled = isEnable;
				}
			}
			catch (Exception innerException)
			{
				throw new Exception("", innerException);
			}
		}

		public void Update_listView_ResultList(StepResultStruct stepResult)
		{
			try
			{
				if (stepResult.Status.ToLower() == "skipped")
				{
					return;
				}
				if (StepRsultList.Count == 0)
				{
					firstStepStartTime = stepResult.StartTime;
				}
				if (_DUTSetting.StepDisplayType.ToLower() == "all")
				{
					stepResult.TotalTime = stepResult.TotalTime;
				}
				else
				{
					stepResult.TotalTime = stepResult.StartTime - firstStepStartTime + stepResult.TotalTime;
				}
				StepRsultList.Add(stepResult);
				for (int i_StepResult = CurrentStepIndex; i_StepResult < StepRsultList.Count; i_StepResult++)
				{
					CurrentStepIndex++;
					stepResult = StepRsultList[i_StepResult];
					if (CurrentStepIndex <= progressBar_Step.Maximum)
					{
						progressBar_Step.Value = CurrentStepIndex;
					}
					string[] rowData = new string[4];
					if (_DUTSetting.StepDisplayType.ToLower() == "all")
					{
						rowData[0] = $"{dataGridView_ResultList.Rows.Count + 1:d3} {stepResult.StepName}";
					}
					else
					{
						rowData[0] = stepResult.StepName;
					}
					switch (stepResult.StepType)
					{
					case "NumericLimitTest":
						rowData[1] = GetExpriceValue(stepResult.Comp, stepResult.LimitsLow, stepResult.LimitsHigh);
						rowData[2] = $"x={stepResult.NumericValue} {stepResult.Unit}";
						break;
					case "PassFailTest":
						rowData[1] = "x=True";
						rowData[2] = $"x={stepResult.PassFailValue}";
						break;
					case "StringValueTest":
						rowData[1] = $"x=\"{stepResult.LimitsString}\"";
						rowData[2] = $"x=\"{stepResult.StringValue}\"";
						break;
					default:
						rowData[1] = "";
						rowData[2] = "";
						break;
					}
					rowData[3] = stepResult.Status;
					Color rowForeColor = default(Color);
					if (stepResult.Status == "Failed" || stepResult.Status == "Error")
					{
						rowForeColor = Color.Red;
					}
					else
					{
						rowForeColor = Color.Navy;
					}
					int rowNum = 0;
					if (_DUTSetting.StepDisplayType == "MeasStep")
					{
						if (stepResult.StepType == "StringValueTest" || stepResult.StepType == "PassFailTest" || stepResult.StepType == "NumericLimitTest")
						{
							rowNum = dataGridView_ResultList.Rows.Add(rowData);
							if (stepResult.Status == "Failed" || stepResult.Status == "Error")
							{
								dataGridView_ResultList.Rows[rowNum].Cells[0].Style.ForeColor = Color.Red;
								dataGridView_ResultList.Rows[rowNum].Cells[1].Style.ForeColor = Color.Red;
								dataGridView_ResultList.Rows[rowNum].Cells[2].Style.ForeColor = Color.Red;
								dataGridView_ResultList.Rows[rowNum].Cells[3].Style.ForeColor = Color.Red;
							}
						}
					}
					else if (_DUTSetting.StepDisplayType == "FailStep")
					{
						if (stepResult.Status == "Failed" && (stepResult.StepType == "StringValueTest" || stepResult.StepType == "PassFailTest" || stepResult.StepType == "NumericLimitTest"))
						{
							rowNum = dataGridView_ResultList.Rows.Add(rowData);
							if (stepResult.Status == "Failed" || stepResult.Status == "Error")
							{
								dataGridView_ResultList.Rows[rowNum].Cells[0].Style.ForeColor = Color.Red;
								dataGridView_ResultList.Rows[rowNum].Cells[1].Style.ForeColor = Color.Red;
								dataGridView_ResultList.Rows[rowNum].Cells[2].Style.ForeColor = Color.Red;
								dataGridView_ResultList.Rows[rowNum].Cells[3].Style.ForeColor = Color.Red;
							}
						}
					}
					else if (_DUTSetting.StepDisplayType.ToLower() == "all")
					{
						if (stepResult.StepType == "StringValueTest" || stepResult.StepType == "PassFailTest" || stepResult.StepType == "NumericLimitTest" || stepResult.StepType == "Action" || stepResult.StepType == "NI_Wait" || stepResult.StepType == "Statement")
						{
							rowNum = dataGridView_ResultList.Rows.Add(rowData);
							if (stepResult.Status == "Failed" || stepResult.Status == "Error")
							{
								dataGridView_ResultList.Rows[rowNum].Cells[0].Style.ForeColor = Color.Red;
								dataGridView_ResultList.Rows[rowNum].Cells[1].Style.ForeColor = Color.Red;
								dataGridView_ResultList.Rows[rowNum].Cells[2].Style.ForeColor = Color.Red;
								dataGridView_ResultList.Rows[rowNum].Cells[3].Style.ForeColor = Color.Red;
							}
						}
					}
					else
					{
						rowNum = dataGridView_ResultList.Rows.Add(rowData);
						if (stepResult.Status == "Failed" || stepResult.Status == "Error")
						{
							dataGridView_ResultList.Rows[rowNum].Cells[0].Style.ForeColor = Color.Red;
							dataGridView_ResultList.Rows[rowNum].Cells[1].Style.ForeColor = Color.Red;
							dataGridView_ResultList.Rows[rowNum].Cells[2].Style.ForeColor = Color.Red;
							dataGridView_ResultList.Rows[rowNum].Cells[3].Style.ForeColor = Color.Red;
						}
					}
					int rowCount = dataGridView_ResultList.DisplayedRowCount(includePartialRow: true);
					if (rowNum + 1 > rowCount && rowCount > 0)
					{
						dataGridView_ResultList.FirstDisplayedScrollingRowIndex = rowNum + 2 - rowCount;
					}
				}
			}
			catch (Exception innerException)
			{
				throw new Exception("", innerException);
			}
		}

		private void MySeqOperation_CurrentStepIndexChanged(int socketIndex, int stepIndex)
		{
			try
			{
				Invoke((EventHandler)delegate
				{
					lock (LockObj_listview)
					{
					}
				});
			}
			catch (Exception)
			{
				throw;
			}
		}

		private void SaveLogfile(string resultStatus)
		{
			try
			{
				string log = "";
				string folderPath = "";
				string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
				folderPath = Path.Combine(_DUTSetting.LogFilePath, datetime.Substring(0, 8));
				if (!Directory.Exists(folderPath))
				{
					Directory.CreateDirectory(folderPath);
				}
				_LogfileFullPath = $"{folderPath}\\{_DUTSetting.ProjectName}_{_DUTSetting.StationName}_{SerialNumber}_{datetime}_{_SocketIndex}.csv";
				StreamWriter sw = new StreamWriter(_LogfileFullPath);
				sw.WriteLine("Product Name," + _DUTSetting.ProjectName);
				sw.WriteLine("Stationt Name," + _DUTSetting.StationName);
				sw.WriteLine("Serial Number," + _SerialNumber);
				sw.WriteLine("Total Test Time," + label_CircleTime.Text);
				sw.WriteLine("DUTResult," + resultStatus);
				sw.WriteLine("Date Time," + datetime);
				sw.WriteLine("Stepname,MeasureValue,Unit,Lowlimit,Highlimit,Result,TotalTime");
				int stepIndex = 0;
				foreach (StepResultStruct stepResult in StepRsultList)
				{
					switch (stepResult.StepType)
					{
					case "NumericLimitTest":
						switch (stepResult.Comp)
						{
						case "EQ":
						case "NE":
							log = $"{stepResult.StepName},{stepResult.NumericValue},{stepResult.Unit},{stepResult.LimitsLow},{stepResult.LimitsLow},{stepResult.Status},{stepResult.TotalTime}";
							break;
						case "GT":
						case "GE":
							log = string.Format("{0},{1},{2},{3},{4},{5},{6}", stepResult.StepName, stepResult.NumericValue, stepResult.Unit, stepResult.LimitsLow, "NA", stepResult.Status, stepResult.TotalTime);
							break;
						case "LT":
						case "LE":
							log = string.Format("{0},{1},{2},{3},{4},{5},{6}", stepResult.StepName, stepResult.NumericValue, stepResult.Unit, "NA", stepResult.LimitsLow, stepResult.Status, stepResult.TotalTime);
							break;
						case "GTLT":
						case "GELE":
						case "GELT":
						case "GTLE":
						case "LTGT":
						case "LEGE":
						case "LEGT":
						case "LTGE":
							log = $"{stepResult.StepName},{stepResult.NumericValue},{stepResult.Unit},{stepResult.LimitsLow},{stepResult.LimitsHigh},{stepResult.Status},{stepResult.TotalTime}";
							break;
						default:
							log = $"{stepResult.StepName},{stepResult.NumericValue},{stepResult.Unit},{stepResult.LimitsLow},{stepResult.LimitsHigh},{stepResult.Status},{stepResult.TotalTime}";
							break;
						}
						break;
					case "PassFailTest":
						log = string.Format("{0},{1},{2},{3},{4},{5},{6}", stepResult.StepName, stepResult.PassFailValue, "", "True", "True", stepResult.Status, stepResult.TotalTime);
						break;
					case "StringValueTest":
						log = string.Format("{0},{1},{2},{3},{4},{5},{6}", stepResult.StepName, stepResult.StringValue, "", stepResult.LimitsString, stepResult.LimitsString, stepResult.Status, stepResult.TotalTime);
						break;
					default:
						log = "";
						break;
					}
					stepIndex++;
					if (log != "")
					{
						if (stepIndex == StepRsultList.Count)
						{
							sw.Write(log);
						}
						else
						{
							sw.WriteLine(log);
						}
					}
				}
				sw.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Save log file error:    " + ex.ToString());
			}
		}

		private void timer_CircleTime_Tick(object sender, EventArgs e)
		{
			CircleTime = (int)(DateTime.Now - StartDateTime).TotalSeconds;
		}

		private void button_Start_Click(object sender, EventArgs e)
		{
			if (this.StartButtonClicked != null)
			{
				this.StartButtonClicked(SocketIndex);
			}
		}

		private void dataGridView_ResultList_SizeChanged(object sender, EventArgs e)
		{
			float sizeRate = (float)(dataGridView_ResultList.Width * dataGridView_ResultList.Height) / 263736f;
			sizeRate = 1f + (sizeRate - 1f) / 5f;
			Font f = new Font(dataGridView_ResultList.Font.FontFamily, sizeRate * 1.016f * 9f, FontStyle.Bold);
			dataGridView_ResultList.Font = f;
			dataGridView_ResultList.RowTemplate.DefaultCellStyle.Font = f;
		}

		private void label_Status_TextChanged(object sender, EventArgs e)
		{
			try
			{
				float sizeRate = (float)(dataGridView_ResultList.Width * dataGridView_ResultList.Height) / 172752f;
				sizeRate = 1f + (sizeRate - 1f) / 5f;
				Font f = new Font(label_Status.Font.FontFamily, sizeRate * 1.06f * 12f);
				Graphics g = label_Status.CreateGraphics();
				SizeF StrSize = g.MeasureString(label_Status.Text, f);
				int width = (int)StrSize.Width;
				int height = (int)StrSize.Height;
				if (width > label_Status.Width - 5)
				{
					int newFontSize = 20;
					for (newFontSize = 20; newFontSize > 8; newFontSize--)
					{
						f = new Font(label_Status.Font.FontFamily, newFontSize);
						SizeF _StrSize = g.MeasureString(label_Status.Text, f);
						if (_StrSize.Height * 2f < (float)label_Status.Height || _StrSize.Width < (float)label_Status.Width)
						{
							break;
						}
					}
					f = new Font(label_Status.Font.FontFamily, newFontSize);
					label_Status.Font = f;
				}
				else
				{
					f = new Font(label_Status.Font.FontFamily, sizeRate * 1.06f * 12f);
					label_Status.Font = f;
				}
			}
			catch (Exception innerException)
			{
				throw new Exception("", innerException);
			}
		}

		private void button_Stop_Click(object sender, EventArgs e)
		{
			if (this.StopButtonClicked != null)
			{
				button_Stop.Enabled = false;
				this.StopButtonClicked(SocketIndex);
			}
		}

		public T MyClone<T>(T RealObject)
		{
			Stream objectStream = new MemoryStream();
			IFormatter formatter = new BinaryFormatter();
			formatter.Serialize(objectStream, RealObject);
			objectStream.Seek(0L, SeekOrigin.Begin);
			return (T)formatter.Deserialize(objectStream);
		}

		private void listView_ResultList_Click(object sender, EventArgs e)
		{
			if (this.DutIndexChanged != null)
			{
				this.DutIndexChanged(_SocketIndex);
			}
		}

		private Image pictureProcess(Image sourceImage, int targetWidth, int targetHeight)
		{
			try
			{
				ImageFormat format = sourceImage.RawFormat;
				Bitmap targetPicture = new Bitmap(targetWidth, targetHeight);
				Graphics g = Graphics.FromImage(targetPicture);
				g.Clear(Color.White);
				int width;
				int height;
				if (sourceImage.Width > targetWidth && sourceImage.Height <= targetHeight)
				{
					width = targetWidth;
					height = width * sourceImage.Height / sourceImage.Width;
				}
				else if (sourceImage.Width <= targetWidth && sourceImage.Height > targetHeight)
				{
					height = targetHeight;
					width = height * sourceImage.Width / sourceImage.Height;
				}
				else if (sourceImage.Width <= targetWidth && sourceImage.Height <= targetHeight)
				{
					width = sourceImage.Width;
					height = sourceImage.Height;
				}
				else
				{
					width = targetWidth;
					height = width * sourceImage.Height / sourceImage.Width;
					if (height > targetHeight)
					{
						height = targetHeight;
						width = height * sourceImage.Width / sourceImage.Height;
					}
				}
				g.DrawImage(sourceImage, (targetWidth - width) / 2, (targetHeight - height) / 2, width, height);
				sourceImage.Dispose();
				return targetPicture;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			return null;
		}

		private string GetExpriceValue(string type, double low, double high)
		{
			string result = "";
			switch (type)
			{
			case "EQ":
				result = $"x={low}";
				break;
			case "NE":
				result = $"x!={low}";
				break;
			case "GT":
				result = $"x>{low}";
				break;
			case "LT":
				result = $"x<{low}";
				break;
			case "GE":
				result = $"x>={low}";
				break;
			case "LE":
				result = $"x<={low}";
				break;
			case "GTLT":
				result = $"{low}<x<{high}";
				break;
			case "GELE":
				result = $"{low}<=x<={high}";
				break;
			case "GELT":
				result = $"{low}<=x<{high}";
				break;
			case "GTLE":
				result = $"{low}<x<={high}";
				break;
			case "LTGT":
				result = $"x<{low} || x>{high}";
				break;
			case "LEGE":
				result = $"x<={low} || x>={high}";
				break;
			case "LEGT":
				result = $"x<={low} || x>{high}";
				break;
			case "LTGE":
				result = $"x<{low} || x>={high}";
				break;
			}
			return result;
		}

		private void dataGridView_ResultList_Scroll(object sender, ScrollEventArgs e)
		{
			if (e.OldValue != 0)
			{
			}
		}

		private void dataGridView_ResultList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FTP2017.DUT_SocketUI));
			this.groupBox_DUT = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.dataGridView_ResultList = new FTP2017.DoubleBufferDataGridView();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.button_Stop = new System.Windows.Forms.Button();
			this.label_Status = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.progressBar_Step = new System.Windows.Forms.ProgressBar();
			this.panel3 = new System.Windows.Forms.Panel();
			this.button_Start = new System.Windows.Forms.Button();
			this.label_CircleTime = new System.Windows.Forms.Label();
			this.contextMenuStrip_StepDebug = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.goToStepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.skipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.breakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.imageList_debug = new System.Windows.Forms.ImageList(this.components);
			this.timer_CircleTime = new System.Windows.Forms.Timer(this.components);
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.groupBox_DUT.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dataGridView_ResultList).BeginInit();
			this.tableLayoutPanel2.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.contextMenuStrip_StepDebug.SuspendLayout();
			base.SuspendLayout();
			this.groupBox_DUT.BackColor = System.Drawing.Color.FromArgb(98, 137, 199);
			this.groupBox_DUT.Controls.Add(this.tableLayoutPanel1);
			this.groupBox_DUT.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox_DUT.Location = new System.Drawing.Point(0, 0);
			this.groupBox_DUT.Name = "groupBox_DUT";
			this.groupBox_DUT.Size = new System.Drawing.Size(666, 396);
			this.groupBox_DUT.TabIndex = 6;
			this.groupBox_DUT.TabStop = false;
			this.groupBox_DUT.Text = "DUT 0";
			this.groupBox_DUT.TextChanged += new System.EventHandler(label_Status_TextChanged);
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
			this.tableLayoutPanel1.Controls.Add(this.dataGridView_ResultList, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 17);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14f));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86f));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(660, 376);
			this.tableLayoutPanel1.TabIndex = 14;
			this.dataGridView_ResultList.AllowUserToAddRows = false;
			this.dataGridView_ResultList.AllowUserToDeleteRows = false;
			this.dataGridView_ResultList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dataGridView_ResultList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.dataGridView_ResultList.BackgroundColor = System.Drawing.SystemColors.Control;
			this.dataGridView_ResultList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.dataGridView_ResultList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
			this.dataGridView_ResultList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridView_ResultList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dataGridView_ResultList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView_ResultList.Columns.AddRange(this.Column1, this.Column2, this.Column3, this.Column4);
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 20.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle2.ForeColor = System.Drawing.Color.MidnightBlue;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.MidnightBlue;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridView_ResultList.DefaultCellStyle = dataGridViewCellStyle2;
			this.dataGridView_ResultList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView_ResultList.GridColor = System.Drawing.SystemColors.ControlLight;
			this.dataGridView_ResultList.Location = new System.Drawing.Point(3, 58);
			this.dataGridView_ResultList.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.dataGridView_ResultList.Name = "dataGridView_ResultList";
			this.dataGridView_ResultList.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("SimSun", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			this.dataGridView_ResultList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dataGridView_ResultList.RowHeadersVisible = false;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("SimSun", 20.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
			this.dataGridView_ResultList.RowsDefaultCellStyle = dataGridViewCellStyle4;
			this.dataGridView_ResultList.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("SimSun", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
			this.dataGridView_ResultList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Navy;
			this.dataGridView_ResultList.RowTemplate.Height = 23;
			this.dataGridView_ResultList.RowTemplate.ReadOnly = true;
			this.dataGridView_ResultList.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView_ResultList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.dataGridView_ResultList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView_ResultList.Size = new System.Drawing.Size(654, 315);
			this.dataGridView_ResultList.TabIndex = 13;
			this.dataGridView_ResultList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(dataGridView_ResultList_CellFormatting);
			this.dataGridView_ResultList.Scroll += new System.Windows.Forms.ScrollEventHandler(dataGridView_ResultList_Scroll);
			this.dataGridView_ResultList.SizeChanged += new System.EventHandler(dataGridView_ResultList_SizeChanged);
			this.tableLayoutPanel2.ColumnCount = 4;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5f));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5f));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.46154f));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.53846f));
			this.tableLayoutPanel2.Controls.Add(this.panel2, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.label_Status, 2, 0);
			this.tableLayoutPanel2.Controls.Add(this.panel1, 2, 1);
			this.tableLayoutPanel2.Controls.Add(this.panel3, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.label_CircleTime, 3, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41f));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 59f));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(660, 52);
			this.tableLayoutPanel2.TabIndex = 14;
			this.panel2.Controls.Add(this.button_Stop);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(82, 0);
			this.panel2.Margin = new System.Windows.Forms.Padding(0);
			this.panel2.Name = "panel2";
			this.tableLayoutPanel2.SetRowSpan(this.panel2, 2);
			this.panel2.Size = new System.Drawing.Size(82, 52);
			this.panel2.TabIndex = 15;
			this.button_Stop.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.button_Stop.BackColor = System.Drawing.Color.Transparent;
			this.button_Stop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.button_Stop.Enabled = false;
			this.button_Stop.Font = new System.Drawing.Font("Microsoft YaHei", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_Stop.Location = new System.Drawing.Point(2, 10);
			this.button_Stop.Name = "button_Stop";
			this.button_Stop.Size = new System.Drawing.Size(77, 33);
			this.button_Stop.TabIndex = 6;
			this.button_Stop.Text = "Stop";
			this.button_Stop.UseVisualStyleBackColor = false;
			this.button_Stop.Click += new System.EventHandler(button_Stop_Click);
			this.label_Status.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label_Status.Font = new System.Drawing.Font("SimSun", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
			this.label_Status.ForeColor = System.Drawing.Color.White;
			this.label_Status.Location = new System.Drawing.Point(167, 0);
			this.label_Status.Name = "label_Status";
			this.label_Status.Size = new System.Drawing.Size(412, 21);
			this.label_Status.TabIndex = 10;
			this.label_Status.Text = "Initializing";
			this.label_Status.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.label_Status.TextChanged += new System.EventHandler(label_Status_TextChanged);
			this.tableLayoutPanel2.SetColumnSpan(this.panel1, 2);
			this.panel1.Controls.Add(this.progressBar_Step);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(164, 21);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(496, 31);
			this.panel1.TabIndex = 0;
			this.progressBar_Step.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.progressBar_Step.BackColor = System.Drawing.Color.White;
			this.progressBar_Step.Location = new System.Drawing.Point(2, 1);
			this.progressBar_Step.Name = "progressBar_Step";
			this.progressBar_Step.Size = new System.Drawing.Size(491, 20);
			this.progressBar_Step.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBar_Step.TabIndex = 12;
			this.panel3.Controls.Add(this.button_Start);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Margin = new System.Windows.Forms.Padding(0);
			this.panel3.Name = "panel3";
			this.tableLayoutPanel2.SetRowSpan(this.panel3, 2);
			this.panel3.Size = new System.Drawing.Size(82, 52);
			this.panel3.TabIndex = 15;
			this.button_Start.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			this.button_Start.BackColor = System.Drawing.Color.Transparent;
			this.button_Start.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.button_Start.Enabled = false;
			this.button_Start.Font = new System.Drawing.Font("Microsoft YaHei", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.button_Start.ImageIndex = 3;
			this.button_Start.Location = new System.Drawing.Point(2, 10);
			this.button_Start.Name = "button_Start";
			this.button_Start.Size = new System.Drawing.Size(77, 33);
			this.button_Start.TabIndex = 7;
			this.button_Start.Text = "Start";
			this.button_Start.UseVisualStyleBackColor = true;
			this.button_Start.Click += new System.EventHandler(button_Start_Click);
			this.label_CircleTime.AutoSize = true;
			this.label_CircleTime.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label_CircleTime.Font = new System.Drawing.Font("SimSun", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
			this.label_CircleTime.ForeColor = System.Drawing.Color.White;
			this.label_CircleTime.Location = new System.Drawing.Point(585, 0);
			this.label_CircleTime.MinimumSize = new System.Drawing.Size(50, 0);
			this.label_CircleTime.Name = "label_CircleTime";
			this.label_CircleTime.Size = new System.Drawing.Size(72, 21);
			this.label_CircleTime.TabIndex = 9;
			this.label_CircleTime.Text = "0 S";
			this.label_CircleTime.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			this.contextMenuStrip_StepDebug.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.goToStepToolStripMenuItem, this.skipToolStripMenuItem, this.breakpointToolStripMenuItem });
			this.contextMenuStrip_StepDebug.Name = "contextMenuStrip_StepDebug";
			this.contextMenuStrip_StepDebug.Size = new System.Drawing.Size(141, 70);
			this.goToStepToolStripMenuItem.Name = "goToStepToolStripMenuItem";
			this.goToStepToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
			this.goToStepToolStripMenuItem.Text = "Go To This";
			this.skipToolStripMenuItem.Name = "skipToolStripMenuItem";
			this.skipToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
			this.skipToolStripMenuItem.Text = "Skip";
			this.breakpointToolStripMenuItem.Name = "breakpointToolStripMenuItem";
			this.breakpointToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
			this.breakpointToolStripMenuItem.Text = "Breakpoint";
			this.imageList_debug.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList_debug.ImageStream");
			this.imageList_debug.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList_debug.Images.SetKeyName(0, "Error.ico");
			this.imageList_debug.Images.SetKeyName(1, "Breakpoint.ico");
			this.imageList_debug.Images.SetKeyName(2, "skip.jpg");
			this.imageList_debug.Images.SetKeyName(3, "stop.jpg");
			this.timer_CircleTime.Interval = 200;
			this.timer_CircleTime.Tick += new System.EventHandler(timer_CircleTime_Tick);
			this.Column1.FillWeight = 40f;
			this.Column1.HeaderText = "Test Item";
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			this.Column2.FillWeight = 24f;
			this.Column2.HeaderText = "Expected";
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			this.Column3.FillWeight = 22f;
			this.Column3.HeaderText = "Measured";
			this.Column3.Name = "Column3";
			this.Column3.ReadOnly = true;
			this.Column4.FillWeight = 14f;
			this.Column4.HeaderText = "Status";
			this.Column4.Name = "Column4";
			this.Column4.ReadOnly = true;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			base.Controls.Add(this.groupBox_DUT);
			this.DoubleBuffered = true;
			base.Name = "DUT_SocketUI";
			base.Size = new System.Drawing.Size(666, 396);
			this.groupBox_DUT.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.dataGridView_ResultList).EndInit();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.contextMenuStrip_StepDebug.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
