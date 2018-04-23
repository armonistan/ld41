using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    private int loadTime = 5;
    private int loadTimer = -1;
    private string scene;

	public void Load(string scene)
    {
        // pauses a second to make sure that any button press sounds have time to start playing
        loadTimer = 0;
        this.scene = scene;
    }

    private void FixedUpdate()
    {
        if (loadTimer > -1) loadTimer++;

        if (loadTimer >= loadTime)
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
}
