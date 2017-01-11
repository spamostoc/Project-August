using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

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
            startUp();
        }
        else if (pDataManager != this)
        {
            Destroy(gameObject);
        }
    }

    public void startUp()
    {
        //units dictionary
        mech m = this.transform.gameObject.AddComponent<mech>();
        m.Initialize();
        UniTable.unitDictionary.Add(typeof(mech), m);

        //parts dictionary
        mechPart mp = this.transform.gameObject.AddComponent<mechPart>();
        mp.Initialize();
        UniTable.partsDictionary.Add(typeof(mechPart), mp);

        steelCore sc = this.transform.gameObject.AddComponent<steelCore>();
        sc.Initialize();
        UniTable.partsDictionary.Add(typeof(steelCore), sc);

        //ability dictionary
        shoot newShoot = new shoot();

        newShoot.abilitySprite = Resources.Load<Sprite>("BoostAttackIcon") as Sprite;
        newShoot.setRange(2);
        newShoot.damage = 1;
        newShoot.actionPointsCost = 1;
        UniTable.abilityDictionary.Add(typeof(shoot), newShoot);

        //prefabs Table

        UniTable.prefabTable.Add(typeof(mech), Resources.Load<Transform>("Alien4") as Transform);
        Debug.Log(Resources.Load<Transform>("Alien4") as Transform);
    }

}

[Serializable]
class dataLibrary
{
    public List<mechdata> playerMechs;
}

[Serializable]
class mechdata
{
    public Guid mechId;

    public List<Guid> partsIds;
    // current att
    public float currentHealth;
    public int currentMovementPoints;
    public int currentMainActionPoints;
    public float currentShieldPoints;

    public float movementSpeed;
    public int playerNumber;

    public List<Guid> abilityIds;

    //base att
    public float health;
    public int movementPoints;
    public int mainActionPoints;
    public int bonusActionPoints;
    public float armor;
    public float heatReduceRate;
    public float shieldPoints;
    public float shieldRegenRate;
    public float shieldMitigation;
}