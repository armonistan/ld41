using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour {
    public GameObject deathOverlay;
    public PlayerControl GamePlayer;

	// Use this for initialization
	void Start () {
        deathOverlay.active = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (GamePlayer.BULK <= 0)
        {
            deathOverlay.active = true;
            GameData.retireCurrentPlayer();
        }
	}
}
