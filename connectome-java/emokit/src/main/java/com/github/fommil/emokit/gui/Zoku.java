// Copyright Samuel Halliday 2012
package com.github.fommil.emokit.gui;

import com.github.fommil.emokit.Emotiv;
import com.github.fommil.emokit.jpa.EmotivJpaController;
import com.github.fommil.jpa.CrudDao;
import com.github.fommil.swing.SwingConvenience;
import lombok.extern.java.Log;

import javax.persistence.EntityManagerFactory;
import javax.swing.*;
import java.awt.*;
import java.io.IOException;
import java.util.logging.Level;

/**
 * Zoku is a Swing GUI application for the acquisition of
 * EEG data from an Emotiv EPOC headset. Although this
 * runs as a standalone application for data acquistition,
 * its primary function is to showcase the various pluggable
 * Swing components in this package.
 *
 * @author Sam Halliday
 * @see <a href="http://en.wikipedia.org/wiki/Glossary_of_terms_in_The_Quantum_Thief#The_zoku">Zoku</a>
 */
@Log
public class Zoku {

    public static void main(String[] args) {
        //EntityManagerFactory emf = CrudDao.createEntityManagerFactory("ZokuPU");
        //EmotivJpaController database = new EmotivJpaController(emf);

        JFrame frame = new JFrame("Zoku");
        SwingConvenience.enableOSXFullscreen(frame);
        frame.setLayout(new BorderLayout());
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setSize(1200, 800);
        JPanel sidebar = new JPanel(new BorderLayout());
        sidebar.setBackground(Color.WHITE);
        frame.add(sidebar, BorderLayout.EAST);

        SensorQualityView quality = new SensorQualityView();
        frame.add(quality, BorderLayout.CENTER);

        BatteryView battery = new BatteryView();
        sidebar.add(battery, BorderLayout.NORTH);

        GyroView gyro = new GyroView();
        sidebar.add(gyro, BorderLayout.SOUTH);

        SessionEditor editor = new SessionEditor();
        //editor.setController(database);
        sidebar.add(editor, BorderLayout.CENTER);

        SensorView sensors = new SensorView();
        frame.add(sensors, BorderLayout.SOUTH);

        frame.setVisible(true);

//        while (true) {
            try {
                Emotiv emotiv = new Emotiv();
                //emotiv.addEmotivListener(database);
                emotiv.addEmotivListener(quality);
                emotiv.addEmotivListener(battery);
                emotiv.addEmotivListener(gyro);
                emotiv.addEmotivListener(sensors);
                emotiv.start();
            } catch (IOException e) {
                //log.log(Level.SEVERE, "", e);
                System.exit(1);
            }
//        }
    }

}
