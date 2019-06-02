using System;

namespace mintymods
{
	public class MsmMonitorRequest
	{

		public MsmMonitorRequest() {
			json = "NO_PARAMS";
		}
		
		public MsmMonitorRequest(String request) {
			json = request;
		}
		
		public string source = "MSS[JNI]";
		public string json;
		public string debug = "false";

	}
}
