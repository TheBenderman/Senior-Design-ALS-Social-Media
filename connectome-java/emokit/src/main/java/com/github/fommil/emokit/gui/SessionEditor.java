package com.github.fommil.emokit.gui;

import com.github.fommil.emokit.jpa.EmotivJpaController;
import com.github.fommil.emokit.jpa.EmotivSession;
import lombok.Getter;
import lombok.Setter;
import lombok.extern.java.Log;

import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

/**
 * @author Sam Halliday
 */
@Log
public class SessionEditor extends JPanel {

    @Getter
    @Setter
    private EmotivJpaController controller;

    private final JTextField title = new JTextField("");
    private final JTextArea notes = new JTextArea("");
    private final JButton start, stop;

    public SessionEditor() {
        super(new BorderLayout());

        JPanel titleRow = new JPanel(new BorderLayout());
        JLabel titleLabel = new JLabel("Title:");
        titleRow.add(titleLabel, BorderLayout.WEST);
        titleRow.add(title, BorderLayout.CENTER);
        add(titleRow, BorderLayout.NORTH);

        JPanel notesRow = new JPanel(new BorderLayout());
        JLabel notesLabel = new JLabel("Notes:");
        notes.setLineWrap(true);
        notesRow.add(notesLabel, BorderLayout.WEST);
        notesRow.add(notes, BorderLayout.CENTER);
        add(notesRow, BorderLayout.CENTER);

        JPanel buttons = new JPanel(new BorderLayout());
        start = new JButton("Record");
        stop = new JButton("Stop");
        stop.setEnabled(false);
        buttons.add(start, BorderLayout.WEST);
        buttons.add(stop, BorderLayout.EAST);
        add(buttons, BorderLayout.SOUTH);

        System.out.println(notes.getBorder().getClass().toString());

        start.addActionListener(new ActionListener() {
          
            public void actionPerformed(ActionEvent e) {
                start();
            }
        });
        stop.addActionListener(new ActionListener() {
             
            public void actionPerformed(ActionEvent e) {
                stop();
            }
        });
        title.addKeyListener(new KeyAdapter() {
            @Override
            public void keyTyped(KeyEvent e) {
                title.setBorder(BorderFactory.createLineBorder(Color.RED));
            }
        });
        notes.addKeyListener(new KeyAdapter() {
            @Override
            public void keyTyped(KeyEvent e) {
                notes.setBorder(BorderFactory.createLineBorder(Color.RED));
            }
        });
        title.addFocusListener(new FocusAdapter() {
            @Override
            public void focusLost(FocusEvent e) {
                titleChanged();
            }
        });
        notes.addFocusListener(new FocusAdapter() {
            @Override
            public void focusLost(FocusEvent e) {
                notesChanged();
            }
        });

    }

    private void start() {
        if (controller.isRecording())
            return;
        EmotivSession session = new EmotivSession();
        session.setName(title.getText());
        session.setNotes(notes.getText());
        controller.createSession(session);
        controller.setRecording(true);
        start.setEnabled(false);
        stop.setEnabled(true);
    }

    private void stop() {
        controller.setRecording(false);
        title.setText(updateTitle());
        start.setEnabled(true);
        stop.setEnabled(false);
    }

    private String updateTitle() {
        Matcher matcher = Pattern.compile("^(.*)(\\d++)$").matcher(title.getText());
        if (!matcher.find())
            return title.getText() + " 1";
        return matcher.group(1) + (Integer.parseInt(matcher.group(2)) + 1);
    }

    private void titleChanged() {
        if (controller.isRecording()) {
            EmotivSession session = controller.getSession();
            session.setName(title.getText());
            controller.updateSession(session);
            title.setBorder(null);
        }
    }

    private void notesChanged() {
        if (controller.isRecording()) {
            EmotivSession session = controller.getSession();
            session.setNotes(notes.getText());
            controller.updateSession(session);
            notes.setBorder(null);
        }
    }

	public void setController(EmotivJpaController database) {
		controller = database; 
		
	}

}
