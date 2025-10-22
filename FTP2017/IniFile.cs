using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace FTP2017
{
	public class IniFile
	{
		public string IniFilePath = Application.StartupPath + "\\Config\\StationConfig.cfg";

		private static object LockedObj = new object();

		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

		[DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
		private static extern uint GetPrivateProfileStringA(string section, string key, string def, byte[] retVal, int size, string filePath);

		public static List<string> ReadSections(string iniFilename)
		{
			List<string> result = new List<string>();
			byte[] buf = new byte[65536];
			uint len = GetPrivateProfileStringA(null, null, null, buf, buf.Length, iniFilename);
			int j = 0;
			for (int i = 0; i < len; i++)
			{
				if (buf[i] == 0)
				{
					result.Add(Encoding.Default.GetString(buf, j, i - j));
					j = i + 1;
				}
			}
			return result;
		}

		public List<string> ReadKeys(string SectionName)
		{
			return ReadKeys(SectionName, IniFilePath);
		}

		public static List<string> ReadKeys(string SectionName, string iniFilename)
		{
			List<string> result = new List<string>();
			byte[] buf = new byte[65536];
			uint len = GetPrivateProfileStringA(SectionName, null, null, buf, buf.Length, iniFilename);
			int j = 0;
			for (int i = 0; i < len; i++)
			{
				if (buf[i] == 0)
				{
					result.Add(Encoding.Default.GetString(buf, j, i - j));
					j = i + 1;
				}
			}
			return result;
		}

		public void IniWriteValue(string Section, string Key, string Value)
		{
			lock (LockedObj)
			{
				WritePrivateProfileString(Section, Key, Value, IniFilePath);
			}
		}

		public string IniReadValue(string Section, string Key)
		{
			lock (LockedObj)
			{
				StringBuilder temp = new StringBuilder(500);
				int i = GetPrivateProfileString(Section, Key, "", temp, 500, IniFilePath);
				return temp.ToString();
			}
		}
	}
}
