using System;

namespace mintymods {
	
	public class MSM {

		static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		const int REQUEST_BODY_OFFSET = 0;
				
		public static void Main(string[] commandLineArguments) { 
			MsmLogging.configureLogging();
			log.Debug("MSM starting#args@" + commandLineArguments.Length);
			if (commandLineArguments.Length != 1) {
				log.Warn("Invalid command line specified : @JSON#" + commandLineArguments);
				sendInvalidCommandLineJsonResponse(commandLineArguments);
			} else {

				try {
					var json = commandLineArguments[REQUEST_BODY_OFFSET];
					var request = new MsmMonitorRequest(json);
					log.Debug("Request forged@" + request.ToString());
					var monitor = new MintySenorMonitor(request);
					log.Debug("Monitor forged@" + monitor.ToString());
					sendValidJsonResponse(monitor.getSensorInfoAsJSON());
				} catch (MsmException e) {
					log.Error("@"+ e.InnerException.Source + "#" + e.Message, e);
					sendInvalidJsonResponse(e);
				}
				
			}
    	}
      
    	static void sendValidJsonResponse(String json) {
			Console.WriteLine(json);
		}

    	static void sendInvalidJsonResponse(MsmException exception) {
			log.Debug(exception);
			Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
		}
		
		static void sendInvalidCommandLineJsonResponse(string[] commandLineArguments) {
			var e = new MsmException("Invalid JSON Request Argument");
			e.hint.message = "You must provide a valid JSON parameter as the ONLY argument when calling MSM";
			e.hint.input = Newtonsoft.Json.JsonConvert.SerializeObject(commandLineArguments);
			e.hint.output = "Usage: MintySensorMonitor.exe {json:here}";
			e.hint.result = "Example Usage: MintySensorMonitor.exe {help:true}";
			sendInvalidJsonResponse(e);
		}

	}
}
