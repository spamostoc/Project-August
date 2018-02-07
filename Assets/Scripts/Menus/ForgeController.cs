using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class ForgeController : MonoBehaviour {

    public string dynamicTag = "dynamicUI";
    public const int buttonHeight = 30;

    public GameObject optionButtonPrefab;

    public RectTransform managementOptions1;
    public RectTransform managementOptions2;
    public RectTransform managementOptions3;

    public RectTransform rootline1;
    public RectTransform rootline2;
    public RectTransform rootline3;
    public RectTransform rootline4;

    public RectTransform managementPanel;
    private int lineIndex = 0;
    private int nextStageIndex = 0;
    private int currentStageIndex = 0;

    public Construction activeConstruct { get; private set; }

	// Use this for initialization
	void Start () {

        this.managementPanel.gameObject.SetActive(false);

        //root 1
        initLine(rootline1);
        initLine(rootline2);
        initLine(rootline3);
        initLine(rootline4);
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
        pManager.pDataManager.pConstructions.Add(new Construction(line.parent.name));
        activeConstruct = pManager.pDataManager.pConstructions.Find(construct => construct.getLineName() == line.parent.name);

        line.Find("lineButton").gameObject.SetActive(false);
        line.Find("forgeLine").gameObject.SetActive(true);
    }

    public void deleteLine(RectTransform line)
    {
        pManager.pDataManager.pConstructions.Remove(pManager.pDataManager.pConstructions.Find(construct => construct.getLineName() == line.parent.name));
        activeConstruct = null;

        updateStageButtons(line);

        if (lineIndex == lineToIndex(line))
        {
            indexToLine(lineIndex).sizeDelta = new Vector2(0, 200);
            this.managementPanel.gameObject.SetActive(false);
        }

        line.Find("lineButton").gameObject.SetActive(true);
        line.Find("forgeLine").gameObject.SetActive(false);
    }

    public void selectLine(RectTransform line)
    {
        if (lineToIndex(line) == lineIndex && currentStageIndex == nextStageIndex && this.managementPanel.gameObject.activeSelf)
        {
            indexToLine(lineIndex).sizeDelta = new Vector2(0, 200);
            this.managementPanel.gameObject.SetActive(false);
        }
        else
        {
            if (lineToIndex(line) != lineIndex)
            {
                indexToLine(lineIndex).sizeDelta = new Vector2(0, 200);
                lineIndex = lineToIndex(line);
            }

            line.sizeDelta = new Vector2(0, 400);
            this.managementPanel.SetParent(line);
            this.managementPanel.anchoredPosition = new Vector2(0, 0);
            this.managementPanel.gameObject.SetActive(true);
        }

        activeConstruct = pManager.pDataManager.pConstructions.Find(construct => construct.getLineName() == line.parent.name);
        currentStageIndex = nextStageIndex; //this weird interaction allows us to close only on clicking the same button twice
                                            //since we use this shitty stage index to ID the line
        updateOptions();
    }

    private void updateStageButtons(RectTransform line)
    {
        //we have to find the corresponding parent object first to avoid nested stuff
        List<RectTransform> transformParents = new List<RectTransform>(line.GetComponentsInChildren<RectTransform>());
        RectTransform buttonParent = transformParents.FindAll(rect => rect.name == "forgeLine").Find(rect => rect.parent == line);

        List<Button> buttonList = new List<Button>(buttonParent.GetComponentsInChildren<Button>());
        List<Text> textList = new List<Text>(buttonParent.GetComponentsInChildren<Text>());

        if (null != activeConstruct)
        {
            buttonList.Find(button => button.name == "Stage2").interactable = activeConstruct.stages[0].nextStageAvailable();
            buttonList.Find(button => button.name == "Stage3").interactable = activeConstruct.stages[1].nextStageAvailable();
            buttonList.Find(button => button.name == "Stage4").interactable = activeConstruct.stages[2].nextStageAvailable();

            textList.Find(text => text.name == "Stage1 Text").text = activeConstruct.stages[0].getDescriptionText();
            textList.Find(text => text.name == "Stage2 Text").text = activeConstruct.stages[1].getDescriptionText();
            textList.Find(text => text.name == "Stage3 Text").text = activeConstruct.stages[2].getDescriptionText();
            textList.Find(text => text.name == "Stage4 Text").text = activeConstruct.stages[3].getDescriptionText();

            ColorBlock cb;

            if (activeConstruct.stages[0].getCompleted())
            {
                cb = buttonList.Find(button => button.name == "Stage1").colors;
                cb.colorMultiplier = 2.5f;
                buttonList.Find(button => button.name == "Stage1").colors = cb;
            }
            if (activeConstruct.stages[1].getCompleted())
            {
                cb = buttonList.Find(button => button.name == "Stage2").colors;
                cb.colorMultiplier = 2.5f;
                buttonList.Find(button => button.name == "Stage2").colors = cb;
            }
            if (activeConstruct.stages[2].getCompleted())
            {
                cb = buttonList.Find(button => button.name == "Stage3").colors;
                cb.colorMultiplier = 2.5f;
                buttonList.Find(button => button.name == "Stage3").colors = cb;
            }
            if (activeConstruct.stages[3].getCompleted())
            {
                cb = buttonList.Find(button => button.name == "Stage3").colors;
                cb.colorMultiplier = 2.5f;
                buttonList.Find(button => button.name == "Stage3").colors = cb;
            }
        }
        else
        {
            buttonList.Find(button => button.name == "Stage2").interactable = false;
            buttonList.Find(button => button.name == "Stage3").interactable = false;
            buttonList.Find(button => button.name == "Stage4").interactable = false;

            textList.Find(text => text.name == "Stage1 Text").text = "";
            textList.Find(text => text.name == "Stage2 Text").text = "";
            textList.Find(text => text.name == "Stage3 Text").text = "";
            textList.Find(text => text.name == "Stage4 Text").text = "";

            ColorBlock cb;
            cb = buttonList.Find(button => button.name == "Stage1").colors;
            cb.colorMultiplier = 1.0f;
            buttonList.Find(button => button.name == "Stage1").colors = cb;

            cb = buttonList.Find(button => button.name == "Stage2").colors;
            cb.colorMultiplier = 1.0f;
            buttonList.Find(button => button.name == "Stage2").colors = cb;

            cb = buttonList.Find(button => button.name == "Stage3").colors;
            cb.colorMultiplier = 1.0f;
            buttonList.Find(button => button.name == "Stage3").colors = cb;

            cb = buttonList.Find(button => button.name == "Stage3").colors;
            cb.colorMultiplier = 1.0f;
            buttonList.Find(button => button.name == "Stage3").colors = cb;
        }
    }

    public void updateOptions()
    {
        cleanPanels();

        List<CraftingComponent.componentCategory> optionCategories = new List<CraftingComponent.componentCategory>();
        if (currentStageIndex == 0)
        {
            optionCategories.Add(CraftingComponent.componentCategory.root);
        }
        else
        {
            //get first option of each stage by compositing the options from the previous stage
            foreach (CraftingComponent c in activeConstruct.stages[currentStageIndex - 1].getComponents())
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
        //fetch other options sequentially and populate to UI
        List<CraftingComponent> firstOption = pManager.pDataManager.getCraftingComponents(optionCategories);
        populateOptions(managementOptions1, 0, firstOption);

        if (activeConstruct.stages[currentStageIndex].getComponents().Count > 1)
        {
            List<CraftingComponent> secondOption = pManager.pDataManager.getCraftingComponents(activeConstruct.stages[currentStageIndex].getComponents(0).getNextCategory());
            populateOptions(managementOptions2, 1, secondOption);
        }
        
        if (activeConstruct.stages[currentStageIndex].getComponents().Count > 2)
        {
            List<CraftingComponent> thirdOption = pManager.pDataManager.getCraftingComponents(activeConstruct.stages[currentStageIndex].getComponents(1).getNextCategory());
            populateOptions(managementOptions3, 2, thirdOption);
        }

        updateStageButtons(indexToLine(lineIndex));
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
        if (!activeConstruct.stages[nextStageIndex].getLocked())
        {
            activeConstruct.setComponent(nextStageIndex, componentIndex, component);
            this.updateOptions();
        }
    }

    public void initLine(RectTransform line)
    {
        activeConstruct = pManager.pDataManager.pConstructions.Find(construct => construct.getLineName() == line.parent.name);
        if (null != activeConstruct && !activeConstruct.isEmpty())
        {
            line.Find("lineButton").gameObject.SetActive(false);
            line.Find("forgeLine").gameObject.SetActive(true);
            updateStageButtons(line);
        }
    }

    public void setStageIndex(int index)
    {
        this.nextStageIndex = index;
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

    private int lineToIndex(RectTransform line)
    {
        if (rootline1 == line)
        {
            return 0;
        }
        else if (rootline2 == line)
        {
            return 1;
        }
        else if (rootline3 == line)
        {
            return 2;
        }
        else if (rootline4 == line)
        {
            return 3;
        }
        throw new IndexOutOfRangeException("not such line found");
    }

    private RectTransform indexToLine(int index)
    {
        switch (index)
        {
            case 0:
                return rootline1;
            case 1:
                return rootline2;
            case 2:
                return rootline3;
            case 3:
                return rootline4;
            default:
                throw new IndexOutOfRangeException("not such line found");
        }

    }

    public void loadScene(string levelName)
    {
        SceneLoader.sceneLoader.loadScene(levelName);
    }
}
