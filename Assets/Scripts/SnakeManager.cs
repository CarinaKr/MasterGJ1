using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour {

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.self;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Bomb" || collision.tag == "Body")
        {
            gameManager.GameOver();
        }
        else if(collision.tag=="Note")
        {
            Note collectedNote = collision.GetComponent<Note>();
            if(collectedNote.colorNum==(int)gameManager.order[0])
            {
                collectedNote.gameObject.SetActive(false);
                gameManager.AddNoteToSound(collectedNote);
            }
            else
            {
                Debug.Log("wrong note!!"); 
            }
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag=="Wall")
        {
            gameManager.GameOver();
        }
    }
}
