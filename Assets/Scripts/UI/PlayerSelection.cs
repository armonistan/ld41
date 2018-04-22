using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelection : MonoBehaviour {

    public PlayerCard[] playerCards;
    public Scrollbar panelScroll;
    private ArrayList generatedPlayers;

	// Use this for initialization
	void Start () {
        generatedPlayers = new ArrayList();
        for (int i = 0; i < playerCards.Length; i++)
        {
            PlayerStats gen = PlayerGenerator.generate();
            generatedPlayers.Add(gen);
            playerCards[i].renderCard(gen);
        }
	}

    public void CardSelected(int selection)
    {
        // set the green highlight only on the selected card
        for (int i = 0; i < playerCards.Length; i++)
        {
            if(selection == i)
            {
                playerCards[i].setSelected(true);
            } else
            {
                playerCards[i].setSelected(false);
            }
        }

        // snap the scrollbar to put the card in view
        if(selection == 0)
        {
            panelScroll.value = 1;
        } else if (selection == 1)
        {
            panelScroll.value = .5f;
        } else
        {
            panelScroll.value = 0;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
