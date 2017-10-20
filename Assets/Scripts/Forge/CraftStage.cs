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
        components = new List<CraftingComponent>() { new CraftingComponent(), new CraftingComponent(), new CraftingComponent() };
    }
}
