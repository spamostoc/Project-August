using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class pManager : MonoBehaviour
{

    public static pManager pDataManager;

    public enum unitClass { mech, mechPart };

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

        shoot newShoot = new shoot();

        newShoot.abilitySprite = Resources.Load<Sprite>("BoostAttackIcon") as Sprite;
        newShoot.setRange(2);
        newShoot.damage = 1;
        newShoot.actionPointsCost = 1;

        newMech.abilities.Add(newShoot);

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
        mechdata data = new mechdata();

        data.partsIds = new List<Guid>();
        foreach (mechPart p in playerMechs[0].parts)
        {
            data.partsIds.Add(classDictionary.unitGuids[p.GetType()]);
        }

        data.currentHealth = playerMechs[0].currentAtt.health;
        data.currentMovementPoints = playerMechs[0].currentAtt.movementPoints;
        data.currentActionPoints = playerMechs[0].currentAtt.actionPoints;

        data.movementSpeed = playerMechs[0].MovementSpeed;
        data.playerNumber = playerMechs[0].PlayerNumber;

        data.abilityIds = new List<Guid>();
        foreach (ability a in playerMechs[0].abilities)
        {
            data.abilityIds.Add(classDictionary.abilityGuids[a.GetType()]);
        }

        data.health = playerMechs[0].att.health;
        data.movementPoints = playerMechs[0].att.movementPoints;
        data.actionPoints = playerMechs[0].att.actionPoints;

        bf.Serialize(file, data);
        file.Close();
        Debug.Log(Application.persistentDataPath);
    }

    public void load()
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);


        mechdata data = (mechdata)bf.Deserialize(file);
        file.Close();

        mech newMech = this.transform.gameObject.AddComponent<mech>();
        newMech.Initialize();

        foreach (Guid g in data.partsIds)
        {
           // newMech.parts.Add()
        }

        newMech.currentAtt.health = data.currentHealth;
        newMech.currentAtt.movementPoints = data.currentMovementPoints;
        newMech.currentAtt.actionPoints = data.currentActionPoints;

        newMech.MovementSpeed = data.movementSpeed;
        newMech.PlayerNumber = data.playerNumber;


        foreach (Guid g in data.abilityIds)
        {
           // data.abilityIds.Add(classDictionary.abilityGuids[a.GetType()]);
        }
        newMech.att.health = data.health;
        newMech.att.movementPoints = data.movementPoints;
        newMech.att.actionPoints = data.actionPoints;

        playerMechs.Add(newMech);
    }

}

[Serializable]
class mechdata
{
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