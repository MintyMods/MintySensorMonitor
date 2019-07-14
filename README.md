# MSM - Minty Sensor Monitor 

[MSM](https://github.com/MintyMods/MintySensorMonitor) has been developed as the backend for [Minty Sensor Server](https://github.com/MintyMods/MintySensorServer). 
MSM consumes JSON requests for hardware sensor information exposed by 3rd party software monitoring components such as HWiNFO and formats this sensor data as a consumable [JSON](https://www.json.org/) response format.

## MSM Quest : Output a native C# library call as JSON response to MSS - Minty Sensor Server
Your current installed hardware components such as:-
* Motherboard
* CPU
* GPU
* MEMORY
* FAN
* PUMP

and more all expose useful information such as:-
* Volts
* Mhz
* °C
* RPM

This information can be utilised using 3rd party sofware such as:-
* [HWiNFO](https://www.hwinfo.com) 
* [GPU-Z](https://www.techpowerup.com/gpuz/) 
* [Open Hardware Monitor](https://openhardwaremonitor.org/) 

## GOAL : Integrate available sensor information with MSS - Minty Sensor Server 
The sister project [MSS - Minty Sensor Server](https://github.com/MintyMods/MintySS) requires integration between various 3rd party sensor information providers who mostly output this sensor information via native C# libraries or directly via Shared Memory access. 
As MSS has been developed using JAVA it is difficult to consume this information directly so MSM provides a bridge between these native C# software components and JAVA via a simple JSON request / response model.

## For more information see the following projects:-
* MSS : [Minty Sensor Server](https://github.com/MintyMods/MintySensorServer)
* MSM : [Minty Sensor Monitor](https://github.com/MintyMods/MintySensorMonitor)
* MSM2MSS : [Minty Sensor Monitor 2 Minty Sensor Server JNI Wrapper](https://github.com/MintyMods/MintySm2MintySsJniWrapper)



## This Project (MSM) C# <--> (MSM2MSS) C++ <--> (MSS) Java 

 - **Full Project Stack:-**
	- Raw Sensor Information 
	- <--?:Sensor Information Providers:?-->
	- <--*SHM::DLL*--> 
	- **MsmServiceInterface** 
	- <--*MSMRequest::MSMResponse*--> 
	- **MSM**[C#] 
	- <--*MSMRequest::MSMResponse*--> 
	- **MSM2MSS**[C++] 
	- <--*MSMRequest::MSMResponse*--> 
	- **MSS**[JNI] 
	- <--*MSMRequest::MSMResponse*--> 
	- **API**[JAVA:REST/JSON]
	- <--HTTP::HTML-->


## MSMRequest 
	
```
{"debug":true} 
```

## Example MsmMonitorResponse 

```
{  
   "sensors":[  
      {  
         "label":{  
            "value":"value",
            "description":"description"
         },
         "id":12345,
         "instance":1
      }
   ],
   "readings":[  
      {  
         "type":2,
         "label":{  
            "value":"CPU[1]VOLT",
            "description":"Central Processor Voltage"
         },
         "id":12345,
         "sensor_index":1,
         "unit":"Volt(s)",
         "value":1.154667,
         "min":1.228763,
         "max":1.354786,
         "avg":1.286443
      },
      {  
         "type":3,
         "label":{  
            "value":"CPU[1]FAN",
            "description":"Central Processor Fan Speed"
         },
         "id":12345,
         "sensor_index":1,
         "unit":"Rpm",
         "value":45.898765,
         "min":0.0,
         "max":305346.12312,
         "avg":40.7345
      }
   ],
   "exception":null,
   "labels":[  
      "sensor.label"
   ],
   "type":"MsmMonitorResponse",
   "source":"MSM[JSON]EXAMPLE",
   "version":"1.0",
   "debug":false,
   "time_taken":"00:00:00.0000015"
}

```
[HWiNFO64 MsmMonitorResponse](docs/MSMReponseJsonHWiNFOExample.md)

## Acknowledgments

  * HWiNFO - Martin Malik for his help with the sensor integration [HWiNFO](http://hwinfo.com)
  
  
	
### License
MSM is licensed under The [GNU General Public License version 3](https://www.gnu.org/licenses/gpl-3.0.en.html).
