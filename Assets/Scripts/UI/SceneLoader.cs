using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	public void Load(string scene)
    {
        Debug.Log("Loading scene " + scene);
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
