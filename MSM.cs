using System;
using Newtonsoft.Json.Converters;

// @todo ::  MSS:Java --(JNA)-->  C/C++ --(COM Interop)--> MSM:C#
// @todo ::  MSS:Java --(TCP)-->  HWiNFO

namespace mintymods {
	
	public class MSM {

		// Java --(Process Builder via EXE) --> C#
		
		public const int REQUEST_BODY_OFFSET = 0;
				
		public static void Main(string[] commandLineArguments) { 
			
			if (commandLineArguments.Length != 1) {
				sendInvalidCommandLineJsonResponse(commandLineArguments);
			} else {

				try {
					string json = commandLineArguments[REQUEST_BODY_OFFSET];
					MsmMonitorRequest request = new MsmMonitorRequest(json);
					MintySenorMonitor monitor = new MintySenorMonitor(request);
					sendJsonResponse(monitor.getSensorInfoAsJSON());
				} catch (MsmException e) {
					sendJsonResponse(Newtonsoft.Json.JsonConvert.SerializeObject(e));
				}
				
			}
    	}
      
    	public static void sendJsonResponse(String json) {
			Console.WriteLine(json);
		}
		
		static void sendInvalidCommandLineJsonResponse(string[] commandLineArguments) {
			MsmException exception = new MsmException("Invalid JSON Request Argument");
			exception.hint.message = "You must provide a valid JSON parameter as the ONLY argument";
			exception.hint.input = Newtonsoft.Json.JsonConvert.SerializeObject(commandLineArguments);
			exception.hint.output = "Usage: MintySensorMonitor.exe {json:here}";
			exception.hint.result = "Example Usage: MintySensorMonitor.exe {help:true, debug:false}";
			sendJsonResponse(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
		}

	}
}
