using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class controlUtility : MonoBehaviour {

    public void makeTestMech()
    {
        Mech newMech = pManager.pDataManager.transform.gameObject.AddComponent<Mech>();
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
        newMech.unitId = Guid.NewGuid();
        newMech.MovementSpeed = 15;
        newMech.PlayerNumber = 0;

        //weapons
        newMech.addPartAs(masterInventory.createPart(typeof(FlakGunWeapon)), Part.slot.weapon1);
        newMech.addPartAs(masterInventory.createPart(typeof(LasGunWeapon)), Part.slot.weapon2);

        //abilities
        newMech.abilities.Add(UniTable.abilityDictionary[UniTable.classGuid[typeof(shoot)]].clone());
        newMech.abilities.Add(UniTable.abilityDictionary[UniTable.classGuid[typeof(switchWeapon)]].clone());
        foreach (ability a in newMech.abilities)
        {
            a.parent = newMech;
        }

        //parts
        newMech.addPartAs(masterInventory.createPart(typeof(SteelCore)), Part.slot.core);

        pManager.pDataManager.playerMechs.Add(newMech.unitId, newMech);
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
        dl.playerMechs = new List<MechData>();

        //transfer active data to save structure
        foreach (Mech pmechj in pManager.pDataManager.playerMechs.Values)
        {
            dl.playerMechs.Add(transcribeMech(pmechj));
        }

        dl.masterInventory = new List<PartData>();

        foreach (Part part in masterInventory.getParts())
        {
            dl.masterInventory.Add(transcribePart(part));
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

        //clear current values
        masterInventory.clearInventory();
        pManager.pDataManager.playerMechs = new Dictionary<Guid, Mech>();

        //initialize game data structure

        foreach (PartData partdata in dl.masterInventory)
        {
            loadPartData(partdata);
        }

        foreach (MechData mdata in dl.playerMechs)
        {
            Mech newMech = loadMechData(mdata);
            pManager.pDataManager.playerMechs.Add(newMech.unitId, newMech);
            Debug.Log("adding new mech guid: " + newMech.unitId);
        }
    }

    private MechData transcribeMech(Mech mech)
    {
        MechData mdata = new MechData();
        mdata.mechTypeId = UniTable.classGuid[mech.GetType()];

        mdata.mechUnitId = mech.unitId;

        //transcribe abilities
        mdata.abilityIds = new List<Guid>();
        foreach (ability a in mech.abilities)
        {
            mdata.abilityIds.Add(UniTable.classGuid[a.GetType()]);
        }

        //transcribe dynamic attributes
        mdata.dynamicAtt = new attributes(mech.dynamicAttributes);

        //transcribe base attributes
        mdata.baseAtt = new attributes(mech.baseAtt);

        //transcribe mech class info
        mdata.parts = new Dictionary<Part.slot, Guid>();
        foreach (KeyValuePair<Part.slot, Part> ownedPart in mech.parts)
        {
            mdata.parts.Add(ownedPart.Key, ownedPart.Value.partId);
        }

        //transcribe unit class info
        mdata.movementSpeed = mech.MovementSpeed;
        mdata.playerNumber = mech.PlayerNumber;
        mdata.displayName = mech.displayName;
        return mdata;
    }

    private Mech loadMechData(MechData mechdata)
    {
        Type mechClass = UniTable.GetTypeFromGuid(mechdata.mechTypeId);
        if (mechClass == null)
        {
            Debug.Log("no such class found");
            return null;
        }

        //load mech
        Mech newMech = (Mech)pManager.pDataManager.transform.gameObject.AddComponent(mechClass);
        newMech.Initialize();

        newMech.unitId = mechdata.mechUnitId;

        //load abilities
        foreach (Guid g in mechdata.abilityIds)
        {
            ability a = UniTable.abilityDictionary[UniTable.classGuid[UniTable.GetTypeFromGuid(g)]].clone();
            a.parent = newMech;
            newMech.abilities.Add(a);
        }
        Debug.Log(newMech.abilities.Count);

        //load dynamic attributes
        newMech.dynamicAttributes.setTo(mechdata.dynamicAtt);

        //load base attributes
        newMech.baseAtt.setTo(mechdata.baseAtt);

        //load mech class info
        foreach (KeyValuePair<Part.slot, Guid> ownedPart in mechdata.parts)
        {
            Debug.Log("slot: " + ownedPart.Key + " has item: " + ownedPart.Value);
            newMech.addPartAs(masterInventory.getPart(ownedPart.Value, ownedPart.Key), ownedPart.Key);
        }

        //load unit class info
        newMech.MovementSpeed = mechdata.movementSpeed;
        newMech.PlayerNumber = mechdata.playerNumber;
        newMech.displayName = mechdata.displayName;

        return newMech;
    }

    private PartData transcribePart(Part part)
    {
        PartData partData = new PartData();

        partData.partTypeId = UniTable.classGuid[part.GetType()];
        Debug.Log("transcribing part: " + part + " as part type as: " + partData.partTypeId);
        partData.displayName = part.displayName;
        partData.partId = part.partId;

        if (null != part.owner)
        {
            partData.ownerId = part.owner.unitId;
        }

        partData.slots = new List<Part.slot>();
        foreach(Part.slot s in part.slots)
        {
            partData.slots.Add(s);
        }
        
        partData.baseAtt = new attributes(part.baseAtt);
        partData.abilityIds = new List<Guid>();
        foreach (ability a in part.abilities)
        {
            partData.abilityIds.Add(UniTable.classGuid[a.GetType()]);
        }
        
        return partData;
    }

    private Part loadPartData(PartData partData)
    {
        Type partClass = UniTable.GetTypeFromGuid(partData.partTypeId);
        Part part = masterInventory.createPart(partClass);

        part.displayName = partData.displayName;
        part.partId = partData.partId;

        foreach (Part.slot s in partData.slots)
        {
            part.slots.Add(s);
        }

        part.baseAtt.setTo(partData.baseAtt);

        foreach (Guid g in partData.abilityIds)
        {
            ability a = UniTable.abilityDictionary[UniTable.classGuid[UniTable.GetTypeFromGuid(g)]].clone();
            a.parent = part.owner;
            part.abilities.Add(a);
        }
        return part;
    }
}


[Serializable]
class dataLibrary
{
    public List<MechData> playerMechs;
    public List<PartData> masterInventory;
}


[Serializable]
class MechData
{
    public Guid mechTypeId;

    public Guid mechUnitId;
    public List<Guid> abilityIds;

    public Dictionary<Part.slot, Guid> parts;
    
    public attributes dynamicAtt;
    public attributes baseAtt;

    // unit class att
    public String displayName;
    public float movementSpeed;
    public int playerNumber;
}

[Serializable]
class PartData
{
    public Guid partTypeId;

    public String displayName;
    public Guid partId;
    public Guid ownerId;
    public List<Part.slot> slots;

    public attributes baseAtt;
    public List<Guid> abilityIds;
}