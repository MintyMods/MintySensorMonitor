using System;
using System.Collections.Generic;

namespace com.mintymods.msm {

	public class MsmMonitorResponse {
		
		public Guid guid;
		public List<string> names;
		public List<Sensor> sensors;
		public List<SensorReading> stats;
		public MsmException exception;
		public bool debug;
		
		public MsmMonitorResponse() {
			guid = Guid.NewGuid();
			names = new List<string>();
			sensors = new List<Sensor>();
			stats = new List<SensorReading>();
		}
		
	}
}
