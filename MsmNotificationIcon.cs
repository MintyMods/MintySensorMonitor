using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace mintymods
{
	
	public sealed class MsmNotificationIcon
	{
		private NotifyIcon notifyIcon;
		private ContextMenu notificationMenu;
		
	
		public MsmNotificationIcon()
		{
			notifyIcon = new NotifyIcon();
			notificationMenu = new ContextMenu(InitializeMenu());
			notifyIcon.DoubleClick += IconDoubleClick;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MsmNotificationIcon));
			notifyIcon.Icon = (Icon)resources.GetObject("$this.Icon");
			notifyIcon.ContextMenu = notificationMenu;		
		}
		
		private MenuItem[] InitializeMenu()
		{
			MenuItem[] menu = new MenuItem[] {
				new MenuItem("MSS - Sensor Server", menuMSSClick),
				new MenuItem("MSM - Sensor Monitor", menuMSMClick),
				new MenuItem("Settings", menuAdminClick),
				new MenuItem("Debug", menuDebugClick),
				new MenuItem("About", menuAboutClick),
				new MenuItem("Exit", menuExitClick)
			};
			return menu;
		}
		
		
//		[STAThread]
//		public static void Main(string[] args)
//		{
//			Application.EnableVisualStyles();
//			Application.SetCompatibleTextRenderingDefault(false);
//			
//			bool isFirstInstance;
//			using (Mutex mtx = new Mutex(true, "MintySensorMonitor", out isFirstInstance)) {
//				if (isFirstInstance) {
//					MsmNotificationIcon icon = new MsmNotificationIcon();
//					icon.notifyIcon.Visible = true;
//					Application.Run();
//					icon.notifyIcon.Dispose();
//				} else {
//					// The application is already running
//					// TODO: Display message box or change focus to existing application instance
//					MessageBox.Show("MSM - Minty's Server Monitor is already running");
//				}
//			} 
//		}
		

		private void menuAdminClick(object sender, EventArgs e)
		{
			Console.Write("Admin");
		}
				
		private void menuAboutClick(object sender, EventArgs e)
		{
			Console.Write("Aboutwwwwwwwwwwwwww");
		}
				
		private void menuMSSClick(object sender, EventArgs e)
		{
			Console.Write("MSS - Minty's Sensor Server");
		}
				
		private void menuMSMClick(object sender, EventArgs e)
		{
			Console.Write("MSM - Minty's Sensor Monitor");
		}
		
		private void menuDebugClick(object sender, EventArgs e)
		{

			
		}
		
		private void menuExitClick(object sender, EventArgs e)
		{
			Application.Exit();
		}
		
		private void IconDoubleClick(object sender, EventArgs e)
		{
			MessageBox.Show("The icon was double clicked - push notifications coming");
		}

	}
}
