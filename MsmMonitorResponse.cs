using System;
using System.Collections.Generic;

namespace mintymods {
	
	public class MsmMonitorResponse {
		
		public string type;
		public string source = "MSM[SHM]HWiNFO";
		MsmMonitorRequest request;
		public List<string> names;
		public List<MsmSensor> sensors;
		public List<MsmSensorReading> stats;
		public MsmException exception;
		public string version = "0.4A"; //@todo obtain from app
		public bool debug;
		public long time_taken_ms;

		public MsmMonitorResponse(MsmMonitorRequest request) {
			
			this.request = request;
			this.debug = request.debug;
			names = new List<string>();
			sensors = new List<MsmSensor>();
			stats = new List<MsmSensorReading>();
			type = GetType().Name;
			
		}
		
	}
	
}
