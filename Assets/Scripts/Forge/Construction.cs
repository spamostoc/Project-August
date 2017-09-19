using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Construction {
    //this class is used to manage an ongoing construction

    private List<CraftingComponent> stage1Components;
    private List<CraftingComponent> stage2Components;
    private List<CraftingComponent> stage3Components;
    private List<CraftingComponent> stage4Components;

    private float progress;
    private float totalCost;

    public void updateComponent()
    {
        //update component
        //update cost
    }

    public float getProgressRatio()
    {
        return progress / totalCost;
    }

}
