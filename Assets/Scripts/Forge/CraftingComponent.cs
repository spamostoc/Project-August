using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class CraftingComponent {
    //component data

    [SerializeField]
    private string name;

    [SerializeField]
    public String componentGuid = Guid.NewGuid().ToString();

    [SerializeField]
    private float cost;

    [SerializeField]
    private componentCategory category;

    [SerializeField]
    private List<componentCategory> nextCategory;

    [SerializeField]
    private List<componentCategory> nextStage;

    public static CraftingComponent noneComponent = new CraftingComponent();
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
        mod, pack, //last category supplements
        faster, harder, better, stronger,
        end //this just makes the check logic work
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

    public bool isEmpty()
    {
        return (this.category == componentCategory.none);
    }
}
