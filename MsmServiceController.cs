using System;
using MintyServiceHWiNFO;

namespace mintymods {

	public class MsmServiceController {
		
		static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		
		public MsmServiceInterface getMonitorForRequest(MsmMonitorRequest request) {
			
			if (MsmServiceHWiNFO.REQUEST_MAPPING.Equals(request.source, StringComparison.InvariantCultureIgnoreCase)) {
				log.Debug("@SERVICE#MsmServiceHWiNFO");
				return new MsmServiceHWiNFO(request);
			} else if (MsmServiceExample.REQUEST_MAPPING.Equals(request.source, StringComparison.InvariantCultureIgnoreCase)) {
				log.Debug("@SERVICE#MsmServiceExample");
				return new MsmServiceExample(request);
			} else {
				logUnknownServiceRequested(request);
				request.source = MsmServiceExample.REQUEST_MAPPING;
				return new MsmServiceExample(request);
			}
			
		}
		
		void logUnknownServiceRequested(MsmMonitorRequestParameters request) {
			var e = new MsmException("Sending an dummy request to MsmServiceExample for an example response");
			log.Debug("Unknown Service Requested " + request.source);
			e.hint.message = "You must provide a valid 'source' within the MsmMonitorRequest";
			e.hint.input = Newtonsoft.Json.JsonConvert.SerializeObject(request);
			e.hint.output = "@SOURCE#" + request.source;
			e.hint.result = "@EXAMPLE#" + Newtonsoft.Json.JsonConvert.SerializeObject(getExampleJson()); 
			log.Debug(e);
		}
		
		MsmMonitorRequestParameters getExampleJson() {
            var example = new MsmMonitorRequestParameters {
                source = MsmServiceExample.REQUEST_MAPPING,
                debug = true,
                help = true
            };
            return example;
		}
		
	}
}
