using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftStage {

    private List<CraftingComponent> components;

    public stageCategory category { get; private set; }

    public int stage { get; private set; }

    public float progress;

    public CraftStage(int s)
    {
        this.stage = s;
        if (this.stage == 0)
        {
            this.category = stageCategory.root;
        }
    }

    public enum stageCategory
    {
        none, root, weapon, laser, missile, gun
    }

    //placeholder <- how is this going to work?
    public static readonly IDictionary<stageCategory, List<stageCategory>> keyMappings = new Dictionary<stageCategory, List<stageCategory>> {
        { stageCategory.gun, new List<stageCategory>(new stageCategory[] { stageCategory.laser }) },
        { stageCategory.weapon, new List<stageCategory>(new stageCategory[] { stageCategory.gun, stageCategory.missile }) },
        { stageCategory.root, new List<stageCategory>(new stageCategory[] { stageCategory.weapon }) },
        };

    //placeholder
    public static readonly IDictionary<stageCategory, List<CraftingComponent.componentCategory>> componentMappings = new Dictionary<stageCategory, List<CraftingComponent.componentCategory>> {
        { stageCategory.gun, new List<CraftingComponent.componentCategory>(new CraftingComponent.componentCategory[] { CraftingComponent.componentCategory.barrel }) },
        { stageCategory.weapon, new List<CraftingComponent.componentCategory>(new CraftingComponent.componentCategory[] { CraftingComponent.componentCategory.box }) },
        { stageCategory.root, new List<CraftingComponent.componentCategory>(new CraftingComponent.componentCategory[] { CraftingComponent.componentCategory.box }) }
        };

    public void setCategory(stageCategory category)
    {
        this.category = category;
    }
}
