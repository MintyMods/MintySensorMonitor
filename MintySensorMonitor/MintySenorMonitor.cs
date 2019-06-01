using System.Web.Script.Serialization;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace com.mintymods.msm {
	/***************************************************************************************************************
	 * MSM (Minty Sensor Monitor) is the integration between HWiNFO and MSS (Minty's Sensor Server)
	 * SENSORS <-?-> HWiNFO <-?-> MSM[C#:SHM] <--> MSS[JNI:JSON] <--> API[JAVA:REST/JSOM/HTML]
	 * This is done using C# <-JSON-> JNI <--> JAVA <--> HTTP(S) <--> HTML5
	 * For more information see the following projects:-
	 * MSM:https://github.com/MintyMods/MintySM
	 * MSS:https://github.com/MintyMods/MintySS
	/***************************************************************************************************************/
	public class MintySenorMonitor {
		
        public string GetSensorInfoAsJSON() {
        	MsmMonitorResponse response = new MsmMonitorResponse();
        	HWiNFOWrapper wrapper = new HWiNFOWrapper(response);
        	string json = "";
        	try {
        		wrapper.Open();	
        		json = new JavaScriptSerializer().Serialize(response);
        	} catch(MsmException e) {
        		response.exception = e;
        		json = new JavaScriptSerializer().Serialize(response);
        	} finally {
        		wrapper.Close();	
        	}
        	Debug.WriteLine(json);
			return json;
        }
	}
}

	