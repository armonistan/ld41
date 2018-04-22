using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour {
    public Text PlayerNameLabel;
    public Text ReversePlayerNameLabel;
    public Text PlayerNumberLabel;
    public Slider BulkSlider;
    public Slider SpeedSlider;
    public Slider StyleSlider;
    public Text PriceLabel;
    public Text AbilityNameLabel;
    public Text AbilityDescriptionLabel;
    public Text PlayerHeightLabel;
    public Text PlayerWeightLabel;
    public Text MilkLabel;

    public bool selected = false;
    private bool selectUpdated = false;

    public void renderCard(PlayerStats player) {
        PlayerNameLabel.text = player.getPlayerName();
        ReversePlayerNameLabel.text = player.getPlayerName();
        PlayerNumberLabel.text = player.getPlayerNumber();
        BulkSlider.value = player.getBulk();
        SpeedSlider.value = player.getSpeed();
        StyleSlider.value = player.getStyle();
        PriceLabel.text = player.getPrice().ToString("C0");
        AbilityNameLabel.text = player.getAbilityName();
        AbilityDescriptionLabel.text = player.getAbilityDescription();
        PlayerHeightLabel.text = player.getHeight();
        PlayerWeightLabel.text = player.getWeight();
        MilkLabel.text = player.getMilk();
    }

    public void setSelected(bool selected)
    {
        this.selected = selected;
        this.selectUpdated = false;
    }

	// Use this for initialization
	void Start () {
		// nothing to put here, just cross ur fingers that nobody fucks with the prefab
        // I mean if you really wanted you could do a best effort lookup of the above game objects
        // but who has time to write that kind of code? not me.
	}

    private void Update()
    {
        //doing this check to save on a bunch of getcomponents calls
        if(!this.selectUpdated)
        {
            this.selectUpdated = true;
            Component[] shadows = this.GetComponentsInChildren<Shadow>();
            foreach(Shadow shadow in shadows)
            {
                shadow.enabled = this.selected;
            }
        }
    }
}
