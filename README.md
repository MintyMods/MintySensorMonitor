##Minty Sensor Monitor 

[MSM](https://github.com/MintyMods/MintySM) has been designed to expose the various hardware sensor information already produced by your individual computer components in a consumable [JSON](https://www.json.org/) format.

#MSM Quest : Output a C# library call as JSON response to MsmMinty
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

# GOAL : Integration with HWiNFO
The larger project being [MSS](https://github.com/MintyMods/MintySS) requires integration between 


#For more information see the following projects:-
	* HWiNFO : [HWiNFO](https://www.hwinfo.com) 
	* Minty Sensor Server : [MSS](https://github.com/MintyMods/MintySS)
	* Minty Sensor Monitor : [MSM](https://github.com/MintyMods/MintySM)
	* Minty Query Language : [MQL](https://github.com/MintyMods/MintySQ)





SENSORS <-?-> HWiNFO <-?-> MSM[C#:SHM] <--> MSS[JNI:JSON] <--> API[JAVA:REST/JSOM/HTML]

This is done using C# <-JSON-> JNI <--> JAVA <--> HTTP(S) <--> HTML5



	
### MSM (This Project) Java --(Process Builder <-> EXE) --> C#

#MintySensorServer(MSS:Java) <json> ProcessBuilder <json> MintySensorMonitor(MSM:C#)
Basic mode of communication between the layers using the System.Console to exchange a JSON formatted request received via a single command line argument and returns a JSON formatted response (via Console.WriteLine)

	
Example:-

'''
MintySensorMonitor.exe {source:'MSM[JSON]EXAMPLE'}
'''

	
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
	
#License
Minty Sensor Monitor is licensed under The MIT License (MIT). Which means that you can use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software. But you always need to state that MintyMods is the original author of this template.
