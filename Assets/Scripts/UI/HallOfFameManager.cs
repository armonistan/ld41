using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallOfFameManager : MonoBehaviour {

    public PlayerCard playerCardPrefab;
    public RectTransform contentArea;

	// Use this for initialization
	void Start () {
        for(int i=0; i<5; i++)
        {
            addPlayerCard(PlayerGenerator.generate(0), i);
        }
	}

    private void addPlayerCard(PlayerStats stats, int listOrder)
    {
        PlayerCard card = Instantiate(playerCardPrefab, contentArea.transform);
        RectTransform cardTransform = card.GetComponentInChildren<RectTransform>();
        //cardTransform.offsetMin = new Vector2(0, (listOrder * -800));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
