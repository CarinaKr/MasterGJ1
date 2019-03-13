using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadManager : MonoBehaviour {

    private GameManager gameManager;
    public SnakeMovement snakeMovement;
    public BombManager bombManager;


    private void Start()
    {
        gameManager = GameManager.self;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Bomb" || collision.tag == "Body")
        {
            gameManager.GameOver();
            if(collision.tag=="Bomb")
            {
                collision.gameObject.SetActive(false);
            }
        }
        else if(collision.tag=="Note")
        {
            Note collectedNote = collision.GetComponent<Note>();
            if(collectedNote.colorNum==(int)gameManager.order[0])
            {
                collectedNote.gameObject.SetActive(false);
                gameManager.AddNoteToSound(collectedNote);
                snakeMovement.AddBody(collectedNote.colorNum);
                bombManager.AddBombs();
            }
            else
            {
                Debug.Log("wrong note!!"); 
            }
            
        }
        else if (collision.transform.tag == "Wall")
        {
            gameManager.GameOver();
        }
    }
}
