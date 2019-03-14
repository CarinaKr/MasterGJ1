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
    public Sprite snakeHeadGameOver;
    public float timePenaltyWrongNote;
    public AudioSource[] beats;
    public AudioSource countdown;

    public bool[,] fieldBlocked { get; private set; }
    public ArrayList order { get; private set; }
    public bool gameOver { private set; get; }
    public float time { get; set; }
    public bool gameStarted { get; set; }

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
            for(int j=0;j<4;j++)    //bottom
            {
                fieldBlocked[i, j] = true;
            }

            fieldBlocked[i, height - 1] = true; //top

            if(i==0 || i==width-1)
            {
                for (int k = 0; k < height; k++)    //left and right
                {
                    fieldBlocked[i, k] = true;
                }
            }
        }
        SetRandomOrder();
        highscoreText.text = GameLoop.self.highScore.ToString("F0");
        
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
        do
        {   posX = Random.Range(0, width);
            posY = Random.Range(0, height);
        } while (fieldBlocked[posX, posY] || ContainsSnake(posX,posY));

        Vector2 randomPosition = new Vector2(0.75f+(posX*1.5f), 0.75f+(posY*1.5f));
        return randomPosition;
    }

    public void AddNoteToSound(Note collectedNote)
    {
        FreeField(collectedNote.transform.position);
        beats[noteCount - order.Count].volume=1;
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
        if (snakeMovement.head.position.x > x * 1.5 && snakeMovement.head.position.x < (x + 1) * 1.5 && snakeMovement.head.position.y > y * 1.5 && snakeMovement.head.position.y < (y + 1) * 1.5)
        {
            blocked = true;
            return blocked;
        }

        foreach (Transform body in snakeMovement.bodies)
        {
            if(body.position.x>x*1.5 && body.position.x<(x+1)*1.5 && body.position.y>y*1.5 && body.position.y<(y+1)*1.5)
            {
                blocked = true;
            }
        }
        
        return blocked;
    }

    public void BlockField(Vector2 position)
    {
        int x = Mathf.FloorToInt(position.x / 1.5f);
        int y = Mathf.FloorToInt(position.y / 1.5f);

        fieldBlocked[x,y] = true;
    }
    public void FreeField(Vector2 position)
    {
        int x = Mathf.FloorToInt(position.x / 1.5f);
        int y = Mathf.FloorToInt(position.y / 1.5f);

        fieldBlocked[x, y] = false;
    }

    public void GameOver(GameOverCause gameOverCause, bool displayScreen)
    {
        gameOver = true;
        snakeMovement.head.GetComponent<SpriteRenderer>().sprite = snakeHeadGameOver;
        GameLoop.self.gameState = GameLoop.GameState.GAMEOVER;
        foreach (AudioSource audio in beats)
            audio.volume = 0;
        if(displayScreen)
            gameOverScreens[(int)gameOverCause].SetActive(true);
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
        countdownText.gameObject.SetActive(false);
        gameStarted = true;
    }
}
