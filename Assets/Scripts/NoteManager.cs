using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour {

    [Tooltip("order: quarter, eigth, sixteenths, key")]
    public GameObject[] notePrefabs;
    public Color[] colors;
    public Transform orderParent;
    [Tooltip("order: quarter, eigth, sixteenths, key")]
    public GameObject[] orderPrefabs;
    public Transform[] showOrderPositions;
    public List<Note> showOrderNotes { get; private set; }
    public Color greyColor;

    private GameManager gameManager;

	// Use this for initialization
	void Start () {
        gameManager = GameManager.self;
        showOrderNotes = new List<Note>();
        InstantiateNotes();
        gameManager.StartCoroutine("StartGame");
    }

    private void InstantiateNotes()
    {
        GameObject nextNote,nextNoteShowOrder;
        Note note;
        int prefabNumber;

        for (int i = 0; i < gameManager.noteCount; i++)
        {
            prefabNumber = UnityEngine.Random.Range(0, notePrefabs.Length);
            nextNote = Instantiate(notePrefabs[prefabNumber], gameManager.GetRandomFreePosition(),Quaternion.identity,transform);
            note = nextNote.GetComponent<Note>();
            note.spriteRenderer.color = colors[(int)gameManager.order[i]];
            note.colorNum = (int)gameManager.order[i];
            gameManager.BlockField(note.transform.position);

            nextNoteShowOrder = Instantiate(orderPrefabs[prefabNumber], showOrderPositions[i].position, Quaternion.identity, orderParent);
            note = nextNoteShowOrder.GetComponent<Note>();
            note.spriteRenderer.color = colors[(int)gameManager.order[i]];
            showOrderNotes.Add(note);
        }
            
    }
}
