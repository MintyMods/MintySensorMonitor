using System;
using System.Collections.Generic;

namespace mintymods {
	
	public class MsmMonitorResponse {
		
		static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		public List<MsmSensor> sensors;
		public List<MsmSensorReading> readings;
		public MsmException exception;
		public List<string> labels;
		public string type;
		public string source = "MSM[SHM]HWiNFO";
		public string version = "0.5A";
		public bool debug;
		public long time_taken_ms;

		public MsmMonitorResponse() {

			labels = new List<string>();
			sensors = new List<MsmSensor>();
			readings = new List<MsmSensorReading>();
			type = GetType().Name;
			
		}
		
	}
	
}
