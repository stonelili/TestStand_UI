using System;
using System.Diagnostics;
using System.Windows.Forms;
using NationalInstruments.TestStand.Utility;

namespace FTP2017
{
	internal static class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{
			try
			{
				Process[] processes = Process.GetProcesses();
				Process currentProcess = Process.GetCurrentProcess();
				bool processExist = false;
				Process[] array = processes;
				foreach (Process p in array)
				{
					if (p.ProcessName == currentProcess.ProcessName && p.Id != currentProcess.Id)
					{
						processExist = true;
					}
				}
				if (processExist)
				{
					MessageBox.Show("The FTP_2017.exe has been opened!");
					Application.Exit();
				}
				else
				{
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(defaultValue: false);
					SplashScreen.Show(typeof(StartupUI));
					LaunchTestStandApplicationInNewDomain.LaunchProtected(MainEntryPoint, args, "FTP2017", DisplayErrorMessage, parseArgs: true);
				}
				CleanupUI.CloseMe();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private static void MainEntryPoint(string[] args)
		{
			FormMain fm = new FormMain();
			SplashScreen.Close();
			ApplicationWrapper.Run(fm);
			Application.Run(new CleanupUI());
		}

		private static void DisplayErrorMessage(string caption, string message)
		{
		}
	}
}
