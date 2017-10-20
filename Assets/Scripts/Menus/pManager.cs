using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class pManager : MonoBehaviour
{
    public static pManager pDataManager;

    private List<CraftingComponent> allComponents = new List<CraftingComponent>();

    /// <summary>
    /// component functions
    /// </summary>

    public void addCraftingComponent(CraftingComponent c)
    {
        allComponents.Add(c);
    }

    public void removeCraftingComponent(CraftingComponent c)
    {
        allComponents.Remove(c);
    }

    public List<CraftingComponent> getCraftingComponents()
    {
        return allComponents;
    }

    public List<CraftingComponent> getCraftingComponents(CraftingComponent.componentCategory c)
    {
        return allComponents.FindAll(comp => comp.getCategory() == c);
    }

    public List<CraftingComponent> getCraftingComponents(List<CraftingComponent.componentCategory> c)
    {
        List<CraftingComponent> ret = new List<CraftingComponent>();
        foreach (CraftingComponent.componentCategory comp in c)
        {
            ret.AddRange(allComponents.FindAll(allComp => allComp.getCategory() == comp));
        }
        return ret;
    }

    // Use this for initialization
    void Awake()
    {
        if (pDataManager == null)
        {
            DontDestroyOnLoad(gameObject);
            pDataManager = this;
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
        Part mp = new Part();
        UniTable.partDictionary.Add(UniTable.classGuid[typeof(Part)], mp);

        SteelCore sc = new SteelCore();
        sc.slots.Add(Part.slot.core);
        UniTable.partDictionary.Add(UniTable.classGuid[typeof(SteelCore)], sc);

        //weapon dictionary
        FlakGunWeapon newFlakGun = new FlakGunWeapon();
        newFlakGun.displayName = "Flak Gun";
        newFlakGun.range = 5;
        newFlakGun.damage = 35;
        newFlakGun.maxAmmo = 8;
        newFlakGun.iconSprite = Resources.Load<Sprite>("BoostAttackIcon") as Sprite;
        newFlakGun.slots.Add(Part.slot.weapon1);
        newFlakGun.slots.Add(Part.slot.weapon2);
        UniTable.partDictionary.Add(UniTable.classGuid[typeof(FlakGunWeapon)], newFlakGun);


        LasGunWeapon newLasGun = new LasGunWeapon();
        newLasGun.displayName = "LasGun";
        newLasGun.range = 2;
        newLasGun.damage = 45;
        newLasGun.iconSprite = Resources.Load<Sprite>("BoostAttackIcon") as Sprite;
        newLasGun.slots.Add(Part.slot.weapon1);
        newLasGun.slots.Add(Part.slot.weapon2);
        UniTable.partDictionary.Add(UniTable.classGuid[typeof(LasGunWeapon)], newLasGun);

        //units dictionary
        //these rely on the above
        Mech m = this.transform.gameObject.AddComponent<Mech>();
        m.Initialize();
        UniTable.unitDictionary.Add(UniTable.classGuid[typeof(Mech)], m);

        //define template rules regarding parts ownership and templated parts
        Mech inter = this.makeIntercessorTemplate();
        UniTable.unitDictionary.Add(new Guid("a36f8211-608f-4afc-be6f-27f5b6143019"), inter);

        //prefabs Table
        UniTable.prefabTable.Add(typeof(Mech), Resources.Load<Transform>("Mech") as Transform);

        initCrafting();
    }

    private void initCrafting()
    {
        //crafting components

        CraftingComponent comp1 = new CraftingComponent();
        comp1.setCategory(CraftingComponent.componentCategory.root);
        comp1.setName("mech");
        this.addCraftingComponent(comp1);

        CraftingComponent comp2 = new CraftingComponent();
        comp2.setCategory(CraftingComponent.componentCategory.root);
        comp2.setName("weapon");
        this.addCraftingComponent(comp2);

        CraftingComponent comp3 = new CraftingComponent();
        comp3.setCategory(CraftingComponent.componentCategory.root);
        comp3.setName("tool");
        this.addCraftingComponent(comp3);

        CraftingComponent comp4 = new CraftingComponent();
        comp4.setCategory(CraftingComponent.componentCategory.core);
        comp4.setName("nuclear");
        this.addCraftingComponent(comp4);

        CraftingComponent comp5 = new CraftingComponent();
        comp5.setCategory(CraftingComponent.componentCategory.core);
        comp5.setName("electic");
        this.addCraftingComponent(comp5);

        CraftingComponent comp6 = new CraftingComponent();
        comp6.setCategory(CraftingComponent.componentCategory.core);
        comp6.setName("ice");
        this.addCraftingComponent(comp6);

    }

    private Mech makeIntercessorTemplate()
    {
        Mech newMech = this.transform.gameObject.AddComponent<Mech>();
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

        //abilities
        newMech.abilities.Add(UniTable.abilityDictionary[UniTable.classGuid[typeof(shoot)]].clone());
        newMech.abilities.Add(UniTable.abilityDictionary[UniTable.classGuid[typeof(switchWeapon)]].clone());
        foreach (ability a in newMech.abilities)
        {
            a.parent = newMech;
        }

        //weapons
        newMech.addPartAs(UniTable.partDictionary[UniTable.classGuid[typeof(FlakGunWeapon)]].clone(), Part.slot.weapon1);
        newMech.addPartAs(UniTable.partDictionary[UniTable.classGuid[typeof(LasGunWeapon)]].clone(), Part.slot.weapon2);

        //parts
        Part part = UniTable.partDictionary[UniTable.classGuid[typeof(SteelCore)]].clone();
        newMech.addPartAs(part, part.slots[0]);

        return newMech;
    }

}