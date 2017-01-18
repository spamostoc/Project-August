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
        UniTable.partDictionary.Add(typeof(mechPart), mp);

        steelCore sc = this.transform.gameObject.AddComponent<steelCore>();
        sc.Initialize();
        UniTable.partDictionary.Add(typeof(steelCore), sc);

        //weapon dictionary
        flakGunWeapon newFlakGun = new flakGunWeapon();
        newFlakGun.Initialize();
        newFlakGun.range = 5;
        newFlakGun.iconSprite = Resources.Load<Sprite>("BoostAttackIcon") as Sprite;
        UniTable.weapondictionary.Add(typeof(flakGunWeapon), newFlakGun);

        //ability dictionary
        shoot newShoot = new shoot();
        newShoot.Initialize();
        newShoot.iconSprite = Resources.Load<Sprite>("BoostAttackIcon") as Sprite;
        UniTable.abilityDictionary.Add(typeof(shoot), newShoot);

        //prefabs Table

        UniTable.prefabTable.Add(typeof(mech), Resources.Load<Transform>("Alien4") as Transform);
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