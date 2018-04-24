using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameData : MonoBehaviour {
    
    private static ArrayList hallOfFame = new ArrayList();
    private static PlayerStats currentPlayer = null;
    private static int money = 0;

    private static bool hasLoaded = false;

    public static bool getHasLoaded()
    {
        return hasLoaded;
    }

    public static void setMoney(int m)
    {
        money = m;
    }

    public static int getMoney()
    {
        return money;
    }

    public static void setCurrentPlayer(PlayerStats stats)
    {
        currentPlayer = stats;
    }

    public static PlayerStats getCurrentPlayer()
    {
        return currentPlayer != null ? currentPlayer : new PlayerStats("Test", "123", 5, 5, 5, 0, PlayerAbilities.Abilities.Unit, "4.20", "5");
    }

    public static void retireCurrentPlayer()
    {
        if (currentPlayer != null)
        {
            hallOfFame.Add(currentPlayer);
            if (currentPlayer.getAbility() == PlayerAbilities.Abilities.GoldenBoy)
            {
                money += currentPlayer.getCareerValue();
            }
            money += currentPlayer.getCareerValue();
            currentPlayer = null;
        }
    }

    public static ArrayList getHallOfFame()
    {
        return hallOfFame;
    }

	public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerStats.dat");

        SaveData data = new SaveData();
        data.currentPlayer = currentPlayer;
        data.hallOfFame = hallOfFame;
        data.money = money;

        bf.Serialize(file, data);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerStats.dat"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/playerStats.dat", FileMode.Open);

            BinaryFormatter bf = new BinaryFormatter();
            SaveData data = (SaveData) bf.Deserialize(file);
            file.Close();

            currentPlayer = data.currentPlayer;
            hallOfFame = data.hallOfFame;
            money = data.money;
            hasLoaded = true;
        }
    }
}
[Serializable]
class SaveData
{
    public PlayerStats currentPlayer;
    public ArrayList hallOfFame;
    public int money;
}
