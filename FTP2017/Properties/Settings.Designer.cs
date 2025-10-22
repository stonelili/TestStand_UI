using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace FTP2017.Properties
{
	// Token: 0x02000016 RID: 22
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x0000CD1C File Offset: 0x0000AF1C
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x040000C2 RID: 194
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
