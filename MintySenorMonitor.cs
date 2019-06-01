using System;
using System.Web.Script.Serialization;
using System.Diagnostics;

namespace com.mintymods.msm
{
	public class MintySenorMonitor
	{
		public string getSensorInfoAsJSON()
		{
			return getSensorInfoAsJSON(new MsmMonitorRequest());
		}
		
		public string getSensorInfoAsJSON(MsmMonitorRequest request)
		{
			var timer = Stopwatch.StartNew();
			MsmMonitorResponse response = new MsmMonitorResponse();
			HWiNFOWrapper wrapper = new HWiNFOWrapper(request, response);
			string json = "";
			try {
				response = wrapper.getSensorReadings();
				timer.Stop();
				response.time_taken_ms = timer.ElapsedMilliseconds;
				json = new JavaScriptSerializer().Serialize(response);
			} catch (MsmException e) {
				if (response.exception == null) {
					response.exception = e;
				}
				timer.Stop();
				response.time_taken_ms = timer.ElapsedMilliseconds;
				json = new JavaScriptSerializer().Serialize(response);
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

	