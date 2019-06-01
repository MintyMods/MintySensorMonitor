using System;
using System.Runtime.Serialization;

namespace com.mintymods.msm {
	
	public class MsmException : Exception, ISerializable {
		
		public Exception exception;
		public string message;
		public string hint;
		public DateTime date;
		
		public MsmException() {
	
		}

		public MsmException(string message) : base(message) {
			this.message = message;
			this.message = message;
		}

		public MsmException(string hint, Exception e) : base(hint, e) {
			this.message = e.Message;
			this.hint = hint; 
			this.exception = e;
		}

		// This constructor is needed for serialization.
		protected MsmException(SerializationInfo info, StreamingContext context) : base(info, context) {
			
		}
	}
}