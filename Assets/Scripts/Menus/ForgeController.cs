using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ForgeController : MonoBehaviour {

    private const string ROOTLINE1 = "rootLine1";
    private const string ROOTLINE2 = "rootLine2";
    private const string ROOTLINE3 = "rootLine3";
    private const string ROOTLINE4 = "rootLine4";
    private const string ROOTLINE5 = "rootLine5";

    public RectTransform managementPanel;
    private int lineIndex;
    private int stageIndex;

    private List<Construction> constructions = new List<Construction>();

    public Construction activeConstruct { get; private set; }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// button handlers
    /// </summary>

    public void spawnPart()
    {
        masterInventory.createPart(typeof(SteelCore));
    }

    public void spawnWeapon()
    {
        masterInventory.createPart(typeof(LasGunWeapon));
    }

    public void spawnMech()
    {

    }

    public void startLine(RectTransform line)
    {
        line.Find("lineButton").gameObject.SetActive(false);
        line.Find("forgeLine").gameObject.SetActive(true);

        constructions.Add(new Construction(line.name));
    }

    public void selectLine(RectTransform line)
    {      
        line.sizeDelta = new Vector2(0, 400);
        this.managementPanel.SetParent(line);
        this.managementPanel.anchoredPosition = new Vector2(0, 0);
        this.managementPanel.gameObject.SetActive(true);

        activeConstruct = constructions.Find(construct => construct.lineName == line.name);
        updateOptions();
    }

    public void updateOptions()
    {
        Debug.Log("index is " + stageIndex);
        Dropdown firstDrop = this.managementPanel.Find("optionDropdown1").GetComponent<Dropdown>();
        List<CraftingComponent.componentCategory> optionCategories = CraftStage.componentMappings[activeConstruct.stages[stageIndex].category];
        List<CraftingComponent> firstOption = pManager.pDataManager.getCraftingComponents(optionCategories);
        populateOptions(firstDrop, firstOption);

        Dropdown secondDrop = this.managementPanel.Find("optionDropdown2").GetComponent<Dropdown>();
        CraftingComponent.componentCategory firstCat = (CraftingComponent.componentCategory) Enum.Parse(typeof(CraftingComponent.componentCategory), firstDrop.options[firstDrop.value].text);
        List<CraftingComponent> secondOption = pManager.pDataManager.getCraftingComponents(CraftingComponent.mappings[firstCat]);
        populateOptions(secondDrop, secondOption);

        Dropdown thirdDrop = this.managementPanel.Find("optionDropdown3").GetComponent<Dropdown>();
        CraftingComponent.componentCategory secondCat = (CraftingComponent.componentCategory) Enum.Parse(typeof(CraftingComponent.componentCategory), secondDrop.options[secondDrop.value].text);
        List<CraftingComponent> thirdOption = pManager.pDataManager.getCraftingComponents(CraftingComponent.mappings[secondCat]);
        populateOptions(thirdDrop, thirdOption);
    }

    public void setLineIndex(int index)
    {
        this.lineIndex = index;
    }

    public void setStageIndex(int index)
    {
        this.stageIndex = index;
    }

    private void populateOptions(Dropdown drop, List<CraftingComponent> options)
    {
        //allow populate null list to clear list
        drop.ClearOptions();

        try {
            List<Dropdown.OptionData> optList = new List<Dropdown.OptionData>();

            foreach (CraftingComponent c in options)
            {
                optList.Add(new Dropdown.OptionData(c.getName()));
            }

            drop.AddOptions(optList);
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("no drop options, clearing list for drop: " + drop);
        }
    }
}
