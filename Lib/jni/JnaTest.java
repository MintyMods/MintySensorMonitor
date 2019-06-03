import com.sun.jna.Library;
import com.sun.jna.Native;

public class JnaTest {

	public interface MintySenorMonitorInterface extends Library {
		MintySenorMonitorInterface INSTANCE = Native.loadLibrary("MintySensorMonitor",
				MintySenorMonitorInterface.class);

		public String getJson(String request);
	}

	public static void main(String[] args) {
		MintySenorMonitorInterface jnaLib = MintySenorMonitorInterface.INSTANCE;
		String json = jnaLib.getJson("Hello World");
		System.out.println(json);
	}
}
