using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSelection : MonoBehaviour {

    public PlayerCard[] playerCards;
    public Scrollbar panelScroll;
    private int money;
    public Text budgetLabel;
    public Text costLabel;
    private ArrayList generatedPlayers;
    private int selected = -1;

	// Use this for initialization
	void Start () {
        money = GameData.getMoney();
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

    public void CardSelected(int selection)
    {
        selected = selection;
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

    public void StartGame()
    {
        if (selected != -1)
        {
            PlayerStats player = (PlayerStats)generatedPlayers[selected];
            int price = player.getPrice();
            if (price <= money)
            {
                GameData.setCurrentPlayer(player);
                GameData.setMoney(money - price);
                money -= price;
                SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
            }
        }
    }
}
