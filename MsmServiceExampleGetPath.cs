

using System;
using System.Management;

namespace mintymods {

	public class MsmServiceExampleGetPath : MsmServiceInterface {
		
		public const string REQUEST_MAPPING = "MSS[EXE]GETPATH";
		static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		MsmMonitorRequest request;
		MsmMonitorResponse response;
		
		public MsmServiceExampleGetPath(MsmMonitorRequest request) {
			this.request = request;
			response = new MsmMonitorResponse();
			response.source = "MSM[SHM]HWiNFO";
			response.version = "1.0";		
		}
		
		public string getRequestMapping() {
			return REQUEST_MAPPING;
		}
		
		
		public MsmMonitorResponse poll() {
			log.Debug("Request received @SOURCE#" + request.source);
			
			var response = new MsmMonitorResponse();
			response.source = "MSM[PROTOCOL]SERVICEID";
			response.version = "1.0";

			// Create tmp variables to store values during the query
			Double temperature = 0;
			String instanceName = "";


			// Note: run your app or Visual Studio (while programming) or you will get "Access Denied"

			ManagementObjectSearcher results = getResults("root\\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");
   	        foreach (ManagementObject service in results.Get())  {
	            log.Info(service.ToString());
	            log.Info(service["InstanceName"].ToString());
			            
				log.Warn("@OBJ#" + service);
		   		temperature = Convert.ToDouble(service["CurrentTemperature"].ToString());
		   		// Convert the value to celsius degrees
		   		temperature = (temperature - 2732) / 10.0;
		   		log.Info(temperature);
		   		instanceName = service["InstanceName"].ToString();	            
	         	log.Info(instanceName);
	         
	        }


			//var searcher = new ManagementObjectSearcher(@"root\cimv2", "SELECT * FROM CIM_VoltageSensor");
			results = getResults("root\\cimv2", "SELECT * FROM CIM_VoltageSensor");
   	        foreach (ManagementObject service in results.Get())  {
	            log.Info(service.ToString());
	            log.Info(service["InstanceName"].ToString());
	        }


			results = getResults("root\\CIMV2", "SELECT * FROM Win32_Service");
   	        foreach (ManagementObject service in results.Get())  {
	            log.Info(service.ToString());
	            log.Info(service["InstanceName"].ToString());
	        }

			
			return response;
		}
		
		public ManagementObjectSearcher getResults(string root, string sql) {
			log.Debug("query:@root#" + root + "@SQL#" + sql);
	        ManagementObjectSearcher searcher = 
	            new ManagementObjectSearcher(root, sql, new EnumerationOptions(null, System.TimeSpan.MaxValue, 1, true, false, true, true, false, true, true));
	       
	        if (request.debug) {
		        foreach (ManagementObject service in searcher.Get())  {
		            log.Info(service.ToString());
		            log.Info(service["InstanceName"].ToString());
		        }			
	        }
	        return searcher;
		}
		
		
		public void dispose() {
			log.Debug("Cleaning up any used resources and shutting down...");
		}
		
	}
}
