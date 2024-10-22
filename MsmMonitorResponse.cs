﻿using System;
using System.Collections.Generic;

namespace mintymods {
	
	public class MsmMonitorResponse {
		
		static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		public List<MsmSensor> sensors;
		public List<MsmSensorReading> readings;
		public MsmException exception;
		public List<string> labels;
		public string type;
		public string source = "MSM[SERVICE]";
		public string version = "1.0";
		public bool debug;
        public TimeSpan time_taken;
        public UInt32 polling_period;

        public MsmMonitorResponse() {

			labels = new List<string>();
			sensors = new List<MsmSensor>();
			readings = new List<MsmSensorReading>();
			type = GetType().Name;
			
		}
		
	}
	
}
