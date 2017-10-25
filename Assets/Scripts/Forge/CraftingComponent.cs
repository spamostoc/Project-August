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

    //placeholder <- how is this going to work?
    // perhaps use a logic map of nested switch? first option sets category unless successive options override?
    /*public static readonly IDictionary<componentCategory, List<componentCategory>> stageMappings = new Dictionary<componentCategory, List<componentCategory>> {
        { componentCategory.none, new List<componentCategory>(new componentCategory[] { }) },

        { componentCategory.weapon, new List<componentCategory>(new componentCategory[] { componentCategory.core }) },
        { componentCategory.mech, new List<componentCategory>(new componentCategory[] { componentCategory.powerplant }) },

        { componentCategory.core, new List<componentCategory>(new componentCategory[] { componentCategory.gun, componentCategory.laser, componentCategory.missile, componentCategory.tool, componentCategory.shield, componentCategory.chassis }) },

        { componentCategory.gun, new List<componentCategory>(new componentCategory[] { componentCategory.mod, componentCategory.pack }) },
        { componentCategory.laser, new List<componentCategory>(new componentCategory[] { componentCategory.mod, componentCategory.pack }) },
        { componentCategory.missile, new List<componentCategory>(new componentCategory[] { componentCategory.mod, componentCategory.pack }) },

        { componentCategory.tool, new List<componentCategory>(new componentCategory[] { componentCategory.mod, componentCategory.pack }) },
        { componentCategory.shield, new List<componentCategory>(new componentCategory[] { componentCategory.mod, componentCategory.pack }) },
        { componentCategory.chassis, new List<componentCategory>(new componentCategory[] { componentCategory.mod, componentCategory.pack }) }
        };

    //placeholder
    public static readonly IDictionary<componentCategory, List<componentCategory>> componentMappings = new Dictionary<componentCategory, List<componentCategory>> {
        { componentCategory.weapon, new List<componentCategory>(new componentCategory[] {}) },
        { componentCategory.mech, new List<componentCategory>(new componentCategory[] {}) },
        { componentCategory.utility, new List<componentCategory>(new componentCategory[] {}) },

        { componentCategory.core, new List<componentCategory>(new componentCategory[] { componentCategory.coreSupplement }) },
        { componentCategory.gun, new List<componentCategory>(new componentCategory[] { componentCategory.gunAction, componentCategory.gunSubAction }) },
        { componentCategory.laser, new List<componentCategory>(new componentCategory[] { componentCategory.laserAction, componentCategory.laserSubAction }) },
        { componentCategory.missile, new List<componentCategory>(new componentCategory[] { componentCategory.missileAction, componentCategory.missileSubAction }) },
        { componentCategory.tool, new List<componentCategory>(new componentCategory[] {  }) },
        { componentCategory.shield, new List<componentCategory>(new componentCategory[] {  }) },
        { componentCategory.chassis, new List<componentCategory>(new componentCategory[] {  }) },
        { componentCategory.mod, new List<componentCategory>(new componentCategory[] {  }) },
        { componentCategory.pack, new List<componentCategory>(new componentCategory[] {  }) }
        };*/

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
