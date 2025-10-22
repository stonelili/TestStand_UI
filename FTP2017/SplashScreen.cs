using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace FTP2017
{
	public static class SplashScreen
	{
		private delegate void ChangeFormTextdelegate(string s);

		private static object _obj = new object();

		private static Form _SplashForm = null;

		private static Thread _SplashThread = null;

		private static string IniFilePath = Application.StartupPath + "\\Config\\StationConfig.cfg";

		private static object LockedObj = new object();

		public static void Show(Type splashFormType)
		{
			if (_SplashThread == null)
			{
				if (splashFormType == null)
				{
					throw new Exception();
				}
				IniWriteValue("Config", "SplashScreenFlag", "1");
				_SplashThread = new Thread((ThreadStart)delegate
				{
					CreateInstance(splashFormType);
					Application.Run(_SplashForm);
				});
				_SplashThread.IsBackground = true;
				_SplashThread.SetApartmentState(ApartmentState.STA);
				_SplashThread.Start();
			}
		}

		public static void GetThreadAndForm(ref string sThread, ref string sForm)
		{
			for (int i = 0; i < 20; i++)
			{
				Thread.Sleep(100);
				if (_SplashForm != null)
				{
					sThread = _SplashThread.ToJSON();
					sForm = FormToString(_SplashForm);
					break;
				}
			}
		}

		public static void ChangeTitle(string status)
		{
			ChangeFormTextdelegate de = ChangeText;
			_SplashForm.Invoke(de, status);
		}

		public static void Close()
		{
			IniWriteValue("Config", "SplashScreenFlag", "0");
			if (_SplashThread != null && _SplashForm != null)
			{
				try
				{
					_SplashForm.Invoke(new MethodInvoker(_SplashForm.Close));
				}
				catch (Exception)
				{
				}
				_SplashThread = null;
				_SplashForm = null;
			}
		}

		public static void Close(string sThread, string sForm)
		{
			_SplashForm = (Form)StringToForm(sForm);
			_SplashForm.Close();
			if (_SplashThread != null && _SplashForm != null)
			{
				try
				{
					_SplashForm.Invoke(new MethodInvoker(_SplashForm.Close));
				}
				catch (Exception)
				{
				}
				_SplashThread = null;
				_SplashForm = null;
			}
		}

		private static void ChangeText(string title)
		{
			_SplashForm.Text = title.ToString();
		}

		private static void CreateInstance(Type FormType)
		{
			if (_SplashForm != null)
			{
				return;
			}
			lock (_obj)
			{
				object obj = FormType.InvokeMember(null, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null);
				_SplashForm = obj as Form;
				_SplashForm.TopMost = true;
				_SplashForm.ShowInTaskbar = false;
				_SplashForm.BringToFront();
				_SplashForm.StartPosition = FormStartPosition.CenterScreen;
				if (_SplashForm == null)
				{
					throw new Exception();
				}
			}
		}

		public static string ObjectToJson(object obj)
		{
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
			MemoryStream stream = new MemoryStream();
			serializer.WriteObject((Stream)stream, obj);
			byte[] dataBytes = new byte[stream.Length];
			stream.Position = 0L;
			stream.Read(dataBytes, 0, (int)stream.Length);
			return Encoding.UTF8.GetString(dataBytes);
		}

		public static object JsonToObject(string jsonString, object obj)
		{
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
			MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
			return serializer.ReadObject((Stream)mStream);
		}

		public static string ToJSON(this object o)
		{
			if (o == null)
			{
				return null;
			}
			return JsonConvert.SerializeObject(o);
		}

		public static T FromJSON<T>(this string input)
		{
			try
			{
				return JsonConvert.DeserializeObject<T>(input);
			}
			catch (Exception)
			{
				return default(T);
			}
		}

		public static string FormToString(object o)
		{
			MemoryStream ms = new MemoryStream();
			BinaryFormatter form = new BinaryFormatter();
			form.Serialize(ms, o);
			ms.Flush();
			byte[] bts = ms.GetBuffer();
			MemoryStream _ms = new MemoryStream(bts);
			object ff = form.Deserialize(_ms);
			return Encoding.Default.GetString(bts);
		}

		public static object StringToForm(string inputStr)
		{
			MemoryStream ms = new MemoryStream();
			BinaryFormatter form = new BinaryFormatter();
			Type type = typeof(StartupUI);
			object obj = Activator.CreateInstance(type);
			form.Serialize(ms, obj);
			ms.Flush();
			byte[] bts = Encoding.Default.GetBytes(inputStr);
			MemoryStream _ms = new MemoryStream(bts);
			return form.Deserialize(_ms);
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
	}
}
