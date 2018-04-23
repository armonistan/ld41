using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour {
    public GameObject deathOverlay;
    public GameObject ControlOverlay;
    public PlayerControl GamePlayer;
    public Slider bulkSlider;
    public Slider styleSlider;
    private PlayerStats currentPlayer;

    private bool paused = false;
    //pause
    public KeyCode Pause = KeyCode.Q;

    // Use this for initialization
    void Start () {
        currentPlayer = GameData.getCurrentPlayer();
        deathOverlay.SetActive(false);

        bulkSlider.maxValue = 7;
        styleSlider.maxValue = 7;

        bulkSlider.value = currentPlayer.getBulk();
        styleSlider.value = currentPlayer.getStyle();

        GamePlayer.InitializeStats();
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

        if (!paused && Input.GetKeyDown(Pause))
        {
            ControlOverlay.SetActive(true);
            paused = true;
            Time.timeScale = 0;

            return;
        }
        else if (paused && Input.GetKeyDown(Pause))
        {
            ControlOverlay.SetActive(false);
            paused = false;
            Time.timeScale = 1;

            return;
        }
    }
}
