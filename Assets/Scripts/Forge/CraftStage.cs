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

    public float progress;

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
}
