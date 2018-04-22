using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	public void Load(SceneAsset scene)
    {
        SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
    }
}
