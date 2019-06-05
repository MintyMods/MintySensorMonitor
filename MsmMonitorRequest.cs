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
				configureLoggingLevel();

			} catch (Exception exception) {
				log.Debug("Unable to parse JSON request @#");
				var e = new MsmException("Unable to parse JSON request", exception);
				e.hint.message = "Your JSON parameter was malformed";
				e.hint.input = new JavaScriptSerializer().Serialize(json);
				e.hint.output = e.Message;
				e.exception = e;
				log.Error(e);
			}

		}
		
		void configureLoggingLevel() {
				
			if (debug) {
				setLoggingLevel("DEBUG");
				((log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository()).RaiseConfigurationChanged(EventArgs.Empty);
				log.Info(" ** Debug logging enabled ** ");
			} 
			
		}
		
		void setLoggingLevel(string level) {
            
            log4net.Repository.ILoggerRepository[] repositories = log4net.LogManager.GetAllRepositories();

            foreach (log4net.Repository.ILoggerRepository repository in repositories) {
                repository.Threshold = repository.LevelMap[level];
                log4net.Repository.Hierarchy.Hierarchy hier = (log4net.Repository.Hierarchy.Hierarchy)repository;
                log4net.Core.ILogger[] loggers=hier.GetCurrentLoggers();
                foreach (log4net.Core.ILogger logger in loggers) {
                    ((log4net.Repository.Hierarchy.Logger) logger).Level = hier.LevelMap[level];
                }
            }

            log4net.Repository.Hierarchy.Hierarchy h = (log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository();
            log4net.Repository.Hierarchy.Logger rootLogger = h.Root;
            rootLogger.Level = h.LevelMap[level];
        }		
		
		void debugRequestJsonData(MsmMonitorRequestParameters parameters) {
			log.Debug("@MsmMonitorRequest()#" + parameters);	
			log.Debug("@TYPE#" + parameters.type);	
			log.Debug("@DEBUG#" + parameters.debug);	
			log.Debug("@HELP#" + parameters.help);				
		}
	
	}
	
}
