// Copyright Samuel Halliday 2012
package com.github.fommil.emokit.jpa;

import com.github.fommil.emokit.Packet;
import com.google.common.base.Preconditions;
import lombok.Data;
import com.github.fommil.emokit.Packet.Sensor;

import javax.persistence.*;

import java.text.Format;
import java.util.Date;
import java.util.UUID;

/**
 * A Java Persistence API (JPA) {@link @Entity} that is appropriate
 * for storage in a traditional RDBMS.
 *
 * @author Sam Halliday
 */
@Entity
@Data
public class EmotivDatum {

    /**
     * @param packet
     * @return a datum, which is not yet assigned to a session.
     */
    public static EmotivDatum fromPacket(Packet packet) {
        // Verbosity alert! Gotta love Java...
        EmotivDatum datum = new EmotivDatum();

        datum.timestamp =  packet.getDate();
        datum.battery = (packet.getBatteryLevel());
        datum.gyroX = (packet.getGyroX());
        datum.gyroY = (packet.getGyroY());

        datum.F3 = (packet.getSensor(Sensor.F3));
        datum.F3_QUALITY = (packet.getQuality(Sensor.F3));
        datum.FC5 = (packet.getSensor(Sensor.FC5));
        datum.FC5_QUALITY = (packet.getQuality(Sensor.FC5));
        datum.AF3 = (packet.getSensor(Sensor.AF3));
        datum.AF3_QUALITY = (packet.getQuality(Sensor.AF3));
        datum.F7 = (packet.getSensor(Sensor.F7));
        datum.F7_QUALITY = (packet.getQuality(Sensor.F7));
        datum.T7 = (packet.getSensor(Sensor.T7));
        datum.T7_QUALITY = (packet.getQuality(Sensor.T7));
        datum.P7 = (packet.getSensor(Sensor.P7));
        datum.P7_QUALITY = (packet.getQuality(Sensor.P7));
        datum.O1 = (packet.getSensor(Sensor.O1));
        datum.O1_QUALITY = (packet.getQuality(Sensor.O1));
        datum.O2 = (packet.getSensor(Sensor.O2));
        datum.O2_QUALITY = (packet.getQuality(Sensor.O2));
        datum.P8 = (packet.getSensor(Sensor.P8));
        datum.P8_QUALITY = (packet.getQuality(Sensor.P8));
        datum.T8 = (packet.getSensor(Sensor.T8));
        datum.T8_QUALITY = (packet.getQuality(Sensor.T8));
        datum.F8 = (packet.getSensor(Sensor.F8));
        datum.F8_QUALITY = (packet.getQuality(Sensor.F8));
        datum.AF4 = (packet.getSensor(Sensor.AF4));
        datum.AF4_QUALITY = (packet.getQuality(Sensor.AF4));
        datum.FC6 = (packet.getSensor(Sensor.FC6));
        datum.FC6_QUALITY = (packet.getQuality(Sensor.FC6));
        datum.F4 = (packet.getSensor(Sensor.F4));
        datum.F4_QUALITY = (packet.getQuality(Sensor.F4));
        return datum;
    }


    @Id
    private UUID id = UUID.randomUUID();


    private EmotivSession session;

    private Date timestamp;

    @Column
    private Integer battery;

    @Column
    private Integer gyroX, gyroY;

    @Column
    private Integer F3, FC5, AF3, F7, T7, P7, O1, O2, P8, T8, F8, AF4, FC6, F4;

    @Column
    private Integer F3_QUALITY, FC5_QUALITY, AF3_QUALITY, F7_QUALITY, T7_QUALITY,
            P7_QUALITY, O1_QUALITY, O2_QUALITY, P8_QUALITY, T8_QUALITY, F8_QUALITY, AF4_QUALITY,
            FC6_QUALITY, F4_QUALITY;

    @Override
    public boolean equals(Object obj) {
        if (this == obj) {
            return true;
        }
        if (!(obj instanceof EmotivDatum) || id == null) {
            return false;
        }
        EmotivDatum other = (EmotivDatum) obj;
        return id.equals(other.id);
    }

    @Override
    public int hashCode() {
        Preconditions.checkNotNull(id, "id must be set before @Entity.hashCode can be called");
        return id.hashCode();
    }

	public void setSession(EmotivSession session) {
		this.session = session; 
	}
	
	@Override
	public String toString()
	{
		return String.format("%d,%d,%d,%d,%d,%d,%d,%d,%d,%d,%d,%d,%d,%d", F3, FC5, AF3, F7, T7, P7, O1, O2, P8, T8, F8, AF4, FC6, F4); 
	}
	
	public String getQuality()
	{
		return String.format("%d,%d,%d,%d,%d,%d,%d,%d,%d,%d,%d,%d,%d,%d", F3_QUALITY, FC5_QUALITY, AF3_QUALITY, F7_QUALITY, T7_QUALITY,
	            P7_QUALITY, O1_QUALITY, O2_QUALITY, P8_QUALITY, T8_QUALITY, F8_QUALITY, AF4_QUALITY,
	            FC6_QUALITY, F4_QUALITY); 
	}

}
