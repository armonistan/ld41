using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAbilities;

public class PlayerStats {
    private string playerName = "Tommy Wiseau";
    private string playerNumber = "69";
    private int bulk = 2;
    private int speed = 2;
    private int style = 2;
    private int price = 0;
    private Abilities ability = Abilities.FreeAgent;
    private string milkPreference = "";
    private string height = "";
    private string weight = "";

    private int enemiesDefeated = 0;
    private float yardsCovered = 0f;
    private int stiffArms = 0;
    private int brokenTackles = 0;
    private int touchdowns = 0;

    private string[] milk = { "2%", "Whole", "Non-Fat", "Skim", "Curdled", "Buttermilk", "1/2 and 1/2", "Swamp", "Vitamin D", "Soy", "Almond", "Almond-Vanilla", "Pasteurized", "Unceasing", "Chocolate", "Extra", "Wet", "Dehydrated" };

    public PlayerStats(string name, string number)
    {
        this.playerName = name;
        this.playerNumber = number;
        this.milkPreference = milk[(int)(Random.value * (milk.Length - 1))];
    }

    public PlayerStats(string name, string number, int speed, int bulk, int style, int price, Abilities ability, string height, string weight)
    {
        this.playerName = name;
        this.playerNumber = number;
        this.speed = speed;
        this.bulk = bulk;
        this.style = style;
        this.price = price;
        this.ability = ability;
        this.milkPreference = milk[(int)(Random.value * (milk.Length - 1))];
        this.height = height;
        this.weight = weight;
    }

    public int getEnemiesDefeated() { return enemiesDefeated; }
    public float getYardsCovered() { return yardsCovered; }
    public int getStiffArms() { return stiffArms; }
    public int getBrokenTackles() { return brokenTackles; }
    public int getTouchdowns() { return touchdowns; }

    public void recordEnemyDefeated() { enemiesDefeated++; }
    public void recordYardsCovered(float additionalYards) { yardsCovered += additionalYards; }
    public void recordStiffArm() { stiffArms++; }
    public void recordBrokenTackle() { brokenTackles++; }
    public void recordTouchdown() { touchdowns++; }

    public string getPlayerName() { return playerName; }
    public string getPlayerNumber() { return playerNumber; }
    public int getSpeed() { return speed; }
    public int getBulk() { return bulk; }
    public int getStyle() { return style; }
    public int getPrice() { return price; }
    public Abilities getAbility() { return ability; }
    public string getAbilityName() { return AbilitiesEnum.GetStringValue(ability); }
    public string getAbilityDescription() { return AbilitiesEnum.GetAttributeDescription(ability); }
    public string getMilk() { return milkPreference; }
    public string getHeight() { return height; }
    public string getWeight() { return weight; }
}
