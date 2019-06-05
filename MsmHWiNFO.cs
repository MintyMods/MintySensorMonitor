using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace mintymods {
	
	public class MsmHWiNFO 	{
	
		static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		public const string HWiNFO_SHM_NAME = "Global\\HWiNFO_SENS_SM2";
		public const string HWiNFO_SHM_MUTEX = "Global\\HWiNFO_SM2_MUTEX";
		public const int HWiNFO_SENSORS_LENGTH = 128;
		public const int HWiNFO_UNIT_LENGTH = 16;
		
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
		MsmMonitorRequest request;
		uint numSensors;
		uint numReadingElements;
		uint offsetSensorSection;
		uint sizeSensorElement;
		uint offsetReadingSection;
		uint sizeReadingSection;

		
		public MsmHWiNFO(MsmMonitorRequest request) {
			this.request = request;
		}
		
		public MsmMonitorResponse poll() {
			var response = new MsmMonitorResponse();
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

								debugSensorElements(SensorElement);
								response.labels.Add(SensorElement.szSensorNameUser);
								
								var sensor = new MsmSensor();
								sensor.label = new MsmSensorLabel(SensorElement.szSensorNameOrig, SensorElement.szSensorNameUser);
								sensor.id = SensorElement.dwSensorID;
								sensor.instance = SensorElement.dwSensorInst;
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

								debugSensorReadings(ReadingElement);
								var reading = new MsmSensorReading((MsmSensorType)ReadingElement.tReading);
								reading.label = new MsmSensorLabel(ReadingElement.szLabelOrig, ReadingElement.szLabelUser);
								reading.sensor_index = ReadingElement.dwSensorIndex;
								reading.id = ReadingElement.dwReadingID;
								reading.unit = ReadingElement.szUnit;
								reading.value = ReadingElement.Value;
								reading.min = ReadingElement.ValueMin;
								reading.avg = ReadingElement.ValueAvg;
								response.readings.Add(reading);
								
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
		
		void debugSensorElements(_HWiNFO_SENSOR_ELEMENT SensorElement) {
			log.Debug("@SensorElement.dwSensorID#" + SensorElement.dwSensorID);
			log.Debug("@SensorElement.dwSensorInst#" + SensorElement.dwSensorInst);
			log.Debug("@SensorElement.szSensorNameOrig#" + SensorElement.szSensorNameOrig);
			log.Debug("@SensorElement.szSensorNameUser#" + SensorElement.szSensorNameUser);
		}
		
		void debugSensorReadings(_HWiNFO_READING_ELEMENT ReadingElement) {
			log.Debug("SENSOR_READING_TYPE@ReadingElement.tReading#" + ReadingElement.tReading);
			log.Debug("UInt32@ReadingElement.dwSensorIndex#" + ReadingElement.dwSensorIndex);
			log.Debug("Unit32@ReadingElement.dwReadingID#" + ReadingElement.dwReadingID);
			log.Debug("string@ReadingElement.szLabelOrig#" + ReadingElement.szLabelOrig);
			log.Debug("string@ReadingElement.szLabelUser#" + ReadingElement.szLabelUser);
			log.Debug("string@ReadingElement.szUnit#" + ReadingElement.szUnit);
			log.Debug("double@ReadingElement.Value#" + ReadingElement.Value);
			log.Debug("double@ReadingElement.ValueMin#" + ReadingElement.ValueMin);
			log.Debug("double@ReadingElement.ValueMax#" + ReadingElement.ValueMax);
			log.Debug("double@ReadingElement.ValueAvg#" + ReadingElement.ValueAvg);
		}
		
	}
	
}