using System;

namespace mintymods {
	
	public class MsmMonitorRequestParameters {
		
		public string type = "";
		public string source = "MSS[EXE]";
		public bool debug = false;
		public bool help = false;
		
		public MsmMonitorRequestParameters()	{
			type = this.GetType().Name;
		}
		
	}
}
