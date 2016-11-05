using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement;

public class pManager : MonoBehaviour
{

    public static pManager pDataManager;

    private int testNumber;

    // Use this for initialization
    void Awake()
    {
        if (pDataManager == null)
        {
            DontDestroyOnLoad(gameObject);
            pDataManager = this;
        }
        else if (pDataManager != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        this.testNumber = 1;
        //SceneManager.LoadScene("Main Menu");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setTestNumber(int num)
    {
        this.testNumber = num;
    }

    public int getTestNumber()
    {
        return this.testNumber;
    }
}
