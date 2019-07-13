# MSM - Minty Sensor Monitor 

[MSM](https://github.com/MintyMods/MintySensorMonitor) has been developed as the backend for Minty Sensor Server. 
MSM consumes requests for various hardware sensor information exposed by 3rd party hardware monitor components such as HWiNFO and formats these into a consumable [JSON](https://www.json.org/) response format.

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
* MSM2MSS : [Minty Sensor Monitor 2 Minty Sensor Server JNI Wrapper](https://github.com/MintyMods/MSM2MSS)



## This Project (MSM) C# <--> (MSM2MSS) C++ <--> (MSS) Java 

SENSORS <-?-> HWiNFO <-SHM-> MSM[C#] <-JSON-> MSM2MSS[C++] <-JSON-> MSS[JNI] <-JSON-> API[JAVA:REST/JSON/HTML]

## MintySensorServer(MSS:Java) <json> ProcessBuilder <json> MintySensorMonitor(MSM:C#)
Basic mode of communication between the layers using the System.Console to exchange a JSON formatted request received via a single command line argument and returns a JSON formatted response (via Console.WriteLine)

	
Example:-


	
	JSON Result:-

{ 
   "sensors":[ 
      {  
         "label":{  

         },
         "id":12345,
         "instance":0,
         "name":"EXAMPLE:CPU"
      }
   ],
   "readings":[ 
      {  
         "label":{  

         },
         "id":12345,
         "sensor_index":1,
         "unit":"Volt(s)",
         "value":100.0,
         "min":0.0,
         "max":0.0,
         "avg":60.0
      },
      {  
         "label":{  

         },
         "id":12345,
         "sensor_index":1,
         "unit":"Rpm",
         "value":12345.098765,
         "min":0.12312,
         "max":0.0,
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
   "time_taken_ms":0
}

	Example:-
	
	* MintySensorMonitor.exe {source:'MSM[JSON]EXAMPLE',debug:true}
	
	Logging Output:

 INFO mintymods.MsmMonitorRequest -  ** Debug logging enabled **
 DEBUG mintymods.MsmServiceController - @SERVICE#MsmServiceExample
 DEBUG mintymods.MsmServiceExample - Request received @SOURCE#MSM[JSON]EXAMPLE
 DEBUG mintymods.MsmServiceExample - Cleaning up any used resources and shutting down...
 DEBUG mintymods.MintySenorMonitor - {"sensors":[{"label":{},"id":12345,"instance":0,"name":"EXAMPLE:CPU"}],"readings":[{"label":{},"id":12345,"sensor_index":1,"unit":"Volt(s)","value":100.0,"min":0.0,"max":0.0,"avg":60.0},{"label":{},"id":12345,"sensor_index":1,"unit":"Rpm","value":12345.098765,"min":0.12312,"max":0.0,"avg":40.7345}],"exception":null,"labels":["sensor.label"],"type":"MsmMonitorResponse","source":"MSM[JSON]EXAMPLE","version":"1.0","debug":false,"time_taken_ms":0}

	JSON Result:-

{  
   "sensors":[  
      {  
         "label":{  

         },
         "id":12345,
         "instance":0,
         "name":"EXAMPLE:CPU"
      }
   ],
   "readings":[  
      {  
         "label":{  

         },
         "id":12345,
         "sensor_index":1,
         "unit":"Volt(s)",
         "value":100.0,
         "min":0.0,
         "max":0.0,
         "avg":60.0
      },
      {  
         "label":{  

         },
         "id":12345,
         "sensor_index":1,
         "unit":"Rpm",
         "value":12345.098765,
         "min":0.12312,
         "max":0.0,
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
   "time_taken_ms":0
}
	
	
	* System.in/System.out (Comnmand Line Console) to interchange data between MSS(JAVA) and MSM(C#) via JSON formatted request/response messages. 
	
	
	// @todo ::  MSS:Java --(JNA)-->  C/C++ --(COM Interop)--> MSM:C#
	
	// @todo ::  MSS:Java --(TCP)-->  HWiNFO


## Acknowledgments

  * HWiNFO - Martin Malik for his help with the sensor integration [HWiNFO](http://hwinfo.com)
  
  
	
### License
MSM is licensed under The [GNU General Public License version 3](https://www.gnu.org/licenses/gpl-3.0.en.html).
