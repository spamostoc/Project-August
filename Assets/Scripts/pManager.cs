﻿using UnityEngine;
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

    public void makeTestMech()
    {
        mech newMech = this.transform.gameObject.AddComponent<mech>();
        newMech.Initialize();

        attributes newAtt = new attributes();

        newAtt.health = 10;
        newAtt.movementPoints = 3;
        newAtt.actionPoints = 1;

        newMech.att.setTo(newAtt);

        newMech.MovementSpeed = 5;

        shoot newShoot = new shoot();

        newShoot.abilitySprite = Resources.Load<Sprite>("BoostAttackIcon") as Sprite;
        newShoot.setRange(2);
        newShoot.damage = 1;
        newShoot.actionPointsCost = 1;
        newShoot.parent = newMech;

        newMech.abilities.Add(newShoot);

        steelCore part = this.transform.gameObject.AddComponent<steelCore>();
        part.Initialize();
        part.parent = newMech;
        newMech.parts.Add(part);

        this.playerMechs.Add(newMech);
    }

    public void save()
    {
        BinaryFormatter bf = new BinaryFormatter();

        if(playerMechs.Count <= 0 )
        {
            throw new IndexOutOfRangeException("no player mechs to save");
        }

        FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Create);
        


        dataLibrary dl = new dataLibrary();
        dl.playerMechs = new List<mechdata>();

        foreach( mech pmechj in playerMechs )
        {
            mechdata mdata = new mechdata();
            mdata.mechId = UniTable.classGuid[pmechj.GetType()];

            mdata.partsIds = new List<Guid>();
            foreach (mechPart p in pmechj.parts)
            {
                mdata.partsIds.Add(UniTable.classGuid[p.GetType()]);
            }

            mdata.currentHealth = pmechj.currentAtt.health;
            mdata.currentMovementPoints = pmechj.currentAtt.movementPoints;
            mdata.currentActionPoints = pmechj.currentAtt.actionPoints;

            mdata.movementSpeed = pmechj.MovementSpeed;
            mdata.playerNumber = pmechj.PlayerNumber;

            mdata.abilityIds = new List<Guid>();
            foreach (ability a in pmechj.abilities)
            {
                mdata.abilityIds.Add(UniTable.classGuid[a.GetType()]);
            }

            mdata.health = pmechj.att.health;
            mdata.movementPoints = pmechj.att.movementPoints;
            mdata.actionPoints = pmechj.att.actionPoints;
            dl.playerMechs.Add(mdata);
        }

        
        bf.Serialize(file, dl);
        file.Close();
        Debug.Log(Application.persistentDataPath);
    }

    public void load()
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);


        dataLibrary dl = (dataLibrary)bf.Deserialize(file);
        file.Close();

        foreach (mechdata mdata in dl.playerMechs)
        {

            Type mechClass = UniTable.getType(mdata.mechId);

            if(mechClass == null)
            {
                Debug.Log("no such class found");
                continue;
            }

            
            mech newMech = (mech)this.transform.gameObject.AddComponent(mechClass);
            newMech.Initialize();

            foreach (Guid g in mdata.partsIds)
            {
                Type mType = UniTable.getType(g);
                mechPart mp = (mechPart)this.transform.gameObject.AddComponent(mType);
                mp.Initialize();
                mp.parent = newMech;
                mp.copyFrom(UniTable.partsDictionary[mType]);
                newMech.parts.Add(mp);
            }

            newMech.currentAtt.health = mdata.currentHealth;
            newMech.currentAtt.movementPoints = mdata.currentMovementPoints;
            newMech.currentAtt.actionPoints = mdata.currentActionPoints;

            newMech.MovementSpeed = mdata.movementSpeed;
            newMech.PlayerNumber = mdata.playerNumber;


            foreach (Guid g in mdata.abilityIds)
            {
                Debug.Log("making an ability" + g);
                ability a = UniTable.abilityDictionary[UniTable.getType(g)].clone();
                Debug.Log(a);
                a.parent = newMech;
                newMech.abilities.Add(a);
            }
            newMech.att.health = mdata.health;
            newMech.att.movementPoints = mdata.movementPoints;
            newMech.att.actionPoints = mdata.actionPoints;

            playerMechs.Add(newMech);
        }
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
    public int currentActionPoints;

    public float movementSpeed;
    public int playerNumber;

    public List<Guid> abilityIds;

    //base att
    public float health;
    public int movementPoints;
    public int actionPoints;

}