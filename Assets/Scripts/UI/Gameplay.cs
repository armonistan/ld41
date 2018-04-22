using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour {
    public GameObject deathOverlay;
    public PlayerControl GamePlayer;
    public Slider bulkSlider;
    public Slider styleSlider;
    private PlayerStats currentPlayer;

	// Use this for initialization
	void Start () {
        currentPlayer = GameData.getCurrentPlayer();
        deathOverlay.active = false;
        bulkSlider.maxValue = currentPlayer.getBulk();
        styleSlider.maxValue = currentPlayer.getStyle();
	}
	
	// Update is called once per frame
	void Update () {
		if (GamePlayer.BULK <= 0)
        {
            deathOverlay.active = true;
            GameData.retireCurrentPlayer();
        } else
        {
            bulkSlider.value = GamePlayer.BULK;
            styleSlider.value = GamePlayer.STYLE;
        }
	}
}
