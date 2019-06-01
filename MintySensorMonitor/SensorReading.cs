using System;

namespace com.mintymods.msm {
	
	public class SensorReading { // "_HWiNFO_SENSORS_READING_ELEMENT"
		
		public Sensor sensor;
		public SensorType type; // SENSOR_READING_TYPE tReading;
		public int id; // dwReadingID
		public string label; // ReadingElement.szLabelUser;
		public string unit; // ReadingElement.szUnit;
		public double value; //ReadingElement.Value;
        public double min;
        public double max;
        public double avg;		
		
//        
//		public SensorReading(_HWiNFO_SENSORS_READING_ELEMENT reading) {
//	        reading.type = (SensorType) reading.tReading;
//	        //reading.sensor = (Sensor)response.names[(int)ReadingElement.dwSensorIndex]; // todo
//	        reading.id = (int)reading.dwSensorIndex;
//	        reading.label = reading.szLabelUser;
//	        reading.unit = reading.szUnit;
//	        reading.value = reading.Value;
//		}
		
	}
}
