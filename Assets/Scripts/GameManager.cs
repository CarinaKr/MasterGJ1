using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager self;
    public int width, height;
    public int noteCount;
    public int startBombCount, growthBombCount;

    public bool[,] fieldBlocked { get; private set; }
    public ArrayList order { get; private set; }

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


        //TODO block snake

        SetRandomOrder();
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
        {
            posX = Random.Range(0, width);
            posY = Random.Range(0, height);
        } while (fieldBlocked[posX, posY]);

        Vector2 randomPosition = new Vector2(posX, posY);
        return randomPosition;
    }

    public void AddNoteToSound(Note collectedNote)
    {
        FreeNote(collectedNote.transform.position);
        order.RemoveAt(0);
        if (order.Count == 0)
        {
            Win();
        }
    }

    public void BlockNote(Vector2 position)
    {
        fieldBlocked[(int)position.x, (int)position.y] = true;
    }
    public void FreeNote(Vector2 position)
    {
        fieldBlocked[(int)position.x, (int)position.y] = false;
    }

    public void GameOver()
    {
        Debug.Log("game over");
    }
    public void Win()
    {
        Debug.Log("Win");
    }
}
