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
        components = new List<CraftingComponent>() { CraftingComponent.noneComponent, CraftingComponent.noneComponent, CraftingComponent.noneComponent };
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
        components = new List<CraftingComponent>() { CraftingComponent.noneComponent, CraftingComponent.noneComponent, CraftingComponent.noneComponent };
    }
}
