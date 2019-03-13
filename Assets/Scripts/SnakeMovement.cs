using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour {

    public Transform head;
    public Transform tail;
    public List<Transform> bodies;
    public Color[] colors;
    public float speed,turnspeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        head.Rotate(Vector3.back, Input.GetAxis("Horizontal") * turnspeed * Time.deltaTime);    //rotate head
        head.Translate(head.right*speed*Time.deltaTime,Space.World);    //move head in look direction
        bodies[0].position = Vector3.MoveTowards(bodies[0].position, head.position, speed*Time.deltaTime);
        foreach(Transform body in bodies)   
        {

        }
	}
}
