using System;

namespace mintymods {

	public class MsmServiceExample : MsmServiceInterface {
		
		static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		
		public static string REQUEST_MAPPING = "MSM[JSON]EXAMPLE";
		public static string RESPONSE_MAPPING = "MSS[JSON]EXAMPLE";
		
		MsmMonitorRequest request;
		MsmMonitorResponse response;
		
		public MsmServiceExample(MsmMonitorRequest request) {
			this.request = request;
			response = new MsmMonitorResponse();
			response.source = getRequestMapping();
			response.version = "1.0";		
		}
	
		public string getRequestMapping() {
			return MsmServiceExample.REQUEST_MAPPING;
		}

		public string getResponseMapping() {
			return MsmServiceExample.REQUEST_MAPPING;
		}
		
		public MsmMonitorResponse poll() {
			log.Debug("Request received @SOURCE#" + request.source);
			
			const uint id = 12345;
			var sensor = new MsmSensor();
			sensor.label = new MsmSensorLabel("value","description");  
			sensor.id = id;
			//sensor.name = "EXAMPLE:CPU";
			sensor.instance = 1; // instance_id for multiples like CPU[1], CPU[2]
			response.sensors.Add(sensor);
			response.labels.Add("sensor.label");			

			var volts = new MsmSensorReading(MsmSensorType.VOLT);
			volts.label = new MsmSensorLabel("CPU[" + sensor.instance + "]VOLT","Central Processor Voltage");
			volts.id = id;
			volts.unit = "Volt(s)";
			volts.value = 1.154667;
			volts.min = 1.228763;
			volts.max = 1.354786;
			volts.avg = 1.286443;
			response.readings.Add(volts);			

			var rpm = new MsmSensorReading(MsmSensorType.FAN);
			rpm.label = new MsmSensorLabel("CPU[" + sensor.instance + "]FAN","Central Processor Fan Speed");
			rpm.id = id;
			rpm.unit = "Rpm";
			rpm.value = 45.898765;
			rpm.min = 0.0;
			rpm.max = 305346.12312;
			rpm.avg = 40.73450;
			response.readings.Add(rpm);				
			
			return response;
		}
		
		public void dispose() {
			log.Debug("Cleaning up any used resources and shutting down...");
		}
		
	}
}
