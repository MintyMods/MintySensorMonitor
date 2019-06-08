using System;

namespace mintymods {

	public class MsmSensorLabel {
		
		public string value;
		public string description;

		public MsmSensorLabel(string value, string description) {
			this.value = value;
			this.description = description;
		}
		
		public void setValue(string value) {
			this.value = value;
		}

		public void setDescription(string description) {
			this.description = description;
		}

		public string getSensorLabel() {
			return value + "#" + description;
		}

		public string getValue() {
			return value;
		}

		public string getDescription() {
			return description;
		}

		
	}
}
