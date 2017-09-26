using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Construction {
    //this class is used to manage an ongoing construction

    private List<CraftingComponent> components = new List<CraftingComponent>();

    private float currentComponentProgress;
    private int currentComponentIndex;

    public void updateComponent()
    {
        //update component
        //update cost
    }


    public float getProgressRatio()
    {
        float progress = 0f;
        for (int i = 0; i < currentComponentIndex; i++)
        {
            progress += components[i].getCost();
        }

        float totalCost = 0f;
        foreach (CraftingComponent c in components)
        {
            totalCost += c.getCost();
        }

        return progress / totalCost;
    }

}
