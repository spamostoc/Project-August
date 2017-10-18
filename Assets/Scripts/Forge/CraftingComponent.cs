using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftingComponent {

    //component data
    private string name;

    private float cost;

    private componentCategory category;

    //crafting table and detailed parts information kept here?

    public enum componentCategory
    {
        none, barrel, stock, casing, box
    }

    //placeholder
    public static readonly IDictionary<componentCategory, List<componentCategory>> mappings = new Dictionary<componentCategory, List<componentCategory>> {
        { componentCategory.box, new List<componentCategory>(new componentCategory[] { componentCategory.casing }) },
        { componentCategory.barrel, new List<componentCategory>(new componentCategory[] { componentCategory.stock, componentCategory.box }) }
        };


    public string getName() { return name; }

    public void setName(string n) { name = n; }

    public float getCost() { return cost; }

    public void setCost(float c) { cost = c; }

    public componentCategory getCategory() { return category; }

    public void setCategory(componentCategory c) { category = c; }
}
