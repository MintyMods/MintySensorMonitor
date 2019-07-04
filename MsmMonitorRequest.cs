using System;

namespace mintymods {
	
	public class MsmMonitorRequest : MsmMonitorRequestParameters {
		
		static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
	 
		public MsmMonitorRequest(String json) {
			log.Debug("@JSON#" + json);
			type = GetType().Name;
			
			try {

				MsmLogging.configureLogging();
				MsmMonitorRequestParameters data = Newtonsoft.Json.JsonConvert.DeserializeObject<MsmMonitorRequestParameters>(json);
				debugRequestJsonData(data);
				this.source = data.source;
				this.type = data.type;	
				this.debug = data.debug;
				this.help = data.help;

			} catch (Exception exception) {
				log.Debug("Unable to parse JSON request @#");
				var e = new MsmException("Unable to parse JSON request", exception);
				e.hint.message = "Your JSON parameter was malformed";
				e.hint.input = Newtonsoft.Json.JsonConvert.SerializeObject(json);
				e.hint.output = e.Message;
				e.exception = e;
				log.Debug(e);
			}

		}
		
		void debugRequestJsonData(MsmMonitorRequestParameters parameters) {
			log.Debug("@MsmMonitorRequest()#" + parameters);	
			log.Debug("@SOURCE#" + parameters.source);	
			log.Debug("@TYPE#" + parameters.type);	
			log.Debug("@DEBUG#" + parameters.debug);	
			log.Debug("@HELP#" + parameters.help);				
		}
	
	}
	
}
