﻿using System;
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
        return currentPlayer;
    }

    public static void retireCurrentPlayer()
    {
        if (currentPlayer != null)
        {
            hallOfFame.Add(currentPlayer);
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

            currentPlayer = data.currentPlayer;
            hallOfFame = data.hallOfFame;
            money = data.money;

            file.Close();
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
