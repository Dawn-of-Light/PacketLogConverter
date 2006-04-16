using System;
using System.Windows.Forms;

namespace PacketLogConverter
{
	/// <summary>
	/// Handles messages logging.
	/// </summary>
	public class Log
	{
		private Log()
		{
		}
		
		public static void Info(string str)
		{
			MessageBox.Show(str, "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		
		public static void Error(string str, Exception e)
		{
			ErrorForm form = new ErrorForm();
			form.errorTextBox.Text = e.ToString() + "\n";
			form.descriptionTextBox.Text = str;
			form.ShowDialog();
		}
		
		public static void Error(string str)
		{
			ErrorForm form = new ErrorForm();
			form.errorTextBox.Text = str + "\n";
			form.ShowDialog();
		}
	}
}
