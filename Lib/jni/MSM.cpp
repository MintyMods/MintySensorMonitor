//.cpp file code:

#include "string.h"
#include "string"

namespace mintymods
{

class MSM
{
public:
    virtual ~MSM() {}
    virtual string getJson() = "";
};

	std::string getJson(const std::string &request)
	{
		MintySenorMonitor *monitor = new MintySenorMonitor();
		delete monitor;
		return monitor->getSensorInfoAsJSON(request);
	}
}
