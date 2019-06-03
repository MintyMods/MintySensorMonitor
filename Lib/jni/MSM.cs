using System;

namespace mintymods
{

	public class MSM
	{
		public MSM {
			
		}
		
		public string getJson(String request)
		{
			MintySenorMonitor monitor = new MintySenorMonitor();
			return monitor.getSensorInfoAsJSON(request);
		}
	}
}
