using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TakeimgIVI.Function
{
	public class INI
	{
		// dùng lại
		public static void WriteIni(string fileName, string section, string item, string value)
		{
			WritePrivateProfileString(ref section, ref item, ref value, ref fileName);
		}

		[DllImport("kernel32", EntryPoint = "WritePrivateProfileStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		public static extern int WritePrivateProfileString([MarshalAs(UnmanagedType.VBByRefStr)] ref string lpApplicationName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpKeyName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpString, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpFileName);


		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
		public static string ReadIni(string fileName, string section, string item, string defaultValue = "")
		{
			StringBuilder SB = new StringBuilder(255);
			GetPrivateProfileString(section, item, defaultValue, SB, 255, fileName);
			return SB.ToString();
		}
	}
}
