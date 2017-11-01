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

    public Button managementStageButton1;
    public Button managementStageButton2;
    public Button managementStageButton3;
    public Button managementStageButton4;

    public Text managementStageText1;
    public Text managementStageText2;
    public Text managementStageText3;
    public Text managementStageText4;

    public RectTransform managementPanel;
    private int lineIndex;
    private int stageIndex;

    private List<Construction> constructions = new List<Construction>();

    public Construction activeConstruct { get; private set; }

	// Use this for initialization
	void Start () {
        managementStageButton2.interactable = false;
        managementStageButton3.interactable = false;
        managementStageButton4.interactable = false;
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
        cleanPanels();

        List<CraftingComponent.componentCategory> optionCategories = new List<CraftingComponent.componentCategory>();
        if (stageIndex == 0)
        {
            optionCategories.Add(CraftingComponent.componentCategory.root);
        }
        else
        {
            foreach (CraftingComponent c in activeConstruct.stages[stageIndex - 1].components)
            {
                List<CraftingComponent.componentCategory> categories = c.getNextStage();
                if (null == categories || categories.Count == 0 || categories[0] == CraftingComponent.componentCategory.none)
                {
                    continue;
                }
                else if (optionCategories.Count == 0)
                {
                    optionCategories.AddRange(categories);
                }
                else
                {
                    optionCategories = optionCategories.Intersect(categories).ToList<CraftingComponent.componentCategory>();
                }
            }
        }
        List<CraftingComponent> firstOption = pManager.pDataManager.getCraftingComponents(optionCategories);
        populateOptions(managementOptions1, 0, firstOption);

        if (activeConstruct.stages[stageIndex].components.Count > 1)
        {
            List<CraftingComponent> secondOption = pManager.pDataManager.getCraftingComponents(activeConstruct.stages[stageIndex].components[0].getNextCategory());
            populateOptions(managementOptions2, 1, secondOption);
        }
        
        if(activeConstruct.stages[stageIndex].components.Count > 2)
        {
            List<CraftingComponent> thirdOption = pManager.pDataManager.getCraftingComponents(activeConstruct.stages[stageIndex].components[1].getNextCategory());
            populateOptions(managementOptions3, 2, thirdOption);
        }

        managementStageButton2.interactable = activeConstruct.stages[0].nextStageAvailable();
        managementStageButton3.interactable = activeConstruct.stages[1].nextStageAvailable();
        managementStageButton4.interactable = activeConstruct.stages[2].nextStageAvailable();

        //populate button text
        managementStageText1.text = activeConstruct.stages[0].getDescriptionText();
        managementStageText2.text = activeConstruct.stages[1].getDescriptionText();
        managementStageText3.text = activeConstruct.stages[2].getDescriptionText();
        managementStageText4.text = activeConstruct.stages[3].getDescriptionText();
    }

    public void setLineIndex(int index)
    {
        this.lineIndex = index;
    }

    public void setStageIndex(int index)
    {
        this.stageIndex = index;
    }

    private bool populateOptions(RectTransform optionTransform, int optionIndex, List<CraftingComponent> options)
    {
        if (null == options || options.Count == 0)
        {
            Debug.Log("no drop options, clearing list for drop: " + optionTransform);
            return false;
        }

        int i = 0;
        foreach (CraftingComponent c in options)
        {
            GameObject newButton = Instantiate(optionButtonPrefab);
            newButton.transform.SetParent(optionTransform);
            newButton.transform.localScale = new Vector3(1, 1, 1);

            RectTransform buttonTransform = newButton.GetComponent<RectTransform>();
            buttonTransform.anchorMin = new Vector2(.03f, 1f);
            buttonTransform.anchorMax = new Vector2(0.97f, 1f);
            buttonTransform.anchoredPosition = new Vector2(0, (-0.55f - i) * buttonHeight);
            buttonTransform.sizeDelta = new Vector2(optionTransform.rect.width * 0.90f, buttonHeight);
            buttonTransform.tag = dynamicTag;

            Button buttonComponent = newButton.GetComponent<Button>();

            int newIndexArg = optionIndex;
            CraftingComponent newCompArg = c;
            buttonComponent.onClick.AddListener(() => selectOption(newIndexArg, newCompArg));

            buttonTransform.Find("Option Name").GetComponent<Text>().text = c.getName();
            i++;
        }
        return true;
    }

    public void selectOption(int componentIndex, CraftingComponent component)
    {
        activeConstruct.setComponent(stageIndex, componentIndex, component);
        this.updateOptions();
    }

    private void cleanPanels()
    {
        foreach (RectTransform r in managementOptions1.GetComponentsInChildren<RectTransform>())
        {
            if (r.tag.Equals(dynamicTag))
                GameObject.DestroyObject(r.gameObject);
        }
        foreach (RectTransform r in managementOptions2.GetComponentsInChildren<RectTransform>())
        {
            if (r.tag.Equals(dynamicTag))
                GameObject.DestroyObject(r.gameObject);
        }
        foreach (RectTransform r in managementOptions3.GetComponentsInChildren<RectTransform>())
        {
            if (r.tag.Equals(dynamicTag))
                GameObject.DestroyObject(r.gameObject);
        }
    }
}
