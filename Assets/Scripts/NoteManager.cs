using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour {

    
    public GameObject notePrefab;
    public Color[] colors;

    

    private GameManager gameManager;

	// Use this for initialization
	void Start () {
        gameManager = GameManager.self;
        InstantiateNotes();
	}
	
	

    private void InstantiateNotes()
    {
        GameObject nextNote;
        Note note;

        for (int i = 0; i < gameManager.noteCount; i++)
        {
            nextNote = Instantiate(notePrefab, gameManager.GetRandomFreePosition(),Quaternion.identity,transform);
            note = nextNote.GetComponent<Note>();
            note.spriteRenderer.color = colors[(int)gameManager.order[i]];
            note.colorNum = (int)gameManager.order[i];
            gameManager.BlockNote(note.transform.position);
        }
            
    }

    
}
