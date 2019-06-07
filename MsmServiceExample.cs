using System;

namespace mintymods {

	public class MsmServiceExample : MsmServiceInterface {
		
		public const string REQUEST_MAPPING = "MSS[EXE]SERVICE";
		static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		MsmMonitorRequest request;
		MsmMonitorResponse response;
		
		public MsmServiceExample(MsmMonitorRequest request) {
			this.request = request;
			response = new MsmMonitorResponse();
			response.source = getRequestMapping();
			response.version = "1.0";		
		}
	
		public string getRequestMapping() {
			return REQUEST_MAPPING;
		}
		
		public MsmMonitorResponse poll() {
			log.Debug("Request received @SOURCE#" + request.source);
			
			const uint id = 213;
			var sensor = new MsmSensor();
			sensor.label = new MsmSensorLabel("value","description");  
			sensor.id = id; 
			sensor.instance = 0; // instance_id for multiples like CPU[1], CPU[2]
			response.sensors.Add(sensor);
			response.labels.Add("sensor.label");			

			var volts = new MsmSensorReading(MsmSensorType.VOLT);
			volts.label = new MsmSensorLabel("CPU[" + sensor.instance + "]VOLT","CPU.Voltage");
			volts.id = id;
			volts.unit = "V";
			volts.value = 100;
			volts.min = 10;
			volts.avg = 70;
			response.readings.Add(volts);			

			var rpm = new MsmSensorReading(MsmSensorType.FAN);
			rpm.label = new MsmSensorLabel("CPU[" + sensor.instance + "]FAN ","CPU Fan");
			rpm.id = id;
			rpm.unit = "Rpm";
			rpm.value = 1213.231;
			rpm.min = 0.12312;
			rpm.avg = 40.73450;
			response.readings.Add(rpm);				
			
			return response;
		}
		
		
		public void dispose() {
			log.Debug("Cleaning up any used resources and shutting down...");
		}
		
	}
}
