using System;

namespace mintymods {
	
	public class MsmSensorReading {
		
		public MsmSensorType type;
		public MsmSensorLabel label;
		public UInt32 id;
		public UInt32 sensor_index = 1;
		public string unit;
		public double value;
		public double min;
		public double max;
		public double avg;
		
		public MsmSensorReading(MsmSensorType type) {
			this.type = type;
		}
			
	}
	
}
