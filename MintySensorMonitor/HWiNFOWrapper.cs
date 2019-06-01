using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace com.mintymods.msm {

	public class HWiNFOWrapper {
		
		public HWiNFOWrapper(MsmMonitorResponse response) {
			this.response = response;
		}
		
        public const string HWiNFO_SENSORS_MAP_FILE_NAME2 = "Global\\HWiNFO_SENS_SM2";
        public const string HWiNFO_SENSORS_SM2_MUTEX = "Global\\HWiNFO_SM2_MUTEX";
        public const int HWiNFO_SENSORS_STRING_LEN2 = 128;
        public const int HWiNFO_UNIT_STRING_LEN = 16;
		public MsmMonitorResponse response;
		
        public enum SENSOR_READING_TYPE {
            SENSOR_TYPE_NONE = 0,
            SENSOR_TYPE_TEMP,
            SENSOR_TYPE_VOLT,
            SENSOR_TYPE_FAN,
            SENSOR_TYPE_CURRENT,
            SENSOR_TYPE_POWER,
            SENSOR_TYPE_CLOCK,
            SENSOR_TYPE_USAGE,
            SENSOR_TYPE_OTHER
        };
		  
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct _HWiNFO_SENSORS_READING_ELEMENT {
            public SENSOR_READING_TYPE tReading;
            public UInt32 dwSensorIndex;
            public UInt32 dwReadingID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = HWiNFO_SENSORS_STRING_LEN2)]
            public string szLabelOrig;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = HWiNFO_SENSORS_STRING_LEN2)]
            public string szLabelUser;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = HWiNFO_UNIT_STRING_LEN)]
            public string szUnit;
            public double Value;
            public double ValueMin;
            public double ValueMax;
            public double ValueAvg;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct _HWiNFO_SENSORS_SENSOR_ELEMENT {
           public UInt32 dwSensorID;
           public UInt32 dwSensorInst;
           [MarshalAs(UnmanagedType.ByValTStr, SizeConst = HWiNFO_SENSORS_STRING_LEN2)]
            public string szSensorNameOrig;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = HWiNFO_SENSORS_STRING_LEN2)]
            public string szSensorNameUser;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct _HWiNFO_SENSORS_SHARED_MEM2 {
            public UInt32 dwSignature;
            public UInt32 dwVersion;
            public UInt32 dwRevision;
            public long poll_time;
            public UInt32 dwOffsetOfSensorSection;
            public UInt32 dwSizeOfSensorElement;
            public UInt32 dwNumSensorElements;
            // descriptors for the Readings section
            public UInt32 dwOffsetOfReadingSection; // Offset of the Reading section from beginning of HWiNFO_SENSORS_SHARED_MEM2
            public UInt32 dwSizeOfReadingElement;   // Size of each Reading element = sizeof( HWiNFO_SENSORS_READING_ELEMENT )
            public UInt32 dwNumReadingElements;     // Number of Reading elements
        };
    

	    MemoryMappedFile mmf;
	    private uint numSensors;
		private uint numReadingElements;
        private uint offsetSensorSection;
        private uint sizeSensorElement;
        private uint offsetReadingSection;
        private uint sizeReadingSection;
        
	bool debug = true;	
        
        public void Open() {
        
            try {
                mmf = MemoryMappedFile.OpenExisting(HWiNFO_SENSORS_MAP_FILE_NAME2, MemoryMappedFileRights.Read);
            } catch (Exception e) {
        		response.exception = new MsmException("Error opening HWiNFO shared memory!", e);
                Console.ReadLine();
                Environment.Exit(1);
            }
		
		
		
            using (var accessor = mmf.CreateViewAccessor(0, Marshal.SizeOf(typeof(_HWiNFO_SENSORS_SHARED_MEM2)), MemoryMappedFileAccess.Read)) {
			    _HWiNFO_SENSORS_SHARED_MEM2 HWiNFOMemory ;
                accessor.Read(0, out HWiNFOMemory);
			    numSensors = HWiNFOMemory.dwNumSensorElements ;
                numReadingElements = HWiNFOMemory.dwNumReadingElements;
			    offsetSensorSection = HWiNFOMemory.dwOffsetOfSensorSection ;
                sizeSensorElement = HWiNFOMemory.dwSizeOfSensorElement;
                offsetReadingSection = HWiNFOMemory.dwOffsetOfReadingSection;
                sizeReadingSection = HWiNFOMemory.dwSizeOfReadingElement;

                
                
			    for (UInt32 dwSensor = 0; dwSensor < numSensors; dwSensor ++) {
                    using (var sensor_element_accessor = mmf.CreateViewStream(offsetSensorSection + (dwSensor * sizeSensorElement), sizeSensorElement, MemoryMappedFileAccess.Read)) {
                    
                        byte[] byteBuffer = new byte[sizeSensorElement];
                        sensor_element_accessor.Read(byteBuffer,0,(int)sizeSensorElement);
                        GCHandle handle = GCHandle.Alloc(byteBuffer, GCHandleType.Pinned);
                        _HWiNFO_SENSORS_SENSOR_ELEMENT SensorElement = (_HWiNFO_SENSORS_SENSOR_ELEMENT)Marshal.PtrToStructure(handle.AddrOfPinnedObject(),
                            typeof(_HWiNFO_SENSORS_SENSOR_ELEMENT));
                        handle.Free();
                        
                        response.names.Add(SensorElement.szSensorNameUser);
                        
                        Sensor sensor = new Sensor();
                        sensor.id = SensorElement.dwSensorID;
                        sensor.instance = (int)SensorElement.dwSensorInst; // dwSensorInst
						sensor.name = SensorElement.szSensorNameOrig;
						sensor.label = SensorElement.szSensorNameUser;
						response.sensors.Add(sensor);
                        
						if (debug) {
						    Console.WriteLine(String.Format("dwSensorID : {0}", SensorElement.dwSensorID));
						    Console.WriteLine(String.Format("dwSensorInst : {0}", SensorElement.dwSensorInst));
						    Console.WriteLine(String.Format("szSensorNameOrig : {0}", SensorElement.szSensorNameOrig));
						    Console.WriteLine(String.Format("szSensorNameUser : {0}", SensorElement.szSensorNameUser));                        
						}
                    }
                }
                
                for (UInt32 dwReading = 0; dwReading < numReadingElements; dwReading++) {
                    using (var sensor_element_accessor = mmf.CreateViewStream(offsetReadingSection + (dwReading * sizeReadingSection), sizeReadingSection, MemoryMappedFileAccess.Read)) {
                        byte[] byteBuffer = new byte[sizeReadingSection];
                        sensor_element_accessor.Read(byteBuffer,0,(int)sizeReadingSection);
                        GCHandle handle = GCHandle.Alloc(byteBuffer, GCHandleType.Pinned);
                        _HWiNFO_SENSORS_READING_ELEMENT ReadingElement = (_HWiNFO_SENSORS_READING_ELEMENT)Marshal.PtrToStructure(handle.AddrOfPinnedObject(),
                            typeof(_HWiNFO_SENSORS_READING_ELEMENT));
                        if (debug) {
	                        Console.WriteLine(String.Format("tReading sensor type : {0}", ReadingElement.tReading));
	                        Console.WriteLine(String.Format("dwSensorIndex : {0} ; Sensor Name: {1}", ReadingElement.dwSensorIndex, response.names[(int)ReadingElement.dwSensorIndex]));
	                        Console.WriteLine(String.Format("dwReadingID : {0}", ReadingElement.dwSensorIndex));
	                        Console.WriteLine(String.Format("szLabelUser : {0}", ReadingElement.szLabelUser));
	                        Console.WriteLine(String.Format("szUnit : {0}", ReadingElement.szUnit));
	                        Console.WriteLine(String.Format("Value : {0}", ReadingElement.Value));
                        }
                        handle.Free();
                       // response.stats.Add(new SensorReading(ReadingElement));
                    }
                }
            }
 	    }
        
	    public void Close() {
            if (mmf != null) {
                mmf.Dispose();
            }
        }
		
	}
}

// ***************************************************************************************************************
//                                          HWiNFO Shared Memory Footprint
// ***************************************************************************************************************
//
//         |-----------------------------|-----------------------------------|-----------------------------------|
// Content |  HWiNFO_SENSORS_SHARED_MEM2 |  HWiNFO_SENSORS_SENSOR_ELEMENT[]  | HWiNFO_SENSORS_READING_ELEMENT[]  |
//         |-----------------------------|-----------------------------------|-----------------------------------|
// Pointer |<--0                         |<--dwOffsetOfSensorSection         |<--dwOffsetOfReadingSection        |
//         |-----------------------------|-----------------------------------|-----------------------------------|
// Size    |  dwOffsetOfSensorSection    |   dwSizeOfSensorElement           |    dwSizeOfReadingElement         |
//         |                             |      * dwNumSensorElement         |       * dwNumReadingElement       |
//         |-----------------------------|-----------------------------------|-----------------------------------|
//	
		