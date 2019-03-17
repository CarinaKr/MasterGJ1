using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoop : MonoBehaviour {

    public enum GameState
    {
        MENU,
        GAME,
        SAVE,
        GAMEOVER
    }

    public static GameLoop self;
    public GameState startGameState;
    public GameState gameState { get; set; }
    public float highScore { get; set; }


    private void Awake()
    {
        if(self==null)
        {
            self = this;
        }

        if(self!=this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        gameState = startGameState;
        highScore = Mathf.Infinity;
    }

    // Update is called once per frame
    void Update () {
		if(gameState==GameState.MENU)
        {
            if(Input.anyKeyDown)
            {
                SceneManager.LoadScene(1);
            }
        }
        else if(gameState==GameState.GAMEOVER)
        {
            if(Input.anyKeyDown)
            {
                SceneManager.LoadScene(0);
            }
        }
	}

    private void OnLevelWasLoaded(int level)
    {
        if(level==0)
        {
            gameState = GameState.MENU;
        }
        else if (level == 1)
        {
            gameState = GameState.GAME;
        }
    }
}
