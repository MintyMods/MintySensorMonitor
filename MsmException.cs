using System;

namespace mintymods {
	
	public class MsmException : Exception {
		
		static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		public MsmExceptionHint hint = new MsmExceptionHint();
		public Exception exception;
		public string message;

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