using System;
using System.Web.Script.Serialization;

namespace mintymods {
	
	public class MsmMonitorRequest : MsmMonitorRequestParameters {
		
		static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
	 
		public MsmMonitorRequest(String json) {
			log.Debug("@JSON#" + json);
			type = GetType().Name;
			
			try {
				
				MsmMonitorRequestParameters data = Newtonsoft.Json.JsonConvert.DeserializeObject<MsmMonitorRequestParameters>(json);
				debugRequestJsonData(data);
				this.type = data.type;	
				this.debug = data.debug;
				this.help = data.help;

			} catch (Exception e) {
				log.Debug("Unable to parse JSON request @#");
				var exception = new MsmException("Unable to parse JSON request", e);
				exception.hint.message = "Your JSON parameter was malformed";
				exception.hint.input = new JavaScriptSerializer().Serialize(json);
				exception.hint.output = e.Message;
				exception.hint.result = "################"; //@todo
				exception.exception = e;
				log.Error(exception);
				throw e;
			}

		}
		
		void debugRequestJsonData(MsmMonitorRequestParameters parameters) {
			log.Debug("@MsmMonitorRequest()#" + parameters);	
			log.Debug("@TYPE#" + parameters.type);	
			log.Debug("@DEBUG#" + parameters.debug);	
			log.Debug("@HELP#" + parameters.help);				
		}
	
	}
	
}
