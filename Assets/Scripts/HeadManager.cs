using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadManager : MonoBehaviour {

    private GameManager gameManager;
    public SnakeMovement snakeMovement;
    public BombManager bombManager;
    public AudioClip[] collectSounds;
    public AudioClip biteSelf,hitWall;

    private AudioSource audioSource;


    private void Start()
    {
        gameManager = GameManager.self;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Bomb")
        {
            bombManager.StartCoroutine("Explode",collision.gameObject);
        }
        else if(collision.tag == "Body")
        {
            StartCoroutine("EatSelf");
        }
        else if(collision.tag=="Note")
        {
            Note collectedNote = collision.GetComponent<Note>();
            if(collectedNote.colorNum==(int)gameManager.order[0])
            {
                audioSource.clip = collectSounds[Random.Range(0, collectSounds.Length)];
                audioSource.Play();
                collectedNote.gameObject.SetActive(false);
                gameManager.AddNoteToSound(collectedNote);
                snakeMovement.AddBody(collectedNote.colorNum);
                bombManager.AddBombs();
            }
            else
            {
                collision.GetComponent<AudioSource>().Play();
                gameManager.time += gameManager.timePenaltyWrongNote;
            }
            
        }
        else if (collision.transform.tag == "Wall")
        {
            audioSource.clip = hitWall;
            audioSource.Play();
            gameManager.GameOver(GameManager.GameOverCause.WALL,true);
        }
    }

    private IEnumerator EatSelf()
    {
        audioSource.clip = biteSelf;
        audioSource.Play();
        gameManager.GameOver(GameManager.GameOverCause.SNAKE, false);
        yield return new WaitForSeconds(1f);
        gameManager.gameOverScreens[(int)GameManager.GameOverCause.SNAKE].SetActive(true);
    }
}
