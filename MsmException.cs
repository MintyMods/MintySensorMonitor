using System;
using System.Runtime.Serialization;

namespace mintymods {
	
	public class MsmException : Exception, ISerializable {
		
		public string message;
		public MsmExceptionHint hint = new MsmExceptionHint();
		public Exception exception;
		
		public MsmException(string message) : base(message) {
			this.message = message;
		}

		public MsmException(string message, Exception e) : base(message, e)	{
			this.message = e.Message;
			this.exception = e;
			this.hint.message = e.Message;
		}

	}
	
}