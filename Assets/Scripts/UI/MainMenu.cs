using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        if (!GameData.getHasLoaded())
        {
            GameData.Load();
        }
    }

    public void Quit()
    {
        GameData.Save();
        Application.Quit();
    }
}
