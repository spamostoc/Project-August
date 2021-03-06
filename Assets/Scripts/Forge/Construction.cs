﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[Serializable]
public class Construction {
    //this class is used to manage an ongoing construction

    [SerializeField]
    private string lineName;

    [SerializeField]
    private bool completed = false;

    public List<CraftStage> stages = new List<CraftStage>()
    {
        new CraftStage(0), new CraftStage(1), new CraftStage(2), new CraftStage(3)
    };

    public Construction(string name)
    {
        this.lineName = name;
    }

    public string getLineName()
    {
        return lineName;
    }

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

    public bool isEmpty()
    {
        foreach (CraftStage s in stages)
        {
            if (!s.isEmpty())
            {
                return false;
            }
        }
        return true;
    }

    public void onTick(float timeDelta)
    {
        if (completed)
        {
            return;
        }

        int completeCount = 0;
        for (int i = 0; i < stages.Count; i++)
        {
            CraftStage cs = stages.Find(s => s.getStage() == i);
            if (!cs.getCompleted())
            {
                cs.onTick(timeDelta);
                if (cs.getCompleted())
                {
                    completeCount++;
                }
                break;
            }
            else
            {
                completeCount++;
            }
        }
        
        if (completeCount >= stages.Count)
        {
            masterInventory.createPart(typeof(LasGunWeapon));
            completed = true;
        }
    }
}
