using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace com.mintymods.msm {

	public class MsmMonitorResponse {
		
		public string source = "MSM[JNI]<--HWiNFO[SHM]";
		public string guid = System.Guid.NewGuid().ToString();
		public List<string> names;
		public List<Sensor> sensors;
		public List<SensorReading> stats;
		public MsmException exception;
		public bool debug;
		public long time_taken_ms;
		
		public MsmMonitorResponse() {
			names = new List<string>();
			sensors = new List<Sensor>();
			stats = new List<SensorReading>();
		}
		
	}
}
