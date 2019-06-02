using System;
using System.Web.Script.Serialization;

// Java --(JNA)-->  C/C++ --(COM Interop)--> C#
namespace mintymods
{
	public class MSM
	{

		public static void Main(string[] args)
		{
			var request = "REQUEST_NOT_SPECIFIED";
			if (args.Length > 0) {
				if (args.Length == 1) {
					request = new JavaScriptSerializer().Serialize(args[0]);
				} 
				if (args.Length > 1) {
					request = "INVALID_REQUEST:ARGUMENTS:" + new JavaScriptSerializer().Serialize(args);
				}
			}
    	Console.WriteLine("{0}", MSM.getJson(request));
    }
      
    public static string getJson(String json)
		{
			var monitor = new MintySenorMonitor();
    		var request = new MsmMonitorRequest(json);
			request.source = "MSS[EXE]";
			return monitor.getSensorInfoAsJSON(request);
		}
	}
}
