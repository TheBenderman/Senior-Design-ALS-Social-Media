// Copyright Samuel Halliday 2012
package com.github.fommil.emokit.jpa;

import com.github.fommil.jpa.CrudDao;

import javax.persistence.EntityManagerFactory;
import java.util.UUID;

/**
 * @author Sam Halliday
 */
public class EmotivSessionCrud extends CrudDao<UUID, EmotivSession> {

    public EmotivSessionCrud(EntityManagerFactory emf) {
        super(EmotivSession.class, emf);
    }

}
