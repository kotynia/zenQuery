using System;
using System.Windows.Forms;

namespace zenQuery
{
	static class Program
	{
        public static ConnectForm ConnectForm = null;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new frmMdi());
            //Application.Run(new ConnectForm());
		}
	}
}