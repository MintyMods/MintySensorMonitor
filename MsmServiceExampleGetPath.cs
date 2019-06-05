using System;
using System.Management;

namespace mintymods {

	public class MsmServiceExampleGetPath : MsmServiceInterface {
		
		static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		MsmMonitorRequest request;
		MsmMonitorResponse response;
		
		public MsmServiceExampleGetPath(MsmMonitorRequest request) {
			this.request = request;
			response = new MsmMonitorResponse();
			response.source = "MSM[REGISTRY]PATH";
			response.version = "1.0";		
		}
		
		public MsmMonitorResponse poll() {
			log.Info("Request received @SOURCE#" + request.source);
			
		
			string path = System.Environment.GetEnvironmentVariable("PATH");
			log.Debug("@PATH#" + path);
			string key = @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment\";
			log.Debug("@KEY#" + key);
			//string variable = (string)Registry.LocalMachine.OpenSubKey(key).GetValue("PATH", "", RegistryValueOptions.DoNotExpandEnvironmentNames);
			//log.Debug("@ENV#" + env);

			return response;
		}
		
		public void dispose() {
			log.Debug("Cleaning up any used resources and shutting down...");
		}
		
	}
}
