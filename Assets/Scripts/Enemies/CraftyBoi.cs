using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftyBoi : Enemy {
    public Vector2 Goal;
    public float MoveSpeed = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(Goal.x, Goal.y, 0), MoveSpeed * Time.deltaTime);
	}
}
