using System;
using System.IO;
using Microsoft.CSharp.RuntimeBinder;
using System.Web.Script.Serialization;



namespace mintymods {
	
	public class MsmMonitorRequest {
		
		MsmMonitorResponse respose;
		public string type;
		public string source = "MSS[EXE]";
		public bool debug = false;

		public MsmMonitorRequest(String json) {
			this.respose = new MsmMonitorResponse(this);
			type = this.GetType().Name;
			
			try {
				var request = Newtonsoft.Json.JsonConvert.SerializeObject(json);
				Console.WriteLine("REQUEST:-");		
				Console.WriteLine(request);		

				MsmMonitorRequest response = Newtonsoft.Json.JsonConvert.DeserializeObject<MsmMonitorRequest>(request);
				Console.WriteLine("RESPONSE:-");		
				Console.WriteLine(response);		

			} catch (Exception e) {
				Console.WriteLine("Exception" );	
				MsmException exception = new MsmException("Unable to parse JSON request", e);
				exception.hint.message = "Your JSON parameter was malformed";
				exception.hint.input = new JavaScriptSerializer().Serialize(json);
				exception.hint.output = e.Message;
				exception.hint.result = "################";
				exception.exception = e;
				throw e;
			}

		}

	
	}
	
}
