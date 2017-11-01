using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CraftingComponent {

    public static CraftingComponent noneComponent = new CraftingComponent();

    //component data
    public Guid componentGuid;

    private string name;

    private float cost;

    private componentCategory category;

    private List<componentCategory> nextCategory;

    private List<componentCategory> nextStage;

    //crafting table and detailed parts information kept here?

    public enum componentCategory
    {
        none,
        root, //weapon, utility, mech
        powerplant, core, coreSupplement, //nuclear, electric, ice, fire
        //stage 3
        gun, gunAction, gunSubAction,
        laser, laserAction, laserSubAction,
        missile, missileAction, missileSubAction,
        tool,
        shield,
        chassis,
        //stage 4
        mod, pack //last category supplements
    }


    public CraftingComponent(String name, componentCategory category, List<componentCategory> nextCat, List<componentCategory> nextStage)
    {
        this.name = name;
        this.category = category;
        this.nextCategory = nextCat;
        this.nextStage = nextStage;
    }

    public CraftingComponent() { }

    public string getName() { return name; }

    public void setName(string n) { name = n; }

    public float getCost() { return cost; }

    public void setCost(float c) { cost = c; }

    public componentCategory getCategory() { return category; }

    public void setCategory(componentCategory c) { category = c; }

    public List<componentCategory> getNextStage() { return nextStage; }

    public void setnextStage(List<componentCategory> c) { nextStage = c; }

    public List<componentCategory> getNextCategory() { return nextCategory; }

    public void setNextCategory(List<componentCategory> c) { nextCategory = c; }
}
