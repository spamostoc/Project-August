using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NavController : MonoBehaviour {

    public Text clock;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        clock.text = pManager.pDataManager.simTime.ToString();
	}

    public void loadScene(string levelName)
    {
        SceneLoader.sceneLoader.loadScene(levelName);
    }

    public void advanceTime(float timeDelta)
    {
        pManager.pDataManager.onTick(timeDelta);
    }
}
