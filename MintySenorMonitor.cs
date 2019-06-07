﻿using System;
using System.Diagnostics;

namespace mintymods {
	
	public class MintySenorMonitor 	{

		static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);		
		MsmMonitorRequest request;
		MsmMonitorResponse response;
		
		public MintySenorMonitor(MsmMonitorRequest request) {
			this.request = request;
			this.response = new MsmMonitorResponse();
		}
		
		public string getSensorInfoAsJSON() {
			var timer = Stopwatch.StartNew();
			MsmServiceController controller = new MsmServiceController();
			MsmServiceInterface monitor = controller.getMonitorForRequest(request);
			
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

	