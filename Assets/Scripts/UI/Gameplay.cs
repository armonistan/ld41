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
        deathOverlay.SetActive(false);

        bulkSlider.maxValue = currentPlayer.getBulk();
        styleSlider.maxValue = currentPlayer.getStyle();
        GamePlayer.InitializeStats(currentPlayer);
	}
	
	// Update is called once per frame
	void Update () {
		if (GamePlayer.BULK <= 0)
        {
            deathOverlay.SetActive(true);
            GameData.retireCurrentPlayer();
        } else
        {
            bulkSlider.value = GamePlayer.BULK;
            styleSlider.value = GamePlayer.STYLE;
        }
	}
}
