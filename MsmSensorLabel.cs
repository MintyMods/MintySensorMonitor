using System;

namespace mintymods {

	public class MsmSensorLabel {
		
		public string value;
		public string desc;

		public MsmSensorLabel(string value, string desc) {
			this.value = value;
			this.desc = desc;
		}
		
		public void setValue(string value) {
			this.value = value;
		}

		public void setDescription(string desc) {
			this.desc = desc;
		}

		public string getSensorLabel() {
			return value + "#" + desc;
		}

		public string getValue() {
			return value;
		}

		public string getDescription() {
			return desc;
		}

		
	}
}
