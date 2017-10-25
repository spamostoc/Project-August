using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Construction {
    //this class is used to manage an ongoing construction

    public Construction(string name)
    {
        this.lineName = name;
    }

    public List<CraftStage> stages = new List<CraftStage>()
    {
        new CraftStage(0), new CraftStage(1), new CraftStage(2), new CraftStage(3)
    };
    
    public string lineName { get; private set; }

    public void setIndex(string name)
    {
        lineName = name;
    }

    public void setComponent(int stageIndex, int componentIndex, CraftingComponent component)
    {
        stages[stageIndex].setComponent(componentIndex, component);
        for (int i = stageIndex + 1; i < stages.Count; i++)
        {
            stages[i].clearComponents();
        }
    }

}
