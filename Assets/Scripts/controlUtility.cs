using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class controlUtility : MonoBehaviour {

    public void makeTestMech()
    {
        mech newMech = pManager.pDataManager.transform.gameObject.AddComponent<mech>();
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

        steelCore part = pManager.pDataManager.transform.gameObject.AddComponent<steelCore>();
        part.Initialize();
        part.parent = newMech;
        newMech.parts.Add(part);

        pManager.pDataManager.playerMechs.Add(newMech);
    }

    public void save()
    {
        BinaryFormatter bf = new BinaryFormatter();

        if (pManager.pDataManager.playerMechs.Count <= 0)
        {
            throw new IndexOutOfRangeException("no player mechs to save");
        }

        FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Create);



        dataLibrary dl = new dataLibrary();
        dl.playerMechs = new List<mechdata>();

        foreach (mech pmechj in pManager.pDataManager.playerMechs)
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

            if (mechClass == null)
            {
                Debug.Log("no such class found");
                continue;
            }


            mech newMech = (mech)pManager.pDataManager.transform.gameObject.AddComponent(mechClass);
            newMech.Initialize();

            foreach (Guid g in mdata.partsIds)
            {
                Type mType = UniTable.getType(g);
                mechPart mp = (mechPart)pManager.pDataManager.transform.gameObject.AddComponent(mType);
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

            pManager.pDataManager.playerMechs.Add(newMech);
        }
    }
}
