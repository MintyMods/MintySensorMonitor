using System;

namespace mintymods {

	public class MsmSensorLabel {
		
		string value;
		string description;

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

		
	}
}
