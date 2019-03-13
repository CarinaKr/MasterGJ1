using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour {

    public GameObject bombPrefab;

    private GameManager gameManager;

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
}
