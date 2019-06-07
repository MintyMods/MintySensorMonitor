using System;

namespace mintymods {

	public class MsmServiceHWiNFO : MsmServiceInterface {
		
		public const string REQUEST_MAPPING = "MSS[EXE]HWiNFO";
		static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		MsmMonitorRequest request;
		MsmMonitorResponse response;
		
		public MsmServiceHWiNFO(MsmMonitorRequest request) {
			this.request = request;
			response = new MsmMonitorResponse();
			response.source = getRequestMapping();
			response.version = "1.0";		
		}
	
		public string getRequestMapping() {
			return REQUEST_MAPPING;
		}
		
		public MsmMonitorResponse poll() {
			log.Debug("Request received @SOURCE#" + request.source);
			
			// Class should be overridden at runtime so do nothing here
			return response;
		}
		
		
		public void dispose() {
			log.Debug("Cleaning up any used resources and shutting down...");
		}
		
	}
}
