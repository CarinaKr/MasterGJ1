using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour {

    public Transform head;
    public Transform tail;
    public List<Transform> bodies;
    public Color[] colors;
    public float speed,turnspeed;

    private List<Vector3> path;
    private List<Vector3> nextPosition;
    private GameManager gameManager;

	// Use this for initialization
	void Start () {
        gameManager = GameManager.self;
        path = new List<Vector3>();
        nextPosition = new List<Vector3>(bodies.Count);
        for(int i=0;i<bodies.Count;i++)
        {
            nextPosition.Add(bodies[i].position);
        }
	}
	
	// Update is called once per frame
	void Update () {
        path.Add(head.position);    //add current position of head to path
        head.Rotate(Vector3.back, Input.GetAxis("Horizontal") * turnspeed * Time.deltaTime);    //rotate head
        head.Translate(head.right * speed * Time.deltaTime, Space.World);    //move head in look direction

        for (int i = 0; i < bodies.Count; i++)
        {
            if (Vector2.Distance(bodies[i].position, nextPosition[i]) <= 0.1f)
            {
                if (path.Contains(nextPosition[i]))
                {
                    nextPosition[i] = path[path.IndexOf(nextPosition[i]) + 1];
                }
                else
                    nextPosition[i] = path[0];
            }
            

            bodies[i].right = nextPosition[i] - bodies[i].position;
            bodies[i].position = Vector3.MoveTowards(bodies[i].position, nextPosition[i], speed*Time.deltaTime);
        }
        
        
	}
}
