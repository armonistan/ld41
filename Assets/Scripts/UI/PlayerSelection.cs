using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelection : MonoBehaviour {

    public PlayerCard[] playerCards;
    public Scrollbar panelScroll;
    public int money;
    public Text budgetLabel;
    public Text costLabel;
    private ArrayList generatedPlayers;

	// Use this for initialization
	void Start () {
        generatedPlayers = new ArrayList();
        PlayerStats gen = PlayerGenerator.generate(0);
        generatedPlayers.Add(gen);
        playerCards[0].renderCard(gen);
        for (int i = 1; i < playerCards.Length; i++)
        {
            gen = PlayerGenerator.generate((int)Random.Range(money/2, money));
            generatedPlayers.Add(gen);
            playerCards[i].renderCard(gen);
        }
        budgetLabel.text = money.ToString("C0");
    }

    public void setMoney(int money)
    {
        this.money = money;
    }

    public void CardSelected(int selection)
    {
        // set the green highlight only on the selected card
        for (int i = 0; i < playerCards.Length; i++)
        {
            if(selection == i)
            {
                playerCards[i].setSelected(true);
                int price = ((PlayerStats)generatedPlayers[i]).getPrice();
                costLabel.text = price.ToString("C0") + "\n" + (money-price).ToString("C0");
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
