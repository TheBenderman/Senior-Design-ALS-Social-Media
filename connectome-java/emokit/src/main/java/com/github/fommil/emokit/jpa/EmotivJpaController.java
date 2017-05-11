// Copyright Samuel Halliday 2012

package com.github.fommil.emokit.jpa;

import com.github.fommil.emokit.EmotivListener;
import com.github.fommil.emokit.Packet;
import com.google.common.base.Preconditions;
import lombok.Getter;
import lombok.Setter;
import lombok.extern.java.Log;

import javax.persistence.EntityManagerFactory;

/**
 * Abstracts the lower level CRUD operations for recording
 * data one session at a time. Data recording is off by
 * default.
 * <p/>
 * Needs to be told about changes to the session to be able
 * to persist (one of the failings of JPA is that it isn't
 * fully compatible with PropertyChangeListener support).
 * <p/>
 * The session object must be re-obtained from the database
 * layer in order to see all associated data.
 *
 * @author Sam Halliday
 */
@Log
public class EmotivJpaController implements EmotivListener {

    private final EmotivDatumCrud datumCrud;
    private final EmotivSessionCrud sessionCrud;

    @Getter @Setter
    private volatile EmotivSession session;

    @Getter @Setter
    private volatile boolean recording;

    public EmotivJpaController(EntityManagerFactory emf) {
        datumCrud = new EmotivDatumCrud(emf);
        sessionCrud = new EmotivSessionCrud(emf);
    }

    public void createSession(EmotivSession session) {
        this.session = session;
        sessionCrud.create(session);
    }

    public void updateSession(EmotivSession session) {
        Preconditions.checkNotNull(session);
        sessionCrud.update(session);
    }


    public void receivePacket(final Packet packet) {
        if (!recording)
            return;
        EmotivSession session = this.session;
        EmotivDatum datum = EmotivDatum.fromPacket(packet);
        datum.setSession(session);
        datumCrud.create(datum);
    }


    public void connectionBroken() { }

	public boolean isRecording() {
		
		return recording;
	}

	public void setRecording(boolean b) {
		recording = b; 
		
	}

	public EmotivSession getSession() {
		 
		return session;
	}
}
