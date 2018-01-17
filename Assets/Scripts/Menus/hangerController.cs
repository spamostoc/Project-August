using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class hangerController : MonoBehaviour {


    private enum panelFunc { first, second, third };

    public RectTransform rootPanel;
    private ScrollPanel rootScroll;
    public RectTransform rootTop;
    public Button rootEdgeButton;

    public RectTransform secondPanel;
    private ScrollPanel secondScroll;
    public RectTransform secondTop;
    public Button secondEdgeButton;

    public RectTransform thirdPanel;
    private ScrollPanel thirdScroll;
    public RectTransform thirdTop;
    public Button thirdEdgeButton;

    private Transform activePanel;

    public GameObject mechButtonPrefab;
    public GameObject slotButtonPrefab;
    public GameObject partButtonPrefab;
    public GameObject innerPanelPrefab;

    private Mech activeMech;
    private Part.slot activeSlot;

    public static int minimizedPanelSize = 0;
    public static int maximizedPanelSize = 500;
    public static float innerPanelXScale = 0.8f;
    public static float innerPanelYScale = 0.8f;
    public static float buttonHeight = 100;

    public string dynamicTag = "dynamicUI";
    
	// Use this for initialization
	void Start () {
        rootScroll = new ScrollPanel(rootPanel);
        rootScroll.scrollPanel.gameObject.SetActive(false);
        rootPanel.Find("Top Panel").gameObject.SetActive(false);

        secondScroll = new ScrollPanel(secondPanel);
        secondScroll.scrollPanel.gameObject.SetActive(false);
        secondPanel.Find("Top Panel").gameObject.SetActive(false);
        secondEdgeButton.interactable = false;

        thirdScroll = new ScrollPanel(thirdPanel);
        thirdScroll.scrollPanel.gameObject.SetActive(false);
        thirdPanel.Find("Top Panel").gameObject.SetActive(false);
        thirdEdgeButton.interactable = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// panel population
    /// </summary>
    #region
    private void activateMechPanel()
    {

        int i = 0;
        foreach (Mech m in masterInventory.getMechs())
        {
            GameObject newButton = (GameObject)Instantiate(mechButtonPrefab);
            newButton.transform.SetParent(rootScroll.contentPanel);
            newButton.transform.localScale = new Vector3(1, 1, 1);

            RectTransform buttonTransform = newButton.GetComponent<RectTransform>();
            buttonTransform.anchorMin = new Vector2(.03f, 1f);
            buttonTransform.anchorMax = new Vector2(0.97f, 1f);
            buttonTransform.anchoredPosition = new Vector2(0, (-0.55f - i) * buttonHeight);
            buttonTransform.sizeDelta = new Vector2(rootScroll.contentPanel.rect.width * 0.90f, buttonHeight);
            buttonTransform.tag = dynamicTag;

            Button buttonComponent = newButton.GetComponent<Button>();
            MechButtonLink MB = new MechButtonLink(m, buttonComponent);
            buttonComponent.onClick.AddListener(() => selectMech(MB.mech));

            buttonTransform.Find("Mech Name").GetComponent<Text>().text = m.displayName;
            buttonTransform.Find("Attribute1").Find("Name").GetComponent<Text>().text = "Health";
            buttonTransform.Find("Attribute1").Find("Value").GetComponent<Text>().text = m.baseAtt.maxHealth.ToString();

            buttonTransform.Find("Attribute2").Find("Name").GetComponent<Text>().text = "Shields";
            buttonTransform.Find("Attribute2").Find("Value").GetComponent<Text>().text = m.baseAtt.maxShieldPoints.ToString();

            buttonTransform.Find("Attribute3").Find("Name").GetComponent<Text>().text = "Movement";
            buttonTransform.Find("Attribute3").Find("Value").GetComponent<Text>().text = m.baseAtt.maxMovementPoints.ToString();
            i++;
        }

        if (Mathf.Abs(i * buttonHeight) > Mathf.Abs(rootScroll.viewport.rect.y))
        {
            rootScroll.contentPanel.sizeDelta = new Vector2(rootScroll.contentPanel.rect.x,  Mathf.Abs((i + 0.55f) * buttonHeight) - Mathf.Abs(rootScroll.viewport.rect.y));
        }

        rootPanel.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(maximizedPanelSize, 0);

        rootPanel.Find("Top Panel").gameObject.SetActive(true);

        rootScroll.scrollPanel.gameObject.SetActive(true);
    }

    private void activateComponentsPanel(ScrollPanel scroll, Mech mech)
    {
        int i = 0;
        foreach (Part.slot s in mech.parts.Keys)
        {
            GameObject newButton = (GameObject)Instantiate(slotButtonPrefab);
            newButton.transform.SetParent(scroll.contentPanel);
            newButton.transform.localScale = new Vector3(1, 1, 1);

            RectTransform buttonTransform = newButton.GetComponent<RectTransform>();
            buttonTransform.anchorMin = new Vector2(.03f, 1f);
            buttonTransform.anchorMax = new Vector2(0.97f, 1f);
            buttonTransform.anchoredPosition = new Vector2(0, (-0.55f - i) * buttonHeight);
            buttonTransform.sizeDelta = new Vector2(scroll.contentPanel.rect.width * 0.90f, buttonHeight);
            buttonTransform.tag = dynamicTag;

            Button buttonComponent = newButton.GetComponent<Button>();
            SlotButtonLink SB = new SlotButtonLink(s, buttonComponent);
            buttonComponent.onClick.AddListener(() => selectSlot(SB.slot));

            Part p = this.activeMech.parts[s];
            if (null != p)
            {
                buttonTransform.Find("Mech Name").GetComponent<Text>().text = p.displayName;

                //part display information here
            }
            else
            {
                foreach (Text t in buttonTransform.GetComponentsInChildren<Text>())
                {
                    t.text = "";
                }
            }

            buttonTransform.Find("Slot Overlay").GetComponentInChildren<Text>().text = s.ToString();
            i++;
        }

        if (Mathf.Abs(i * buttonHeight) > Mathf.Abs(scroll.viewport.rect.y))
        {
            scroll.contentPanel.sizeDelta = new Vector2(scroll.contentPanel.rect.x, Mathf.Abs((i + 0.55f) * buttonHeight) - Mathf.Abs(scroll.viewport.rect.y));
        }

        secondPanel.Find("Top Panel").gameObject.SetActive(true);

        secondScroll.scrollPanel.gameObject.SetActive(true);
    }

    private void ActivatePartsPanel(ScrollPanel scroll, Part.slot slot)
    {
        int i = 0;
        foreach (Part p in masterInventory.getParts(slot))
        {
            GameObject newButton = (GameObject)Instantiate(partButtonPrefab);
            newButton.transform.SetParent(scroll.contentPanel);
            newButton.transform.localScale = new Vector3(1, 1, 1);

            RectTransform buttonTransform = newButton.GetComponent<RectTransform>();
            buttonTransform.anchorMin = new Vector2(.03f, 1f);
            buttonTransform.anchorMax = new Vector2(0.97f, 1f);
            buttonTransform.anchoredPosition = new Vector2(0, (-0.55f - i) * buttonHeight);
            buttonTransform.sizeDelta = new Vector2(scroll.contentPanel.rect.width * 0.90f, buttonHeight);
            buttonTransform.tag = dynamicTag;

            Button buttonComponent = newButton.GetComponent<Button>();
            PartButtonLink PB = new PartButtonLink(p, buttonComponent);
            buttonComponent.onClick.AddListener(() => selectPart(PB.part));

            buttonTransform.Find("Part Name").GetComponent<Text>().text = p.displayName;
            if (null != p.owner)
            {
                buttonTransform.Find("Owner Panel").GetComponentInChildren<Text>().text = p.owner.displayName;
            }
            else
            {
                buttonTransform.Find("Owner Panel").gameObject.SetActive(false);
            }

            i++;
        }

        if (Mathf.Abs(i * buttonHeight) > Mathf.Abs(scroll.viewport.rect.y))
        {
            scroll.contentPanel.sizeDelta = new Vector2(scroll.contentPanel.rect.x, Mathf.Abs((i + 0.55f) * buttonHeight) - Mathf.Abs(scroll.viewport.rect.y));
        }

        thirdPanel.Find("Top Panel").gameObject.SetActive(true);

        thirdScroll.scrollPanel.gameObject.SetActive(true);
    }
    #endregion

    /// <summary>
    /// panel selection arbitration
    /// </summary>
    #region
    public void selectPanel(RectTransform selectedPanel)
    {
        Debug.Log("selected panel is " + selectedPanel);
        if (activePanel != null)
        {
            activePanel.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(minimizedPanelSize, 0);
            cleanPanel(activePanel);

            rootScroll.scrollPanel.gameObject.SetActive(false);
            secondScroll.scrollPanel.gameObject.SetActive(false);
            thirdScroll.scrollPanel.gameObject.SetActive(false);

            activePanel.Find("Top Panel").gameObject.SetActive(false);
        }

        if (null == activeMech || rootPanel == selectedPanel)
        {
            rootPanelSelect();
        }
        else if (Part.slot.Undefined == activeSlot || secondPanel == selectedPanel)
        {
            secondaryPanelSelect();
        }
        else if (thirdPanel == selectedPanel)
        {
            tertiaryPanelSelect();
        }
        else
        {
            throw new System.Exception("unknown panel selection case: selectedPanel = " + selectedPanel
                + " activeMech = " + activeMech + " activeSlot = " + activeSlot);
        }
    }

    private void rootPanelSelect()
    {
        if (activePanel != rootPanel)
        {
            activateMechPanel();
            this.activePanel = rootPanel;
        }
        else
        {
            this.activePanel = null;
        }
    }

    private void secondaryPanelSelect()
    {
        if (activePanel != secondPanel)
        {
            activateComponentsPanel(secondScroll, activeMech);
            this.activePanel = secondPanel;
            secondPanel.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(maximizedPanelSize, 0);
        }
        else
        {
            this.activePanel = null;
        }
    }

    private void tertiaryPanelSelect()
    {
        if (activePanel != thirdPanel)
        {
            ActivatePartsPanel(thirdScroll, activeSlot);
            this.activePanel = thirdPanel;
            thirdPanel.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(maximizedPanelSize, 0);
        }
        else
        {
            this.activePanel = null;
        }
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
    #endregion

    /// <summary>
    /// button bind functions
    /// </summary>
    #region
    public void testclick(System.Object o)
    {
        Debug.Log("button is clicked with object " + o);
    }

    public void selectMech(Mech m)
    {
        this.activeMech = m;
        selectPanel(secondPanel);
        secondEdgeButton.interactable = true;
    }

    public void selectSlot(Part.slot s)
    {
        this.activeSlot = s;
        selectPanel(thirdPanel);
        thirdEdgeButton.interactable = true;
    }

    public void selectPart(Part p)
    {
        if(null != p.owner)
        {
            p.owner.removePart(p);
        }
        this.activeMech.removePart(this.activeSlot);
        this.activeMech.addPartAs(p, this.activeSlot);
        selectPanel(secondPanel);
    }
    #endregion

    /// <summary>
    /// UI object initialization and linking
    /// </summary>
    #region
    class MechButtonLink
    {
        public Mech mech;
        public Button button;

        public MechButtonLink(Mech m, Button b)
        {
            mech = m;
            button = b;
        }
    }

    class SlotButtonLink
    {
        public Part.slot slot;
        public Button button;

        public SlotButtonLink(Part.slot s, Button b)
        {
            slot = s;
            button = b;
        }
    }

    class PartButtonLink
    {
        public Part part;
        public Button button;

        public PartButtonLink(Part p, Button b)
        {
            part = p;
            button = b;
        }
    }

    class ScrollPanel
    {
        public RectTransform scrollPanel;
        public RectTransform viewport;
        public RectTransform contentPanel;
        public List<Button> buttonList;

        public ScrollPanel(RectTransform basePanel)
        {
            RectTransform newInnerPanel = (RectTransform)basePanel.Find("Scroll Panel");

            scrollPanel = newInnerPanel.GetComponent<RectTransform>();
            scrollPanel.anchorMin = new Vector2(.03f, 0.03f);
            scrollPanel.anchorMax = new Vector2(.97f, .88f);
            scrollPanel.anchoredPosition = new Vector2(0, 0);
            scrollPanel.sizeDelta = new Vector2(0, 0);

            viewport = (RectTransform)scrollPanel.Find("Viewport");
            viewport.anchorMin = new Vector2(0, 0);
            viewport.anchorMax = new Vector2(1, 1);
            viewport.anchoredPosition = new Vector2(0, 0);

            contentPanel = (RectTransform)viewport.Find("Content");
            contentPanel.anchorMin = new Vector2(0, 0);
            contentPanel.anchorMax = new Vector2(1, 1);
            contentPanel.anchoredPosition = new Vector2(0, 0);

            buttonList = new List<Button>();
        }
    }
    #endregion

    public void loadScene(string levelName)
    {
        SceneLoader.sceneLoader.loadScene(levelName);
    }
}

