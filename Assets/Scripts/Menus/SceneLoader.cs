using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public static SceneLoader sceneLoader;

    /// scene indices:
    /// 0   Main Menu
    /// 1   Meta Map
    /// 2   NavMenu
    /// 3   Hanger
    /// 4   example scene 4
    /// 5   Forge
    
        
    // Use this for initialization
    void Awake()
    {
        if (sceneLoader == null)
        {
            DontDestroyOnLoad(gameObject);
            sceneLoader = this;
        }
        else if (sceneLoader != this)
        {
            Destroy(gameObject);
        }
    }

    public void loadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void loadScene(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
