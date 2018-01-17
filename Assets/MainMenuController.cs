using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void loadScene(string levelName)
    {
        SceneLoader.sceneLoader.loadScene(levelName);
    }

    public void quitGame()
    {
        SceneLoader.sceneLoader.quitGame();
    }
}
