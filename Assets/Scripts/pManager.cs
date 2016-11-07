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
            playerMechs = new List<mech>();
        }
        else if (pDataManager != this)
        {
            Destroy(gameObject);
        }
    }

    public void makeTestMech()
    {
        mech newMech = this.transform.gameObject.AddComponent<mech>();

        attributes newAtt = new attributes();

        newAtt.health = 10;
        newAtt.movementPoints = 3;
        newAtt.actionPoints = 1;

        newMech.att.setTo(newAtt);
        newMech.Initialize();

        newMech.MovementSpeed = 5;

        newMech.abilities.Add(new shoot());

        newMech.abilities[0].abilitySprite = Resources.Load<Sprite>("BoostAttackIcon") as Sprite;

        this.playerMechs.Add(newMech);
    }

}
