using System;
using System.Threading;
using System.Windows.Forms;

namespace PacketLogConverter
{
	public delegate void WorkCallback(ProgressCallback callback, object state);
	public delegate void ProgressCallback(int workDone, int workTotal);
	public delegate void StateObjectCallback(object state);

	/// <summary>
	/// Show progress window and do the work
	/// </summary>
	public class Progress
	{
		private const int MIN_UPDATE_INTERVAL = 100; // update every 100ms or more

		private object m_state;
		private Thread m_thread;
		private uint m_lastUpdate;
		private WorkCallback m_workCallback;
		private readonly Form m_parentForm;
		private readonly ProgressForm m_progressForm;
		private readonly ProgressCallback m_progressCallback;

		public Progress(Form parentForm)
		{
			if (parentForm == null)
				throw new ArgumentNullException("parentForm");
			
			m_parentForm = parentForm;
			m_progressForm = new ProgressForm();
			m_progressCallback = new ProgressCallback(ProgressCallback);
		}

		public Form ParentForm
		{
			get { return m_parentForm; }
		}

		public WorkCallback WorkCallback
		{
			get { return m_workCallback; }
			set { m_workCallback = value; }
		}

		public StateObjectCallback WorkFinishedCallback;

		public void SetDescription(string str)
		{
			m_progressForm.DescriptionLabel.Text = str;
//			m_progressForm.Invoke(new StringParamCallback(ChangeDescription), new object[] {str});
		}

		private void ChangeDescription(string str)
		{
			m_progressForm.DescriptionLabel.Text = str;
		}

		private delegate void StringParamCallback(string str);
		
		public void Start(WorkCallback work, object state)
		{
			lock (this)
			{
				if (m_thread != null)
					throw new Exception("work is already started!");
				
				m_state = state;
				m_workCallback = work;
				m_thread = new Thread(new ThreadStart(WorkThread));
				m_thread.IsBackground = true;
				m_thread.Name = "WorkThread";
				m_thread.Start();
			}
		}
		
		public bool IsActive
		{
			get { lock (this) return m_thread != null; }
		}
		
		private void WorkThread()
		{
			object state = m_state;
			
			m_parentForm.BeginInvoke(new MethodInvoker(WorkStartCallback));
			
			while (!m_progressForm.Visible)
				Thread.Sleep(1);
			
			try
			{
				m_workCallback(m_progressCallback, state);
			}
			catch (Exception e)
			{
				Log.Error("work callback", e);
			}
			finally
			{
				m_parentForm.Invoke(new MethodInvoker(WorkEndCallback));
				lock (this)
				{
					m_thread = null;
				}
			}
		}
		
		private void WorkStartCallback()
		{
//			m_parentForm.Enabled = false;
			m_progressForm.ShowDialog(m_parentForm);
		}

		private void WorkEndCallback()
		{
			m_lastUpdate = uint.MinValue;
			int max = m_progressForm.ProgressBar.Maximum;
			ProgressCallback(max, max);
			m_progressForm.Refresh();
			m_progressForm.Close();

			m_parentForm.Enabled = true;
			m_parentForm.Refresh();

			StateObjectCallback callback = WorkFinishedCallback;
			try
			{
				if (callback != null)
					callback(m_state);
			}
			catch (OutOfMemoryException e)
			{
				Log.Info(e.Message);
			}
			catch (Exception e)
			{
				Log.Error("work end callback", e);
			}
		}
		
		private void ProgressCallback(int workDone, int workTotal)
		{
			uint ticks = (uint)Environment.TickCount;
			if (ticks - m_lastUpdate < MIN_UPDATE_INTERVAL)
				return;
			m_lastUpdate = ticks;
			
			UpdateData data = new UpdateData();
			data.WorkDone = workDone;
			data.WorkTotal = workTotal;
			data.Label = string.Format("{0:N0} of {1:N0}", workDone, workTotal);
			m_progressForm.Invoke(new StateObjectCallback(ProgressUpdateCallback), new object[] {data});
		}

		private void ProgressUpdateCallback(object state)
		{
			UpdateData data = (UpdateData) state;
			m_progressForm.ProgressBar.Maximum = data.WorkTotal;
			m_progressForm.ProgressBar.Value = data.WorkDone;
			m_progressForm.WorkProgressLabel.Text = data.Label;
		}

		private class UpdateData
		{
			public int WorkTotal;
			public int WorkDone;
			public string Label;
		}
	}
}
