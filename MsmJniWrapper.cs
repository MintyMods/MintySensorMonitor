using System;

namespace mintymods {

    public class MsmJniWrapper {

        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static String processRequest(String json) {
            log.Debug("MsmJniWrapper poll...");
            if (json == null) {
                log.Debug("Invalid JSON request specified : @JSON#" + json);
                var e = new MsmException("Invalid JSON Request Argument");
                e.hint.message = "You must provide a valid MSMRequest in a JSON format";
                e.hint.input = Newtonsoft.Json.JsonConvert.SerializeObject(json);
                e.hint.output = "Usage: new MsmJniWrapper().poll({json:here})";
                e.hint.result = "Example Usage: MsmJniWrapper().poll({help:true})";
                return Newtonsoft.Json.JsonConvert.SerializeObject(e);
            } else {
                try {
                     var monitor = new MintySenorMonitor(new MsmMonitorRequest(json));
                     return monitor.getSensorInfoAsJSON();
                } catch (MsmException e) {
                    log.Debug("@" + e.InnerException.Source + "#" + e.Message, e);
                    return Newtonsoft.Json.JsonConvert.SerializeObject(e);
                }
            }
        }
        
    }
}
