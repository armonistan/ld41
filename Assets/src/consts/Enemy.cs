using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum EnemyType
{
    CraftyBoi,
    MenacingBoi
}

[Serializable]
public class EnemyMap
{
    public EnemyType EnemyType;
    public Enemy EnemyPrefab;
}

public class Enemy : MonoBehaviour {
    public EnemyType EnemyType;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
