using System.IO;
using System.Windows.Forms;

namespace FTP2017
{
	public class SequenceSetting
	{
		private string _StepDisplayType = "";

		private string _ProjectName = "";

		private string _CustomerName = "";

		private string _AutomationPath = "";

		private string _SequenceFile = "";

		private string _ConfigFile = "";

		private string _Version = "";

		private int _SerialNumberLen = 0;

		private int _SocketNumber = 2;

		private int _SocketColumnsNumber = 2;

		private string _Station = "";

		private string _ExePath = "";

		private string _LogFilePath = "";

		private string _LogoFilePath = "";

		public string StepDisplayType
		{
			get
			{
				return _StepDisplayType;
			}
			set
			{
				_StepDisplayType = value;
			}
		}

		public string ProjectName
		{
			get
			{
				return _ProjectName;
			}
			set
			{
				_ProjectName = value;
			}
		}

		public string CustomerName
		{
			get
			{
				return _CustomerName;
			}
			set
			{
				_CustomerName = value;
			}
		}

		public string AutomationPath
		{
			get
			{
				_AutomationPath = GetAbsolutePath(_AutomationPath);
				return _AutomationPath;
			}
			set
			{
				_AutomationPath = value;
			}
		}

		public string SequenceFile
		{
			get
			{
				_SequenceFile = GetAbsolutePath(_SequenceFile);
				return _SequenceFile;
			}
			set
			{
				_SequenceFile = value;
			}
		}

		public string ConfigFile
		{
			get
			{
				_ConfigFile = GetAbsolutePath(_ConfigFile);
				return _ConfigFile;
			}
			set
			{
				_ConfigFile = value;
			}
		}

		public string Version
		{
			get
			{
				return _Version;
			}
			set
			{
				_Version = value;
			}
		}

		public int SerialNumberLen
		{
			get
			{
				return _SerialNumberLen;
			}
			set
			{
				_SerialNumberLen = value;
			}
		}

		public int SocketNumber
		{
			get
			{
				return _SocketNumber;
			}
			set
			{
				_SocketNumber = value;
			}
		}

		public int SocketColumnsNumber
		{
			get
			{
				return _SocketColumnsNumber;
			}
			set
			{
				_SocketColumnsNumber = value;
			}
		}

		public string StationName
		{
			get
			{
				return _Station;
			}
			set
			{
				_Station = value;
			}
		}

		public string ExePath
		{
			get
			{
				return _ExePath;
			}
			set
			{
				_ExePath = value;
			}
		}

		public string LogFilePath
		{
			get
			{
				_LogFilePath = GetAbsolutePath(_LogFilePath);
				return _LogFilePath;
			}
			set
			{
				_LogFilePath = value;
			}
		}

		public string LogoFilePath
		{
			get
			{
				_LogoFilePath = GetAbsolutePath(_LogoFilePath);
				return _LogoFilePath;
			}
			set
			{
				_LogoFilePath = value;
			}
		}

		private string GetAbsolutePath(string filePath)
		{
			string resultStr = filePath;
			if (resultStr == "")
			{
				return resultStr;
			}
			if (!File.Exists(filePath))
			{
				resultStr = ((!(filePath.Substring(0, 1) == "\\")) ? (Application.StartupPath + "\\" + filePath) : (Application.StartupPath + filePath));
				if (!File.Exists(resultStr))
				{
					resultStr = filePath;
				}
			}
			return resultStr;
		}
	}
}
