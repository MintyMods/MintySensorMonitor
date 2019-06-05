using System;
using System.Diagnostics;

namespace mintymods {
	
	public class MintySenorMonitor 	{
		
		MsmMonitorRequest request;
		MsmMonitorResponse response;
		
		public MintySenorMonitor(MsmMonitorRequest request) {
			this.request = request;
			this.response = new MsmMonitorResponse();
		}
		
		public string getSensorInfoAsJSON() {
			var timer = Stopwatch.StartNew();
			var monitor = new MsmHWiNFO(request);
			string json = "";
			try {
				response = monitor.poll();
				timer.Stop();
				response.time_taken_ms = timer.ElapsedMilliseconds;
				json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
			} catch (MsmException e) {
				if (response.exception == null) {
					response.exception = e;
				}
				timer.Stop();
				response.time_taken_ms = timer.ElapsedMilliseconds;
				json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
			} finally {
				monitor.Dispose();
			}
		
			if (request.debug) {
				Debug.WriteLine(json);
			}
			return json;
		}
		
	}
}

	