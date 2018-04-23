using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallOfFameManager : MonoBehaviour {

    public PlayerCard playerCardPrefab;
    public RectTransform contentArea;

	// Use this for initialization
	void Start () {
        ArrayList hall = GameData.getHallOfFame();
        for(int i=hall.Count; i > 0; i--)
        {
            addPlayerCard((PlayerStats)hall[i-1]);
        }
	}

    private void addPlayerCard(PlayerStats stats)
    {
        PlayerCard card = Instantiate(playerCardPrefab, contentArea.transform);
        card.renderCard(stats);
    }
}
