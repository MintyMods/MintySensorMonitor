using System;

namespace mintymods {
	
	public interface MsmServiceInterface {
	
		MsmMonitorResponse poll();
		
		string getRequestMapping();
		
		void dispose();

	}
}
