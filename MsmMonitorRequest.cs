using System;

namespace com.mintymods.msm
{
	public class MsmMonitorRequest
	{
	
		public string source = "MSS[JNI]";
		public string guid = System.Guid.NewGuid().ToString();
		public bool debug = false;

	}
}
