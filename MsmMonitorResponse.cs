using System;
using System.Collections.Generic;

namespace mintymods
{
	public class MsmMonitorResponse
	{
		public string source = "MSM[SHM]HWiNFO";
		public MsmMonitorRequest request;
		public List<string> names;
		public List<Sensor> sensors;
		public List<SensorReading> stats;
		public MsmException exception;
		public string version = "0.3A";
		public bool debug;
		public long time_taken_ms;

		public MsmMonitorResponse(MsmMonitorRequest jsonRequest)
		{
			request = jsonRequest;
			names = new List<string>();
			sensors = new List<Sensor>();
			stats = new List<SensorReading>();
		}
	}
}
