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
            response = new MsmMonitorResponse {
                source = getRequestMapping(),
                version = "1.0"
            };
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
            var sensor = new MsmSensor {
                label = new MsmSensorLabel("value", "description"),
                id = id,
                instance = 1 // instance_id for multiples like CPU[1], CPU[2]
            };
            response.sensors.Add(sensor);
			response.labels.Add("sensor.label");

            var volts = new MsmSensorReading(MsmSensorType.VOLT) {
                label = new MsmSensorLabel("CPU[" + sensor.instance + "]VOLT", "Central Processor Voltage"),
                id = id,
                unit = "Volt(s)",
                value = 1.154667,
                min = 1.228763,
                max = 1.354786,
                avg = 1.286443
            };
            response.readings.Add(volts);

            var rpm = new MsmSensorReading(MsmSensorType.FAN) {
                label = new MsmSensorLabel("CPU[" + sensor.instance + "]FAN", "Central Processor Fan Speed"),
                id = id,
                unit = "Rpm",
                value = 45.898765,
                min = 0.0,
                max = 305346.12312,
                avg = 40.73450
            };
            response.readings.Add(rpm);				
			
			return response;
		}
		
		public void dispose() {
			log.Debug("Cleaning up any used resources and shutting down...");
		}
		
	}
}
