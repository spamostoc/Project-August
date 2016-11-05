using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class mainMenuScript : MonoBehaviour {

    public void loadScene(int index)
    {
        Debug.Log( "ptest number is :" + pManager.pDataManager.getTestNumber());
        SceneManager.LoadScene(index);
    }

    public void loadScene(string levelName)
    {
        Debug.Log("ptest number is :" + pManager.pDataManager.getTestNumber());
        SceneManager.LoadScene(levelName);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
