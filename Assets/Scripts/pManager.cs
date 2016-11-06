using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine.SceneManagement;

public class pManager : MonoBehaviour
{

    public static pManager pDataManager;

    public List<mech> playerMechs;

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

    public void makeTestMech()
    {
        mech newMech = this.transform.gameObject.AddComponent<mech>();
        playerMechs = new List<mech>();

        attributes newAtt = new attributes();

        newAtt.health = 10;
        newAtt.movementPoints = 3;
        newAtt.actionPoints = 1;

        newMech.Initialize();

        newMech.att.setTo(newAtt);

        newMech.MovementSpeed = 5;

        this.playerMechs.Add(newMech);
    }

}
