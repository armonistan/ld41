using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour {
    public GameObject PlayerNameLabel;
    public GameObject ReversePlayerNameLabel;
    public GameObject PlayerNumberLabel;
    public GameObject BulkSlider;
    public GameObject SpeedSlider;
    public GameObject StyleSlider;
    public GameObject PriceLabel;
    public GameObject AbilityNameLabel;
    public GameObject AbilityDescriptionLabel;
    public GameObject PlayerHeightLabel;
    public GameObject PlayerWeightLabel;
    public GameObject MilkLabel;

    public bool selected = false;
    private bool selectUpdated = false;

    public void renderCard(PlayerStats player) {
        PlayerNameLabel.GetComponent<Text>().text = player.getPlayerName();
        ReversePlayerNameLabel.GetComponent<Text>().text = player.getPlayerName();
        PlayerNumberLabel.GetComponent<Text>().text = player.getPlayerNumber();
        BulkSlider.GetComponent<Slider>().value = player.getBulk();
        SpeedSlider.GetComponent<Slider>().value = player.getSpeed();
        StyleSlider.GetComponent<Slider>().value = player.getStyle();
        PriceLabel.GetComponent<Text>().text = player.getPrice().ToString("C");
        AbilityNameLabel.GetComponent<Text>().text = player.getAbilityName();
        AbilityDescriptionLabel.GetComponent<Text>().text = player.getAbilityDescription();
        PlayerHeightLabel.GetComponent<Text>().text = player.getHeight();
        PlayerWeightLabel.GetComponent<Text>().text = player.getWeight();
        MilkLabel.GetComponent<Text>().text = player.getMilk();
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
