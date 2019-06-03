using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace mintymods {
	
	public class HWiNFOWrapper 	{
		
		public const string HWiNFO_SHM_NAME = "Global\\HWiNFO_SENS_SM2";
		public const string HWiNFO_SHM_MUTEX = "Global\\HWiNFO_SM2_MUTEX";
		public const int HWiNFO_SENSORS_LENGTH = 128;
		public const int HWiNFO_UNIT_LENGTH = 16;
		public MsmMonitorRequest request;
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
		public struct _HWiNFO_READING_ELEMENT {
			public SENSOR_READING_TYPE tReading;
			public UInt32 dwSensorIndex;
			public UInt32 dwReadingID;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = HWiNFO_SENSORS_LENGTH)]
			public string szLabelOrig;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = HWiNFO_SENSORS_LENGTH)]
			public string szLabelUser;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = HWiNFO_UNIT_LENGTH)]
			public string szUnit;
			public double Value;
			public double ValueMin;
			public double ValueMax;
			public double ValueAvg;
		};

		[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
		public struct _HWiNFO_SENSOR_ELEMENT {
			public UInt32 dwSensorID;
			public UInt32 dwSensorInst;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = HWiNFO_SENSORS_LENGTH)]
			public string szSensorNameOrig;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = HWiNFO_SENSORS_LENGTH)]
			public string szSensorNameUser;
		};

		[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
		public struct _HWiNFO_SHM {
			public UInt32 dwSignature;
			public UInt32 dwVersion;
			public UInt32 dwRevision;
			public long poll_time;
			public UInt32 dwOffsetOfSensorSection;
			public UInt32 dwSizeOfSensorElement;
			public UInt32 dwNumSensorElements;
			public UInt32 dwOffsetOfReadingSection;
			public UInt32 dwSizeOfReadingElement;
			public UInt32 dwNumReadingElements;
		};
	
		MemoryMappedFile mmf;
		private uint numSensors;
		private uint numReadingElements;
		private uint offsetSensorSection;
		private uint sizeSensorElement;
		private uint offsetReadingSection;
		private uint sizeReadingSection;

		public HWiNFOWrapper(MsmMonitorRequest request, MsmMonitorResponse response) {
			this.request = request;
			this.response = response;
		}
		
		public MsmMonitorResponse poll() {
			
			try {
				mmf = MemoryMappedFile.OpenExisting(HWiNFO_SHM_NAME, MemoryMappedFileRights.Read);
				using (var accessor = mmf.CreateViewAccessor(0, Marshal.SizeOf(typeof(_HWiNFO_SHM)), MemoryMappedFileAccess.Read)) {
					_HWiNFO_SHM HWiNFOMemory;
					accessor.Read(0, out HWiNFOMemory);
					numSensors = HWiNFOMemory.dwNumSensorElements;
					numReadingElements = HWiNFOMemory.dwNumReadingElements;
					offsetSensorSection = HWiNFOMemory.dwOffsetOfSensorSection;
					sizeSensorElement = HWiNFOMemory.dwSizeOfSensorElement;
					offsetReadingSection = HWiNFOMemory.dwOffsetOfReadingSection;
					sizeReadingSection = HWiNFOMemory.dwSizeOfReadingElement;
					
					for (UInt32 dwSensor = 0; dwSensor < numSensors; dwSensor++) {
						using (var sensor_element_accessor = mmf.CreateViewStream(offsetSensorSection + (dwSensor * sizeSensorElement), sizeSensorElement, MemoryMappedFileAccess.Read)) {
							byte[] byteBuffer = new byte[sizeSensorElement];
							sensor_element_accessor.Read(byteBuffer, 0, (int)sizeSensorElement);
							GCHandle handle = GCHandle.Alloc(byteBuffer, GCHandleType.Pinned);
							try {
								_HWiNFO_SENSOR_ELEMENT SensorElement = (_HWiNFO_SENSOR_ELEMENT)Marshal.PtrToStructure(handle.AddrOfPinnedObject(),
									                                       typeof(_HWiNFO_SENSOR_ELEMENT));
								if (request.debug) {
									debugSensorElements(SensorElement);
								}
								
								response.names.Add(SensorElement.szSensorNameUser);
								
								MsmSensor sensor = new MsmSensor();
								sensor.id = SensorElement.dwSensorID;
								sensor.instance = (int)SensorElement.dwSensorInst;
								sensor.name = SensorElement.szSensorNameOrig;
								sensor.label = SensorElement.szSensorNameUser;
								response.sensors.Add(sensor);
								
							} catch (Exception e) {
								response.exception = new MsmException("Error processing Sensor Elements", e);							
								throw e;
							} finally {
								handle.Free();
							}
						}
					}
					
					for (UInt32 dwReading = 0; dwReading < numReadingElements; dwReading++) {
						using (var sensor_element_accessor = mmf.CreateViewStream(offsetReadingSection + (dwReading * sizeReadingSection), sizeReadingSection, MemoryMappedFileAccess.Read)) {
							byte[] byteBuffer = new byte[sizeReadingSection];
							sensor_element_accessor.Read(byteBuffer, 0, (int)sizeReadingSection);
							GCHandle handle = GCHandle.Alloc(byteBuffer, GCHandleType.Pinned);
							try {	
								_HWiNFO_READING_ELEMENT ReadingElement = (_HWiNFO_READING_ELEMENT)Marshal.PtrToStructure(handle.AddrOfPinnedObject(),
									                                         typeof(_HWiNFO_READING_ELEMENT));
								if (request.debug) {
									debugSensorReadings(ReadingElement);
								}
								
								MsmSensorReading reading = new MsmSensorReading();
								reading.type = (MsmSensorType)ReadingElement.tReading;
								reading.id = ReadingElement.dwReadingID;
								reading.index = (int)ReadingElement.dwSensorIndex;
								reading.label = (string)ReadingElement.szLabelUser;
								reading.unit = (string)ReadingElement.szUnit;
								reading.value = (Double)ReadingElement.Value;
								response.stats.Add(reading);
								
							} catch (Exception e) {
								response.exception = new MsmException("Error processing Reading Elements", e);
								throw e;
							} finally {
								handle.Free();
							}
						}
					}
				}
			} catch (Exception e) {
				response.exception = new MsmException("Error opening HWiNFO shared memory!", e);
				throw e;
			}
			return response;
		}
		
		public void Dispose() {
			if (mmf != null) {
				mmf.Dispose();
			}
		}
		
		public void debugSensorElements(_HWiNFO_SENSOR_ELEMENT SensorElement) {
			Console.WriteLine(String.Format("dwSensorID : {0}", SensorElement.dwSensorID));
			Console.WriteLine(String.Format("dwSensorInst : {0}", SensorElement.dwSensorInst));
			Console.WriteLine(String.Format("szSensorNameOrig : {0}", SensorElement.szSensorNameOrig));
			Console.WriteLine(String.Format("szSensorNameUser : {0}", SensorElement.szSensorNameUser));						
		}
		
		public void debugSensorReadings(_HWiNFO_READING_ELEMENT ReadingElement) {
			Console.WriteLine(String.Format("tReading sensor type : {0}", ReadingElement.tReading));
			Console.WriteLine(String.Format("dwSensorIndex : {0} ; Sensor Name: {1}", ReadingElement.dwSensorIndex, response.names[(int)ReadingElement.dwSensorIndex]));
			Console.WriteLine(String.Format("dwReadingID : {0}", ReadingElement.dwSensorIndex));
			Console.WriteLine(String.Format("szLabelUser : {0}", ReadingElement.szLabelUser));
			Console.WriteLine(String.Format("szUnit : {0}", ReadingElement.szUnit));
			Console.WriteLine(String.Format("Value : {0}", ReadingElement.Value));
		}
		
	}
	
}