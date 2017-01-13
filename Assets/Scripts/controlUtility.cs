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

        //creating new testmech
        attributes newAtt = new attributes();

        newAtt.health = 10;
        newAtt.movementPoints = 3;
        newAtt.mainActionPoints = 1;

        newMech.baseAtt.setTo(newAtt);

        newMech.dynamicAttributes = new attributes(newAtt);

        newMech.MovementSpeed = 5;


        //make test shoot ability
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
        //initialize save data structure
        FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Create);
        dataLibrary dl = new dataLibrary();
        dl.playerMechs = new List<mechdata>();

        //transfer active data to save structure
        foreach (mech pmechj in pManager.pDataManager.playerMechs)
        {
            mechdata mdata = new mechdata();
            mdata.mechId = UniTable.classGuid[pmechj.GetType()];

            //transcribe abilities
            mdata.abilityIds = new List<Guid>();
            foreach (ability a in pmechj.abilities)
            {
                mdata.abilityIds.Add(UniTable.classGuid[a.GetType()]);
            }

            //transcribe parts
            mdata.partsIds = new List<Guid>();
            foreach (mechPart p in pmechj.parts)
            {
                mdata.partsIds.Add(UniTable.classGuid[p.GetType()]);
            }

            //transcribe dynamic attributes
            mdata.dynamicAtt = new attributes(pmechj.dynamicAttributes);

            //transcribe base attributes
            mdata.baseAtt = new attributes(pmechj.dynamicAttributes);

            //transcribe mech class info
            mdata.weaponIds = new List<Guid>();
            foreach (mechWeapon w in pmechj.weapons)
            {
                mdata.weaponIds.Add(UniTable.classGuid[w.GetType()]);
            }

            //transcribe unit class info
            mdata.movementSpeed = pmechj.MovementSpeed;
            mdata.playerNumber = pmechj.PlayerNumber;


            dl.playerMechs.Add(mdata);
        }


        bf.Serialize(file, dl);
        file.Close();
        Debug.Log(Application.persistentDataPath);
    }

    public void load()
    {
        BinaryFormatter bf = new BinaryFormatter();
        //read from file
        FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);
        dataLibrary dl = (dataLibrary)bf.Deserialize(file);
        file.Close();

        //initialize game data structure
        foreach (mechdata mdata in dl.playerMechs)
        {
            Type mechClass = UniTable.getType(mdata.mechId);
            if (mechClass == null)
            {
                Debug.Log("no such class found");
                continue;
            }

            //load mech
            mech newMech = (mech)pManager.pDataManager.transform.gameObject.AddComponent(mechClass);
            newMech.Initialize();

            //load parts
            foreach (Guid g in mdata.partsIds)
            {
                Type mType = UniTable.getType(g);
                mechPart mp = (mechPart)pManager.pDataManager.transform.gameObject.AddComponent(mType);
                mp.Initialize();
                mp.parent = newMech;
                mp.copyFrom(UniTable.partDictionary[mType]);
                newMech.parts.Add(mp);
            }

            //load abilities
            foreach (Guid g in mdata.abilityIds)
            {
                ability a = UniTable.abilityDictionary[UniTable.getType(g)].clone();
                a.parent = newMech;
                newMech.abilities.Add(a);
            }

            //load dynamic attributes
            newMech.dynamicAttributes = new attributes(mdata.dynamicAtt);

            //load base attributes
            newMech.baseAtt.setTo(mdata.baseAtt);

            //load mech class info
            foreach (Guid g in mdata.weaponIds)
            {
                mechWeapon w = UniTable.weapondictionary[UniTable.getType(g)].clone();
                w.parent = newMech;
                newMech.weapons.Add(w);
            }

            //load unit class info
            newMech.MovementSpeed = mdata.movementSpeed;
            newMech.PlayerNumber = mdata.playerNumber;

            Debug.Log(newMech.dynamicAttributes.movementPoints);
            pManager.pDataManager.playerMechs.Add(newMech);
        }
    }
}
