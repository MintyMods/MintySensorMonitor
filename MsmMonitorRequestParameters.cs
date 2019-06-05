using System;

namespace mintymods {
	
	public class MsmMonitorRequestParameters {
		
		public string type = "";
		public string source = "MSS[EXE]";
		public bool debug;
		public bool help;
		
		public MsmMonitorRequestParameters()	{
			type = this.GetType().Name;
		}
		
	}
}
