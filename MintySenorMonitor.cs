using System;
using System.Diagnostics;

namespace mintymods {
	
	public class MintySenorMonitor 	{

		static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);		
		readonly MsmMonitorRequest request;
		MsmMonitorResponse response;
		
		public MintySenorMonitor(MsmMonitorRequest request) {
			this.request = request;
			this.response = new MsmMonitorResponse();
            log.Debug("Created new MintySensorMonitor");
		}
		
		public string getSensorInfoAsJSON() {
			var timer = Stopwatch.StartNew();
            var controller = new MsmServiceController();
            MsmServiceInterface monitor = controller.getMonitorForRequest(request);
            log.Debug("@Created monitor#" + monitor);
            string json = "";
			try {
				response = monitor.poll();
                timer.Stop();
                response.time_taken = timer.Elapsed;
                json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                log.Debug("@JSON#" + json);
			} catch (MsmException e) {
				if (response.exception == null) {
					response.exception = e;
				}
                timer.Stop();
                response.time_taken = timer.Elapsed;
                json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
			} finally {
                monitor.dispose();
            }

            if (request.debug) {
				log.Debug(json);
				Debug.WriteLine(json);
			}
			return json;
		}
		
	}
}

	