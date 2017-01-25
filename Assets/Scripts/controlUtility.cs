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

        //transcribe dynamic attributes

        //transcribe base attributes
        newMech.baseAtt.health = 100;
        newMech.baseAtt.maxHealth = 100;
        newMech.baseAtt.movementPoints = 7;
        newMech.baseAtt.maxMovementPoints = 7;
        newMech.baseAtt.mainActionPoints = 1;
        newMech.baseAtt.maxMainActionPoints = 1;
        newMech.baseAtt.bonusActionPoints = 1;
        newMech.baseAtt.maxBonusActionPoints = 1;
        newMech.baseAtt.armor = 0.15f;
        newMech.baseAtt.heatReduceRate = 3;
        newMech.baseAtt.shieldPoints = 100;
        newMech.baseAtt.maxShieldPoints = 100;
        newMech.baseAtt.shieldRegenRate = 10;
        newMech.baseAtt.shieldMitigation = 10;

        //transcribe mech class info

        //transcribe unit class info
        newMech.displayName = "Intercessor";
        newMech.MovementSpeed = 15;
        newMech.PlayerNumber = 0;

        //weapons
        newMech.weapons.Add(UniTable.weapondictionary[UniTable.classGuid[typeof(flakGunWeapon)]].clone());
        foreach (mechWeapon w in newMech.weapons)
        {
            w.parent = newMech;
        }

        //abilities
        newMech.abilities.Add(UniTable.abilityDictionary[UniTable.classGuid[typeof(shoot)]].clone());
        foreach (ability a in newMech.abilities)
        {
            a.parent = newMech;
        }

        //parts
        steelCore part = pManager.pDataManager.transform.gameObject.AddComponent<steelCore>();
        part.copyFrom(UniTable.partDictionary[UniTable.classGuid[typeof(steelCore)]]);
        newMech.parts.Add(part);
        foreach (mechPart mp in newMech.parts) {
            mp.parent = newMech;
        }

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
            mdata.baseAtt = new attributes(pmechj.baseAtt);

            //transcribe mech class info
            mdata.weaponIds = new List<Guid>();
            foreach (mechWeapon w in pmechj.weapons)
            {
                mdata.weaponIds.Add(UniTable.classGuid[w.GetType()]);
            }

            //transcribe unit class info
            mdata.movementSpeed = pmechj.MovementSpeed;
            mdata.playerNumber = pmechj.PlayerNumber;
            mdata.displayName = pmechj.displayName;

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
            Type mechClass = UniTable.GetTypeFromGuid(mdata.mechId);
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
                Type mType = UniTable.GetTypeFromGuid(g);
                mechPart mp = (mechPart)pManager.pDataManager.transform.gameObject.AddComponent(mType);
                mp.Initialize();
                mp.parent = newMech;
                mp.copyFrom(UniTable.partDictionary[UniTable.classGuid[mType]]);
                newMech.parts.Add(mp);
            }

            //load abilities
            foreach (Guid g in mdata.abilityIds)
            {
                ability a = UniTable.abilityDictionary[UniTable.classGuid[UniTable.GetTypeFromGuid(g)]].clone();
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
                mechWeapon w = UniTable.weapondictionary[UniTable.classGuid[UniTable.GetTypeFromGuid(g)]].clone();
                w.parent = newMech;
                newMech.weapons.Add(w);
            }

            //load unit class info
            newMech.MovementSpeed = mdata.movementSpeed;
            newMech.PlayerNumber = mdata.playerNumber;
            newMech.displayName = mdata.displayName;

            pManager.pDataManager.playerMechs.Add(newMech);
        }
    }
}
