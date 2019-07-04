using System;

namespace mintymods {

	public static class MsmLogging {
				
		static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		public static string LEVEL = "DEBUG";
		
		public static void configureLogging() {
			configureLogging(true);
		}
		
		public static void configureLogging(bool debug) {
			if (debug) {
				setLoggingLevel(LEVEL);
				((log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository()).RaiseConfigurationChanged(EventArgs.Empty);
				log.Info(" ** Debug logging enabled ** ");
			} 
			
		}
		
		public static void setLoggingLevel(string level) {
            
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
		
	}
}
