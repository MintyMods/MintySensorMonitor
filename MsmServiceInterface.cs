using System;

namespace mintymods {
	
	public interface MsmServiceInterface {
	
		MsmMonitorResponse poll();
			
		void dispose();

	}
}
