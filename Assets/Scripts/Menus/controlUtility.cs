using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class controlUtility : MonoBehaviour {

    private string GAME_DATA_LIBRARY = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/GitHub/August/Assets/Resources" + "/gameData.dat";
    private string PLAYER_DATA_LIBRARY = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/GitHub/August/Assets/Resources" + "/playerData.dat";

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

        masterInventory.addMech(newMech.unitId, newMech);
    }

    public void savePlayerData()
    {
        playerDataLibrary pdl = new playerDataLibrary();

        foreach (Mech pmechj in masterInventory.getMechs())
        {
            pdl.playerMechs.Add(transcribeMech(pmechj));
        }

        foreach (Part part in masterInventory.getParts())
        {
            pdl.masterInventory.Add(transcribePart(part));
        }

        pdl.playerConstructions = pManager.pDataManager.pConstructions;

        File.WriteAllText(PLAYER_DATA_LIBRARY, JsonUtility.ToJson(pdl));
    }

    public void saveGameData()
    {
        gameDataLibrary gdl = new gameDataLibrary();

        gdl.craftingRecipes = pManager.pDataManager.getCraftingComponents();

        File.WriteAllText(GAME_DATA_LIBRARY, JsonUtility.ToJson(gdl));
    }

    public void loadPlayerData()
    {
        string jsonString = File.ReadAllText(PLAYER_DATA_LIBRARY);
        playerDataLibrary pdl = JsonUtility.FromJson<playerDataLibrary>(jsonString);
        
        masterInventory.clearParts();
        foreach (PartData partdata in pdl.masterInventory)
        {
            loadPartData(partdata);
        }

        masterInventory.clearMechs();
        foreach (MechData mdata in pdl.playerMechs)
        {
            Mech newMech = loadMechData(mdata);
            masterInventory.addMech(newMech.unitId, newMech);
        }

        foreach (Construction c in pdl.playerConstructions)
        {
            pManager.pDataManager.pConstructions.Add(c);
        }
    }

    public void loadGameData()
    {
        string jsonString = File.ReadAllText(GAME_DATA_LIBRARY);
        gameDataLibrary gdl = JsonUtility.FromJson<gameDataLibrary>(jsonString);

        Debug.Log(pManager.pDataManager.getCraftingComponents().Count);
        pManager.pDataManager.clearCraftingComponents();
        Debug.Log(pManager.pDataManager.getCraftingComponents().Count);
        foreach (CraftingComponent c in gdl.craftingRecipes)
        {
            pManager.pDataManager.addCraftingComponent(c);
        }
        Debug.Log(pManager.pDataManager.getCraftingComponents().Count);
    }

    private MechData transcribeMech(Mech mech)
    {
        MechData mdata = new MechData();
        mdata.mechTypeId = UniTable.classGuid[mech.GetType()].ToString();

        mdata.mechUnitId = mech.unitId.ToString();

        //transcribe abilities
        mdata.abilityIds = new List<String>();
        foreach (ability a in mech.abilities)
        {
            mdata.abilityIds.Add(UniTable.classGuid[a.GetType()].ToString());
        }

        //transcribe dynamic attributes
        mdata.dynamicAtt = new attributes(mech.dynamicAttributes);

        //transcribe base attributes
        Debug.Log(mech.baseAtt.maxMovementPoints);
        mdata.baseAtt = new attributes(mech.baseAtt);
        Debug.Log(mdata.baseAtt.maxMovementPoints);

        //transcribe mech class info
        mdata.parts = new List<partOwnership>();
        foreach (KeyValuePair<Part.slot, Part> ownedPart in mech.parts)
        {
            mdata.parts.Add(new partOwnership(ownedPart.Key, ownedPart.Value.partId.ToString()));
        }

        //transcribe unit class info
        mdata.movementSpeed = mech.MovementSpeed;
        mdata.playerNumber = mech.PlayerNumber;
        mdata.displayName = mech.displayName;
        return mdata;
    }

    private Mech loadMechData(MechData mechdata)
    {
        Type mechClass = UniTable.GetTypeFromGuid(new Guid(mechdata.mechTypeId));
        if (mechClass == null)
        {
            Debug.Log("no such class found");
            return null;
        }

        //load mech
        Mech newMech = (Mech)pManager.pDataManager.transform.gameObject.AddComponent(mechClass);
        newMech.Initialize();

        newMech.unitId = new Guid(mechdata.mechUnitId);

        //load abilities
        foreach (String g in mechdata.abilityIds)
        {
            Guid mechGuid = new Guid(g);
            ability a = UniTable.abilityDictionary[UniTable.classGuid[UniTable.GetTypeFromGuid(mechGuid)]].clone();
            a.parent = newMech;
            newMech.abilities.Add(a);
        }
        Debug.Log(newMech.abilities.Count);

        //load dynamic attributes
        newMech.dynamicAttributes.setTo(mechdata.dynamicAtt);

        //load base attributes
        newMech.baseAtt.setTo(mechdata.baseAtt);

        //load mech class info
        foreach (partOwnership ownedPart in mechdata.parts)
        {
            Debug.Log("slot: " + ownedPart.slot + " has item: " + ownedPart.partId);
            Guid partGuid = new Guid(ownedPart.partId);
            newMech.addPartAs(masterInventory.getPart(partGuid, ownedPart.slot), ownedPart.slot);
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

        partData.partTypeId = UniTable.classGuid[part.GetType()].ToString();
        partData.displayName = part.displayName;
        partData.partId = part.partId.ToString();

        if (null != part.owner)
        {
            partData.ownerId = part.owner.unitId.ToString();
        }

        partData.slots = new List<Part.slot>();
        foreach(Part.slot s in part.slots)
        {
            partData.slots.Add(s);
        }
        
        partData.baseAtt = new attributes(part.baseAtt);
        partData.abilityIds = new List<String>();
        foreach (ability a in part.abilities)
        {
            partData.abilityIds.Add(UniTable.classGuid[a.GetType()].ToString());
        }
        
        return partData;
    }

    private Part loadPartData(PartData partData)
    {
        Type partClass = UniTable.GetTypeFromGuid(new Guid(partData.partTypeId));
        Part part = masterInventory.createPart(partClass);

        part.displayName = partData.displayName;
        part.partId = new Guid(partData.partId);

        foreach (Part.slot s in partData.slots)
        {
            part.slots.Add(s);
        }

        part.baseAtt.setTo(partData.baseAtt);

        foreach (String g in partData.abilityIds)
        {
            Guid abilityGuid = new Guid(g);
            ability a = UniTable.abilityDictionary[UniTable.classGuid[UniTable.GetTypeFromGuid(abilityGuid)]].clone();
            a.parent = part.owner;
            part.abilities.Add(a);
        }
        return part;
    }
}


[Serializable]
class gameDataLibrary
{
    public List<CraftingComponent> craftingRecipes = new List<CraftingComponent>();
}

[Serializable]
class playerDataLibrary
{
    public List<MechData> playerMechs = new List<MechData>();
    public List<PartData> masterInventory = new List<PartData>();
    public List<Construction> playerConstructions = new List<Construction>();
}

[Serializable]
class MechData
{
    public String mechTypeId;

    public String mechUnitId;
    public List<String> abilityIds;

    public List<partOwnership> parts;
    
    public attributes dynamicAtt;
    public attributes baseAtt;

    // unit class att
    public String displayName;
    public float movementSpeed;
    public int playerNumber;
}

[Serializable]
class partOwnership
{
    public Part.slot slot;
    public String partId;

    public partOwnership(Part.slot slot, String partId)
    {
        this.slot = slot;
        this.partId = partId;
    }
}

[Serializable]
class PartData
{
    public String partTypeId;

    public String displayName;
    public String partId;
    public String ownerId;
    public List<Part.slot> slots;

    public attributes baseAtt;
    public List<String> abilityIds;
}