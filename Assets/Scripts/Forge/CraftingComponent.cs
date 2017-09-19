using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftingComponent {

    //component data
    private string name;

    private float cost;

    private craftingCategories category;

    //crafting table and detailed parts information kept here?

    public enum craftingCategories
    {
        laser, missile, gun
    }

    public static readonly IDictionary<craftingCategories, List<craftingCategories>> categoryMappings = new Dictionary<craftingCategories, List<craftingCategories>> {
        { craftingCategories.laser, new List<craftingCategories>(new craftingCategories[] { craftingCategories.missile }) }
        };


    public string getName() { return name; }

    public void setName(string n) { name = n; }

    public float getCost() { return cost; }

    public void setCost(float c) { cost = c; }

    public craftingCategories getCategory() { return category; }

    public void setCategory(craftingCategories c) { category = c; }

}
