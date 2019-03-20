using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public enum GameOverCause
    {
        WIN,
        BOMB,
        SNAKE,
        WALL
    }

    public static GameManager self;
    public NoteManager noteManager;
    public SnakeMovement snakeMovement;
    public int width, height;
    public int noteCount;
    public int startBombCount, growthBombCount;
    public Text timeText, highscoreText,countdownText;
    [Tooltip("order: win, bomb, snake")]
    public GameObject[] gameOverScreens;
    public Sprite snakeHeadGameOver,snakeHeadNormal;
    public float timePenaltyWrongNote;
    public AudioSource[] beats;
    public AudioSource countdown;
    public AudioClip lastCountdownBeat;
    public AudioClip looseSound;

    public bool[,] fieldBlocked { get; private set; }
    public ArrayList order { get; private set; }
    public bool gameOver { private set; get; }
    public float time { get; set; }
    public bool gameStarted { get; set; }

    private List<AudioSource> collectedBeats;
    private float cellSize;

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

        fieldBlocked = new bool[width, height];
        order = new ArrayList();

        //block for frame (left, right, top 1 each; bottom 4)
        for (int i=0;i<width;i++)   
        {
            for (int j = 0; j < 4; j++)    //bottom  DEBUG
            {
                fieldBlocked[i, j] = true;
            }

            fieldBlocked[i, height - 1] = true; //top

            if (i == 0 || i == width - 1)
            {
                for (int k = 0; k < height; k++)    //left and right
                {
                    fieldBlocked[i, k] = true;
                }
            }
        }
        SetRandomOrder();
        highscoreText.text = GameLoop.self.highScore.ToString("F0");
        collectedBeats = new List<AudioSource>();
        cellSize = (float)39 / (float)width;
    }

    private void Update()
    {
        if (gameOver || !gameStarted) return;
        time += Time.deltaTime;
        timeText.text = time.ToString("F0");
    }

    private void SetRandomOrder()
    {
        int randNum;
        for (int i = 0; i < noteCount; i++)
        {
            do
            {
                randNum = UnityEngine.Random.Range(0, noteCount);
            } while (order.Contains(randNum));
            order.Add(randNum);
        }
    }

    public Vector2 GetRandomFreePosition()
    {
        int posX, posY;
        int tryCounter = 0;
        do
        {   posX = Random.Range(0, width);
            posY = Random.Range(0, height);
            tryCounter++;
        } while ((fieldBlocked[posX, posY] || ContainsSnake(posX,posY)) && tryCounter<100);
        Debug.Log("try counter: " + tryCounter);
        if(tryCounter>=100)
            return new Vector2(0,0);

        Vector2 randomPosition = new Vector2((cellSize/2)+(posX*cellSize), (cellSize/2)+(posY*cellSize));
        return randomPosition;
    }

    public void AddNoteToSound(Note collectedNote)
    {
        FreeField(collectedNote.transform.position);
        beats[noteCount - order.Count].volume=1;
        collectedBeats.Add(beats[noteCount - order.Count]);
        noteManager.showOrderNotes[noteCount-order.Count].spriteRenderer.color = noteManager.greyColor;
        order.RemoveAt(0);
        if (order.Count == 0)
        {
            Win(GameOverCause.WIN);
        }
    }

    public bool ContainsSnake(int x, int y)
    {
        bool blocked=false;
        if (snakeMovement.head.position.x > x * cellSize && snakeMovement.head.position.x < (x + 1) * cellSize && snakeMovement.head.position.y > y * cellSize && snakeMovement.head.position.y < (y + 1) * cellSize)
        {
            blocked = true;
            return blocked;
        }

        foreach (Transform body in snakeMovement.bodies)
        {
            if(body.position.x>x*cellSize && body.position.x<(x+1)*cellSize && body.position.y>y*cellSize && body.position.y<(y+1)*cellSize)
            {
                blocked = true;
            }
        }
        
        return blocked;
    }

    public void BlockField(Vector2 position)
    {
        int x = Mathf.FloorToInt(position.x / cellSize);
        int y = Mathf.FloorToInt(position.y / cellSize);

        fieldBlocked[x,y] = true;
    }
    public void FreeField(Vector2 position)
    {
        int x = Mathf.FloorToInt(position.x / cellSize);
        int y = Mathf.FloorToInt(position.y / cellSize);

        fieldBlocked[x, y] = false;
    }

    public void GameOver(GameOverCause gameOverCause, bool displayScreen)
    {
        gameOver = true;
        snakeMovement.head.GetComponent<SpriteRenderer>().sprite = snakeHeadGameOver;
        if(gameOverCause!=GameOverCause.BOMB)
            GameLoop.self.gameState = GameLoop.GameState.GAMEOVER;
        foreach (AudioSource audio in beats)
            audio.volume = 0;
        if(displayScreen)
        {
            gameOverScreens[(int)gameOverCause].SetActive(true);
            countdown.clip = looseSound;
            countdown.loop = true;
            countdown.Play();
        }
    }
    public void RevertGameOver()
    {
        gameOver = false;
        snakeMovement.head.GetComponent<SpriteRenderer>().sprite = snakeHeadNormal;
        GameLoop.self.gameState = GameLoop.GameState.GAME;
        foreach (AudioSource audio in collectedBeats)
            audio.volume = 1;
    }
    public void Win(GameOverCause gameOverCause)
    {
        gameOver = true;
        GameLoop.self.gameState = GameLoop.GameState.GAMEOVER;
        gameOverScreens[(int)gameOverCause].SetActive(true);
        if (time < GameLoop.self.highScore)
        {
            GameLoop.self.highScore = time;
            timeText.color = highscoreText.color;
        }
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.5f);
        for(int i=3;i>0;i--)
        {
            countdownText.text = i.ToString();
            countdown.Play();
            yield return new WaitForSeconds(1);
        }
        countdown.clip = lastCountdownBeat;
        countdown.Play();
        countdownText.gameObject.SetActive(false);
        gameStarted = true;
    }
}
