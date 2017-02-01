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
        //ability dictionary
        shoot newShoot = new shoot();
        newShoot.Initialize();
        newShoot.iconSprite = Resources.Load<Sprite>("BoostAttackIcon") as Sprite;
        UniTable.abilityDictionary.Add(UniTable.classGuid[typeof(shoot)], newShoot);

        switchWeapon newSwitchWeapon = new switchWeapon();
        newSwitchWeapon.Initialize();
        newSwitchWeapon.iconSprite = Resources.Load<Sprite>("BoostAttackIcon") as Sprite;
        UniTable.abilityDictionary.Add(UniTable.classGuid[typeof(switchWeapon)], newSwitchWeapon);

        //parts dictionary
        mechPart mp = this.transform.gameObject.AddComponent<mechPart>();
        mp.Initialize();
        UniTable.partDictionary.Add(UniTable.classGuid[typeof(mechPart)], mp);

        steelCore sc = this.transform.gameObject.AddComponent<steelCore>();
        sc.Initialize();
        UniTable.partDictionary.Add(UniTable.classGuid[typeof(steelCore)], sc);

        //weapon dictionary
        flakGunWeapon newFlakGun = new flakGunWeapon();
        newFlakGun.Initialize();
        newFlakGun.range = 5;
        newFlakGun.iconSprite = Resources.Load<Sprite>("BoostAttackIcon") as Sprite;
        UniTable.weapondictionary.Add(UniTable.classGuid[typeof(flakGunWeapon)], newFlakGun);

        //units dictionary
        //these rely on the above
        mech m = this.transform.gameObject.AddComponent<mech>();
        m.Initialize();
        UniTable.unitDictionary.Add(UniTable.classGuid[typeof(mech)], m);
        mech inter = this.makeIntercessorTemplate();
        UniTable.unitDictionary.Add(new Guid("a36f8211-608f-4afc-be6f-27f5b6143019"), inter);

        //prefabs Table

        UniTable.prefabTable.Add(typeof(mech), Resources.Load<Transform>("Alien4") as Transform);
    }

    private mech makeIntercessorTemplate()
    {
        mech newMech = this.transform.gameObject.AddComponent<mech>();
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

        //weapons
        newMech.weapons.Add(UniTable.weapondictionary[UniTable.classGuid[typeof(flakGunWeapon)]].clone());
        foreach (mechWeapon w in newMech.weapons)
        {
            w.parent = newMech;
        }

        //abilities
        newMech.abilities.Add(UniTable.abilityDictionary[UniTable.classGuid[typeof(shoot)]].clone());
        newMech.abilities.Add(UniTable.abilityDictionary[UniTable.classGuid[typeof(switchWeapon)]].clone());
        foreach (ability a in newMech.abilities)
        {
            a.parent = newMech;
        }

        //parts
        steelCore part = pManager.pDataManager.transform.gameObject.AddComponent<steelCore>();
        part.copyFrom(UniTable.partDictionary[UniTable.classGuid[typeof(steelCore)]]);
        newMech.parts.Add(part);
        foreach (mechPart mp in newMech.parts)
        {
            mp.parent = newMech;
        }
        return newMech;
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

    public List<Guid> abilityIds;

    // dynamic att
    public attributes dynamicAtt;

    //base att
    public attributes baseAtt;

    //mech class att
    public List<Guid> weaponIds;

    // unit class att
    public String displayName;
    public float movementSpeed;
    public int playerNumber;


}