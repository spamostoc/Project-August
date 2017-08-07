using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class hangerController : MonoBehaviour {


    private enum panelFunc { first, second, third };

    public RectTransform rootPanel;
    public RectTransform secondPanel;
    public RectTransform thirdPanel;

    private ScrollPanel rootScroll;
    private ScrollPanel secondScroll;
    private ScrollPanel thirdScroll;

    private Transform activePanel;

    public GameObject buttonPrefab;
    public GameObject innerPanelPrefab;

    private Mech activeMech;
    private Part.slot activeSlot;

    public static int minimizedPanelSize = 50;
    public static int maximizedPanelSize = 500;
    public static float innerPanelXScale = 0.8f;
    public static float innerPanelYScale = 0.8f;
    public static float buttonHeight = 50;

    public string dynamicTag = "dynamicUI";
    
	// Use this for initialization
	void Start () {
        rootScroll = new ScrollPanel(rootPanel);
        rootScroll.scrollPanel.gameObject.SetActive(false);
        secondScroll = new ScrollPanel(secondPanel);
        secondScroll.scrollPanel.gameObject.SetActive(false);
        thirdScroll = new ScrollPanel(thirdPanel);
        thirdScroll.scrollPanel.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //panel population

    private void spawnMechPanelButton(ScrollPanel scroll)
    {
        int i = 0;
        foreach (Mech m in pManager.pDataManager.playerMechs.Values)
        {
            GameObject newButton = (GameObject)Instantiate(buttonPrefab);
            newButton.transform.SetParent(scroll.contentPanel);
            newButton.transform.localScale = new Vector3(1, 1, 1);

            RectTransform buttonTransform = newButton.GetComponent<RectTransform>();
            buttonTransform.anchorMin = new Vector2(0.48f, 1f);
            buttonTransform.anchorMax = new Vector2(0.48f, 1f);
            buttonTransform.anchoredPosition = new Vector2(0, (-0.55f - i) * buttonHeight);
            buttonTransform.sizeDelta = new Vector2(scroll.contentPanel.rect.width * 0.90f, buttonHeight);
            buttonTransform.tag = dynamicTag;

            Button buttonComponent = newButton.GetComponent<Button>();
            MechButton MB = new MechButton(m, buttonComponent);
            buttonComponent.onClick.AddListener(() => selectMech(MB.mech));
            buttonComponent.GetComponentInChildren<Text>().text = m.displayName;

            i++;
        }

        if (Mathf.Abs(i * buttonHeight) > Mathf.Abs(scroll.viewport.rect.y))
        {
            Debug.Log(i * buttonHeight);
            Debug.Log(scroll.viewport.rect.y);
            Debug.Log(Mathf.Abs(scroll.viewport.rect.y) - Mathf.Abs(i * buttonHeight));
            scroll.contentPanel.sizeDelta = new Vector2(scroll.contentPanel.rect.x,  Mathf.Abs((i + 0.55f) * buttonHeight) - Mathf.Abs(scroll.viewport.rect.y));
        }
    }

    private void spawnComponentsPanelButton(ScrollPanel scroll, Mech mech)
    {
        int i = 0;
        foreach (Part.slot s in mech.parts.Keys)
        {
            GameObject newButton = (GameObject)Instantiate(buttonPrefab);
            newButton.transform.SetParent(scroll.contentPanel);
            newButton.transform.localScale = new Vector3(1, 1, 1);

            RectTransform buttonTransform = newButton.GetComponent<RectTransform>();
            buttonTransform.anchorMin = new Vector2(0.48f, 1f);
            buttonTransform.anchorMax = new Vector2(0.48f, 1f);
            buttonTransform.anchoredPosition = new Vector2(0, (-0.55f - i) * buttonHeight);
            buttonTransform.sizeDelta = new Vector2(scroll.contentPanel.rect.width * 0.90f, buttonHeight);
            buttonTransform.tag = dynamicTag;

            Button buttonComponent = newButton.GetComponent<Button>();
            SlotButton SB = new SlotButton(s, buttonComponent);
            buttonComponent.onClick.AddListener(() => selectSlot(SB.slot));
            buttonComponent.GetComponentInChildren<Text>().text = s.ToString();

            i++;
        }

        if (Mathf.Abs(i * buttonHeight) > Mathf.Abs(scroll.viewport.rect.y))
        {
            Debug.Log(i * buttonHeight);
            Debug.Log(scroll.viewport.rect.y);
            Debug.Log(Mathf.Abs(scroll.viewport.rect.y) - Mathf.Abs(i * buttonHeight));
            scroll.contentPanel.sizeDelta = new Vector2(scroll.contentPanel.rect.x, Mathf.Abs((i + 0.55f) * buttonHeight) - Mathf.Abs(scroll.viewport.rect.y));
        }
    }

    private void spawnPartsPanelButton(ScrollPanel scroll, Part.slot slot)
    {
        int i = 0;
        foreach (Part p in masterInventory.getParts(slot))
        {
            GameObject newButton = (GameObject)Instantiate(buttonPrefab);
            newButton.transform.SetParent(scroll.contentPanel);
            newButton.transform.localScale = new Vector3(1, 1, 1);

            RectTransform buttonTransform = newButton.GetComponent<RectTransform>();
            buttonTransform.anchorMin = new Vector2(0.48f, 1f);
            buttonTransform.anchorMax = new Vector2(0.48f, 1f);
            buttonTransform.anchoredPosition = new Vector2(0, (-0.55f - i) * buttonHeight);
            buttonTransform.sizeDelta = new Vector2(scroll.contentPanel.rect.width * 0.90f, buttonHeight);
            buttonTransform.tag = dynamicTag;

            Button buttonComponent = newButton.GetComponent<Button>();
            PartButton PB = new PartButton(p, buttonComponent);
            buttonComponent.onClick.AddListener(() => selectPart(PB.part));
            buttonComponent.GetComponentInChildren<Text>().text = p.displayName;

            i++;
        }

        if (Mathf.Abs(i * buttonHeight) > Mathf.Abs(scroll.viewport.rect.y))
        {
            Debug.Log(i * buttonHeight);
            Debug.Log(scroll.viewport.rect.y);
            Debug.Log(Mathf.Abs(scroll.viewport.rect.y) - Mathf.Abs(i * buttonHeight));
            scroll.contentPanel.sizeDelta = new Vector2(scroll.contentPanel.rect.x, Mathf.Abs((i + 0.55f) * buttonHeight) - Mathf.Abs(scroll.viewport.rect.y));
        }
    }

    //panel switching

    public void selectPanel(RectTransform selectedPanel)
    {
        Debug.Log("selected panel is " + selectedPanel);
        if (activePanel != null)
        {
            activePanel.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(minimizedPanelSize, 0);
            cleanPanel(activePanel);
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
            spawnMechPanelButton(rootScroll);
            this.activePanel = rootPanel;
            rootPanel.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(maximizedPanelSize, 0);

            rootScroll.scrollPanel.gameObject.SetActive(true);
            secondScroll.scrollPanel.gameObject.SetActive(false);
            thirdScroll.scrollPanel.gameObject.SetActive(false);
        }
        else
        {
                rootScroll.scrollPanel.gameObject.SetActive(false);
                this.activePanel = null;
        }
    }

    private void secondaryPanelSelect()
    {
        if (activePanel != secondPanel)
        {
            spawnComponentsPanelButton(secondScroll, activeMech);
            this.activePanel = secondPanel;
            secondPanel.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(maximizedPanelSize, 0);

            rootScroll.scrollPanel.gameObject.SetActive(false);
            secondScroll.scrollPanel.gameObject.SetActive(true);
            thirdScroll.scrollPanel.gameObject.SetActive(false);
        }
        else
        {
                secondScroll.scrollPanel.gameObject.SetActive(false);
                this.activePanel = null;
        }
    }

    private void tertiaryPanelSelect()
    {
        if (activePanel != thirdPanel)
        {
            spawnPartsPanelButton(thirdScroll, activeSlot);
            this.activePanel = thirdPanel;
            thirdPanel.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(maximizedPanelSize, 0);

            rootScroll.scrollPanel.gameObject.SetActive(false);
            secondScroll.scrollPanel.gameObject.SetActive(false);
            thirdScroll.scrollPanel.gameObject.SetActive(true);
        }
        else
        {
                thirdScroll.scrollPanel.gameObject.SetActive(false);
                this.activePanel = null;
        }
    }

    private void cleanPanel(Transform activeP)
    {
        if (activeP == null)
            return;
        /*foreach (Button b in activeP.GetComponentsInChildren<Button>())
        {
            if (b.tag.Equals(dynamicTag))
                GameObject.DestroyObject(b.gameObject);
        }*/
        foreach (RectTransform r in activeP.GetComponentsInChildren<RectTransform>())
        {
            if (r.tag.Equals(dynamicTag))
                GameObject.DestroyObject(r.gameObject);
        }
    }





    public void testclick(System.Object o)
    {
        Debug.Log("button is clicked with object " + o);
    }

    public void selectMech(Mech m)
    {
        this.activeMech = m;
        selectPanel(secondPanel);
    }

    public void selectSlot(Part.slot s)
    {
        this.activeSlot = s;
        selectPanel(thirdPanel);
    }

    public void selectPart(Part p)
    {
        if(null != p.owner)
        {
            p.owner.removePart(p);
        }
        this.activeMech.removePart(this.activeSlot);
        this.activeMech.addPartAs(p, this.activeSlot);
    }

    class MechButton
    {
        public Mech mech;
        public Button button;

        public MechButton(Mech m, Button b)
        {
            mech = m;
            button = b;
        }
    }

    class SlotButton
    {
        public Part.slot slot;
        public Button button;

        public SlotButton(Part.slot s, Button b)
        {
            slot = s;
            button = b;
        }
    }

    class PartButton
    {
        public Part part;
        public Button button;

        public PartButton(Part p, Button b)
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
            scrollPanel.anchorMin = new Vector2(0.45f, 0.5f);
            scrollPanel.anchorMax = new Vector2(0.45f, 0.5f);
            scrollPanel.anchoredPosition = new Vector2(0, 0);
            scrollPanel.sizeDelta = new Vector2(maximizedPanelSize * innerPanelXScale, basePanel.rect.height * innerPanelYScale);

            viewport = (RectTransform)scrollPanel.Find("Viewport");
            viewport.anchorMin = new Vector2(0, 0);
            viewport.anchorMax = new Vector2(1, 1);
            viewport.anchoredPosition = new Vector2(0, 0);

            contentPanel = (RectTransform)viewport.Find("Content");
            contentPanel.anchorMin = new Vector2(0, 0);
            contentPanel.anchorMax = new Vector2(1, 1);
            contentPanel.anchoredPosition = new Vector2(0, 0);

            buttonList = new ArrayList<Button>();
        }
    }
}

