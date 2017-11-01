using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftStage {

    public List<CraftingComponent> components { get; private set; }
    public int stage { get; private set; }

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
}
