using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class ForgeController : MonoBehaviour {

    private const string ROOTLINE1 = "rootLine1";
    private const string ROOTLINE2 = "rootLine2";
    private const string ROOTLINE3 = "rootLine3";
    private const string ROOTLINE4 = "rootLine4";
    private const string ROOTLINE5 = "rootLine5";

    public string dynamicTag = "dynamicUI";
    public const int buttonHeight = 30;

    public GameObject optionButtonPrefab;

    public RectTransform managementOptions1;
    public RectTransform managementOptions2;
    public RectTransform managementOptions3;

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
        List<CraftingComponent.componentCategory> optionCategories = new List<CraftingComponent.componentCategory>();
        if (stageIndex == 0)
        {
            optionCategories.Add(CraftingComponent.componentCategory.root);
        }
        else
        {
            foreach (CraftingComponent c in activeConstruct.stages[stageIndex - 1].components)
            {
                List<CraftingComponent.componentCategory> categories = CraftingComponent.stageMappings[c.getCategory()];
                if (optionCategories.Count == 0)
                {
                    optionCategories.AddRange(categories);
                }
                else if (null == categories || categories.Count == 0)
                {
                    continue;
                }
                else
                {
                    optionCategories = (List<CraftingComponent.componentCategory>) optionCategories.Intersect(categories);
                }
            }
        }
        List<CraftingComponent> firstOption = pManager.pDataManager.getCraftingComponents(optionCategories);
        populateOptions(managementOptions1, 0, firstOption);

        /*CraftingComponent.componentCategory firstCat = (CraftingComponent.componentCategory) Enum.Parse(typeof(CraftingComponent.componentCategory), firstDrop.options[firstDrop.value].text);
        List<CraftingComponent> secondOption = pManager.pDataManager.getCraftingComponents(CraftingComponent.componentMappings[firstCat]);
        populateOptions(managementOptions2, 1, secondOption);

       /// CraftingComponent.componentCategory secondCat = (CraftingComponent.componentCategory) Enum.Parse(typeof(CraftingComponent.componentCategory), secondDrop.options[secondDrop.value].text);
        List<CraftingComponent> thirdOption = pManager.pDataManager.getCraftingComponents(CraftingComponent.componentMappings[secondCat]);
        populateOptions(managementOptions3, 2, thirdOption);*/
    }

    public void setLineIndex(int index)
    {
        this.lineIndex = index;
    }

    public void setStageIndex(int index)
    {
        this.stageIndex = index;
    }

    private void populateOptions(RectTransform optionTransform, int optionIndex, List<CraftingComponent> options)
    {
        //allow populate null list to clear list
        cleanPanel(optionTransform);

        if (null == options || options.Count == 0)
        {
            Debug.Log("no drop options, clearing list for drop: " + optionTransform);
            return;
        }

        int i = 0;
        foreach (CraftingComponent c in options)
        {
            GameObject newButton = (GameObject)Instantiate(optionButtonPrefab);
            newButton.transform.SetParent(optionTransform);
            newButton.transform.localScale = new Vector3(1, 1, 1);

            RectTransform buttonTransform = newButton.GetComponent<RectTransform>();
            buttonTransform.anchorMin = new Vector2(.03f, 1f);
            buttonTransform.anchorMax = new Vector2(0.97f, 1f);
            buttonTransform.anchoredPosition = new Vector2(0, (-0.55f - i) * buttonHeight);
            buttonTransform.sizeDelta = new Vector2(optionTransform.rect.width * 0.90f, buttonHeight);
            buttonTransform.tag = dynamicTag;

            Button buttonComponent = newButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => selectOption(optionIndex, c));

            buttonTransform.Find("Option Name").GetComponent<Text>().text = c.getName();
            i++;
        }
    }

    public void selectOption(int componentIndex, CraftingComponent component)
    {
        activeConstruct.stages[stageIndex].components[componentIndex] = component;
    }

    private void cleanPanel(Transform activeP)
    {
        if (activeP == null)
            return;
        foreach (RectTransform r in activeP.GetComponentsInChildren<RectTransform>())
        {
            if (r.tag.Equals(dynamicTag))
                GameObject.DestroyObject(r.gameObject);
        }
    }
}
