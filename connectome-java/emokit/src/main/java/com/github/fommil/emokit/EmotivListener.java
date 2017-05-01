package com.github.fommil.emokit;

/**
 * Asynchronous listener interface for packets from an Emotiv.
 *
 * @author Sam Halliday
 */
public interface EmotivListener {
    /**
     * @param packet
     */
    void receivePacket(Packet packet);

    /**
     * Called if the underlying connection has become irretrievably broken.
     * <p/>
     * Robust applications may choose to unregister listeners and spawn a
     * new Emotiv connection.
     */
    void connectionBroken();
}
