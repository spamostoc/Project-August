using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class CraftStage {

    [SerializeField]
    private List<CraftingComponent> components;

    [SerializeField]
    private int stage;

    [SerializeField]
    public float progress;

    [SerializeField]
    private bool locked = false;

    [SerializeField]
    private bool completed = false;

    public CraftStage(int s)
    {
        stage = s;

        clearComponents();

    }

    public void setComponent(int componentIndex, CraftingComponent component)
    {
        components[componentIndex] = component;
        for (int i = componentIndex + 1; i < components.Count; i++)
        {
            components[i] = CraftingComponent.noneComponent;
        }
    }

    public List<CraftingComponent> getComponents()
    {
        return components;
    }

    public CraftingComponent getComponents(int i)
    {
        return components[i];
    }


    public void clearComponents()
    {
        switch (stage)
        {
            case 0:
                components = new List<CraftingComponent>() { CraftingComponent.noneComponent };
                break;
            case 1:
                components = new List<CraftingComponent>() { CraftingComponent.noneComponent, CraftingComponent.noneComponent };
                break;
            case 2:
                components = new List<CraftingComponent>() { CraftingComponent.noneComponent, CraftingComponent.noneComponent, CraftingComponent.noneComponent };
                break;
            case 3:
                components = new List<CraftingComponent>() { CraftingComponent.noneComponent, CraftingComponent.noneComponent };
                break;
            default:
                components = new List<CraftingComponent>() { };
                break;
        }
    }

    public string getDescriptionText()
    {
        string ret = "";
        foreach (CraftingComponent c in components)
        {
            ret += c.getName() + " ";
        }
        return ret.Trim();
    }

    public bool nextStageAvailable()
    {
        if (components.Count == 0)
        {
            Debug.Log("error condition");
            return false;
        }

        foreach (CraftingComponent c in components)
        {
            Debug.Log("stage number " + stage + " component name " + c.getName());
            if (c.getCategory() == CraftingComponent.componentCategory.none)
            {
                return false;
            }
        }

        return true;
    }

    public int getStage()
    {
        return stage;
    }

    public bool isEmpty()
    {
        foreach(CraftingComponent c in components)
        {
            if(!c.isEmpty())
            {
                return false;
            }
        }
        return true;
    }

    public void onTick(float timeDelta)
    {
        this.progress += timeDelta;

        if (!locked && progress > 0.0f)
        {
            this.locked = true;
        }

        if (getPercentage() >= 1.0f)
        {
            Debug.Log("completed stage " + this.stage + " with value " + this.progress);
            this.completed = true;
        }
    }

    public float getPercentage()
    {
        float sum = 0.0f;
        foreach (CraftingComponent c in components)
        {
            sum += c.getCost();
        }
        return this.progress / sum;
    }

    public bool getCompleted()
    {
        return this.completed;
    }

    public bool getLocked()
    {
        return this.locked;
    }
}
