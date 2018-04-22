using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour {
    public GameObject deathOverlay;
    private bool playerAlive = true;
    private int deathTimer = 100;
	// Use this for initialization
	void Start () {
        deathOverlay.active = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (deathTimer > 0)
        {
            deathTimer--;
        } else
        {
            playerAlive = false;
        }

		if (!playerAlive)
        {
            deathOverlay.active = true;
            GameData.retireCurrentPlayer();
        }
	}
}
