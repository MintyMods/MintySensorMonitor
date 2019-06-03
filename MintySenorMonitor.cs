using System;
using System.Diagnostics;

namespace mintymods {
	
	public class MintySenorMonitor 	{
		
		private MsmMonitorRequest request;
		private MsmMonitorResponse response;
		
		public MintySenorMonitor(MsmMonitorRequest request) {
			this.request = request;
			this.response = new MsmMonitorResponse(request);
		}
		
		public string getSensorInfoAsJSON() {
			var timer = Stopwatch.StartNew();
			HWiNFOWrapper wrapper = new HWiNFOWrapper(request, response);
			string json = "";
			try {
				response = wrapper.poll();
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
				wrapper.Dispose();
			}
		
			if (request.debug) {
				Debug.WriteLine(json);
			}
			return json;
		}
		
	}
}

	