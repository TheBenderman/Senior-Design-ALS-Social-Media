import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.PrintStream;
import java.util.Map;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.ReentrantLock;

import javax.swing.Timer;

import com.github.fommil.emokit.Emotiv;
import com.github.fommil.emokit.EmotivListener;
import com.github.fommil.emokit.Packet;
import com.github.fommil.emokit.Packet.Sensor;
import com.github.fommil.emokit.jpa.EmotivDatum;
import com.github.fommil.emokit.jpa.EmotivSession;

public class BasicReadings 
{
	static String build = "Time, F3, FC5, AF3, F7, T7, P7, O1, O2, P8, T8, F8, AF4, FC6, F4\n"; 
	
	public static void main(String[] args)
	{
		try {
				//Change. In seconds 
				final int recordDutarion = 3; 
				final String fileDestenation = "data.csv"; 
			
				//Leave
				Emotiv emotiv = new Emotiv();
				final long initTime = System.currentTimeMillis(); 
				
		        final EmotivSession session = new EmotivSession();
		        final Condition condition = new ReentrantLock().newCondition();
	
		        emotiv.addEmotivListener(new EmotivListener() 
		        {
		        	public void receivePacket(Packet packet) 
		        	{
		                EmotivDatum datum = EmotivDatum.fromPacket(packet);
		                datum.setSession(session);
		                //System.out.println(datum.toString());
		                build += (System.currentTimeMillis() - initTime) +","+ datum.toString() + "\n"; 
		            }

		            public void connectionBroken() 
		            {
		                condition.signal();
		                System.out.println("RIP");
		            }
		        });
		        
	        	Timer timer = new Timer(recordDutarion * 1000, new ActionListener() 
	        	{
        			public void actionPerformed(ActionEvent e) 
        			{
    					File f = new File(fileDestenation);
	                	
	                	try 
	                	{
							PrintStream out = new PrintStream(f);
							out.print(build);
							out.close();
							System.exit(1);
							
						} 
	                	catch (FileNotFoundException e1) 
	                	{
							 
							e1.printStackTrace();
						} 
					}
				}); 
				

		       emotiv.start();
		       timer.start();
		       while(true); 
			
			
		} catch (IOException e) {
			
			e.printStackTrace();
		} 


	}

}
