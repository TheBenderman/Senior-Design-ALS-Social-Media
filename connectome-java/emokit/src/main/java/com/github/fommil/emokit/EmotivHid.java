// Copyright Samuel Halliday 2012
package com.github.fommil.emokit;

import com.codeminders.hidapi.*;
import com.google.common.collect.Lists;
import lombok.Getter;
import lombok.extern.java.Log;

import javax.annotation.concurrent.NotThreadSafe;
import javax.crypto.spec.SecretKeySpec;

import org.hibernate.service.jta.platform.internal.SynchronizationRegistryBasedSynchronizationStrategy;

import java.io.Closeable;
import java.io.IOException;
import java.util.Arrays;
import java.util.List;
import java.util.concurrent.TimeoutException;

import static java.lang.String.format;
import static java.lang.System.currentTimeMillis;

/**
 * Wrapper for the low level HIDAPI to access an Emotiv EEG.
 * <p/>
 * Supported devices are discovered on construction and a
 * poll is provided to obtain raw packets.
 *
 * @author Sam Halliday
 */
@Log
@NotThreadSafe
final class EmotivHid implements Closeable {
    static final int VENDOR_ID = 4660;
    static final int PRODUCT_ID = 60674;
    static final int BUFSIZE = 32; // at 128hz
    static final int TIMEOUT = 1000;

    private static final List<byte[]> supportedResearch = Lists.newArrayList();
    private static final List<byte[]> supportedConsumer = Lists.newArrayList();

    static {
        try {
            ClassPathLibraryLoader.loadNativeHIDLibrary();
            supportedConsumer.add(new byte[]{33, -1, 31, -1, 30, 0, 0, 0});
            supportedConsumer.add(new byte[]{32, -1, 31, -1, 30, 0, 0, 0});
            supportedConsumer.add(new byte[]{-32, -1, 31, -1, 0, 0, 0, 0}); // unconfirmed
        } catch (Exception e) {
            throw new ExceptionInInitializerError(e);
        }
    }

    private volatile boolean research = true;
    private final HIDDevice device;
    @Getter
    private volatile boolean closed;

    public EmotivHid() throws IOException {
        device = findEmotiv();
        device.enableBlocking();
    }

    public void close() throws IOException {
        closed = true;
        device.close();
    }

    @Override
    public void finalize() throws Throwable {
        synchronized (this) {
            close();
            super.finalize();
        }
    }

    /**
     * @param buf use the supplied buffer.
     * @throws java.io.IOException if there was no response from the Emotiv.
     * @throws TimeoutException    which may indicate that the Emotiv is not connected.
     */
    public byte[] poll(byte[] buf) throws TimeoutException, IOException {
        assert buf.length == BUFSIZE;

        int n;
        long startTime = currentTimeMillis();
        while((n = device.readTimeout(buf, 0)) == 0 && currentTimeMillis() - startTime < TIMEOUT) {
          try {
            // limits us to 100Hz samples
            // http://stackoverflow.com/questions/11094857
            Thread.sleep(10);
          } catch (InterruptedException e) {
          }
          Thread.yield();
        }

        if (n != BUFSIZE)
            throw new IOException(format("Bad Packet: (%s) %s", n, Arrays.toString(buf)));
        return buf;
    }

    /**
     * @return the crypto key for this device.
     * @throws IOException
     */
    public SecretKeySpec getKey() throws IOException {
        String serial = getSerial();

        byte[] raw = serial.getBytes();
        assert raw.length == 16;
        byte[] bytes = new byte[16];

        bytes[0] = raw[15];
        bytes[1] = 0;
        bytes[2] = raw[14];
        bytes[3] = research ? (byte)'H' : (byte)'T';
        bytes[4] = research ? raw[15] : raw[13];
        bytes[5] = research ? (byte)0 : 16;
        bytes[6] = research ? raw[14] : raw[12];
        bytes[7] = research ? (byte)'T' : (byte)'B';
        bytes[8] = research ? raw[13] : raw[15];
        bytes[9] = research ? (byte)16 : 0;
        bytes[10] = research ? raw[12] : raw[14];
        bytes[11] = research ? (byte)'B' : (byte)'H';
        bytes[12] = raw[13];
        bytes[13] = 0;
        bytes[14] = raw[12];
        bytes[15] = 'P';

        return new SecretKeySpec(bytes, "AES");
    }

    /**
     * @return
     */
    public String getSerial() throws IOException {
        String serial = device.getSerialNumberString();
        if (serial.length() != 16)
            throw new IOException("Bad serial: " + serial);
        return serial;
    }

    // workaround http://code.google.com/p/javahidapi/issues/detail?id=40
    private List<HIDDeviceInfo> findDevices(int vendor, int product) throws IOException {
        HIDManager manager = HIDManager.getInstance();
        HIDDeviceInfo[] infos = manager.listDevices();
        
        System.out.println(infos.length);
        
        for(HIDDeviceInfo ii : infos)
        {
        	System.out.println(ii.getProduct_string());
        }
        
        List<HIDDeviceInfo> devs = Lists.newArrayList();
        for (HIDDeviceInfo info : infos) {
            if (info.getVendor_id() == vendor && info.getProduct_id() == product)
                devs.add(info);
        }
        return devs;
    }

    private HIDDevice findEmotiv() throws IOException 
    {
    	HIDManager manager = HIDManager.getInstance();
        HIDDeviceInfo[] infos = manager.listDevices();
        
        System.out.println(infos.length);
        
        for(HIDDeviceInfo ii : infos)
        {
        	HIDDevice dev = ii.open(); 
        	
        	if(ii.getProduct_string().equals("EEG Signals"))
			{
        		if(dev == null)
        		{
        			System.out.println("Null");
        		}
        		System.out.println(ii.toString());
        		research = true; 
        		//byte[] report = new byte[9];
                //int size = dev.getFeatureReport(report);
                //byte[] result = Arrays.copyOf(report, size);
        		 System.out.println(format("Found (%s) %s [%s] with report",
                         dev.getManufacturerString(),
                         dev.getProductString(),
                         dev.getSerialNumberString()));
                         //Arrays.toString(result)));
        		return dev; 
			}
        }
        
        /*List<HIDDeviceInfo> infos = findDevices(VENDOR_ID, PRODUCT_ID);
        for (HIDDeviceInfo info : infos) {
            HIDDevice dev = info.open();
            try {
                byte[] report = new byte[9];
                int size = dev.getFeatureReport(report);
                byte[] result = Arrays.copyOf(report, size);
               System.out.println(format("Found (%s) %s [%s] with report: %s",
                        dev.getManufacturerString(),
                        dev.getProductString(),
                        dev.getSerialNumberString(),
                        Arrays.toString(result)));
                for (byte[] check : supportedConsumer) {
                    if (Arrays.equals(check, result)) {
                        return dev;
                    }
                }
                for (byte[] check : supportedResearch) {
                    if (Arrays.equals(check, result)) {
                        research = true;
                        return dev;
                    }
                }
                dev.close();
            } catch (Exception e)
            {
                dev.close();
            }
        }
        */
        throw new HIDDeviceNotFoundException("Send all this information to https://github.com/fommil/emokit-java/issues and let us know if you have the 'research' or 'consumer' product.");
   
    }

	public boolean isClosed() {
		
		return closed;
	}

}
