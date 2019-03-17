using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour {

    public GameObject bombPrefab;
    public List<KeyCode> saveWord;
    public float bombPenalty;

    private GameManager gameManager;
    private bool gotSaved;
    private int saveCounter;

	// Use this for initialization
	void Start () {
        gameManager = GameManager.self;
        InstantiateBombs();
	}
	
	private void InstantiateBombs()
    {
        GameObject newBomb;
        for(int i=0;i<gameManager.startBombCount;i++)
        {
            newBomb=Instantiate(bombPrefab, gameManager.GetRandomFreePosition(), Quaternion.identity, transform);
            gameManager.BlockField(newBomb.transform.position);
        }
    }

    public void AddBombs()
    {
        GameObject newBomb;
        for (int i=0;i<gameManager.growthBombCount;i++)
        {
            newBomb=Instantiate(bombPrefab, gameManager.GetRandomFreePosition(), Quaternion.identity, transform);
            gameManager.BlockField(newBomb.transform.position);
        }
    }

    public IEnumerator Explode(GameObject bomb)
    {
        bomb.GetComponent<Animator>().SetTrigger("explode");
        bomb.GetComponent<AudioSource>().Play();
        gameManager.GameOver(GameManager.GameOverCause.BOMB,false);
        gotSaved = false;
        saveCounter = 0;
        StartCoroutine("WaitForSave");
        yield return new WaitForSeconds(1.25f);
        StopCoroutine("WaitForSave");
        bomb.SetActive(false);
        if (!gotSaved)
        {
            gameManager.gameOverScreens[(int)GameManager.GameOverCause.BOMB].SetActive(true);
            GameLoop.self.gameState = GameLoop.GameState.GAMEOVER;
            gameManager.countdown.clip = gameManager.looseSound;
            gameManager.countdown.loop = true;
            gameManager.countdown.Play();
        }
        else
        {
            gameManager.RevertGameOver();
            gameManager.time += bombPenalty;
        }
    }

    public IEnumerator WaitForSave()
    {
        while (saveCounter<saveWord.Count)
        {
            if (Input.GetKeyDown(saveWord[saveCounter]))
            {
                Debug.Log(saveWord[saveCounter]);
                saveCounter++;
                if (saveCounter == saveWord.Count - 1)
                {
                    gotSaved = true;
                    saveCounter = 0;
                }
            }
            yield return null;
        }
    }
}
