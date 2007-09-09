using System;
using System.IO;

namespace PacketLogConverter.LogWriters
{
	/// <summary>
	/// Summary description for Log.
	/// </summary>
	public class Logger
	{
		static string LogFile=null;
		static bool LogInit=true;

		private Logger()
		{
		}

		static public string File
		{
			set
			{
				LogFile=value;
				LogInit=false;
			}
		}

		static public void Say(string s)
		{
			if (LogInit)
			{
				try
				{
					LogFile=(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)+System.IO.Path.DirectorySeparatorChar+System.IO.Path.ChangeExtension(System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase),"log")).Replace("file:\\","");
				}
				catch
				{
					LogFile=null;
				}
				LogInit=false;
			}
			Console.WriteLine(System.DateTime.Now.ToString() + " " + s);
			if (LogFile != null)
			{
				StreamWriter sw = new StreamWriter(LogFile,true,System.Text.Encoding.Default);
				sw.WriteLine(System.DateTime.Now.ToString() + " " + s);
				sw.Close();
			}
		}

		static public void Say( string s,  params object[] arg )
		{
			Say(String.Format(s, arg));
		}


		static public void Say(TimeSpan time, string s)
		{
			if (LogInit)
			{
				try
				{
					LogFile=(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)+System.IO.Path.DirectorySeparatorChar+System.IO.Path.ChangeExtension(System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase),"log")).Replace("file:\\","");
				}
				catch
				{
					LogFile = null;
				}
				LogInit = false;
			}
			Console.WriteLine(time.ToString() + " " + s);
			if (LogFile != null)
			{
				StreamWriter sw = new StreamWriter(LogFile, true, System.Text.Encoding.Default);
				sw.WriteLine(string.Format("{0, -16} {1}", time.ToString(), s));
				sw.Close();
			}
		}

		static public void Say(TimeSpan time, string s, params object[] arg)
		{
			Say(time, String.Format(s, arg));
		}
	}
}
