import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.PrintStream;
import java.util.Map;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.ReentrantLock;

import com.github.fommil.emokit.Emotiv;
import com.github.fommil.emokit.EmotivListener;
import com.github.fommil.emokit.Packet;
import com.github.fommil.emokit.Packet.Sensor;
import com.github.fommil.emokit.jpa.EmotivDatum;
import com.github.fommil.emokit.jpa.EmotivSession;

public class BasicReadings {

	public static void main(String[] args)
	{
		
		try {
			   Emotiv emotiv = new Emotiv();

		        final EmotivSession session = new EmotivSession();
		        //session.setName("My Session");
		        //session.setNotes("My Notes for " + emotiv.getSerial());

		        final Condition condition = new ReentrantLock().newCondition();

		        emotiv.addEmotivListener(new EmotivListener() 
		        {
		        	int counter = 1000; 
		        	String build = "F3, FC5, AF3, F7, T7, P7, O1, O2, P8, T8, F8, AF4, FC6, F4\n"; 
		        	
		           public void receivePacket(Packet packet) 
		           {
		        	   counter--; 
		                EmotivDatum datum = EmotivDatum.fromPacket(packet);
		                datum.setSession(session);
		                //System.out.println(datum.toString());
		                build += datum.toString() + "\n"; 
		                System.out.println(counter); 
		                if(counter < 0 && counter < 1000 )
		                {
		                	counter = 1000; 
		                	File f = new File("data.csv");
		                	
		                	try {
								PrintStream out = new PrintStream(f);
								out.print(build);
								out.close();
								
								System.exit(1);
							} catch (FileNotFoundException e) {
								 
								e.printStackTrace();
							} 
		                	
		                	
		                }
		            }

		            
		            public void connectionBroken() {
		                condition.signal();
		                System.out.println("RIPpp");
		            }
		        });

		       emotiv.start();
		       while(true); 
			
			
		} catch (IOException e) {
			
			e.printStackTrace();
		} 


	}

}
