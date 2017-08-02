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

        spawnMechPanelButton(rootScroll);
        spawnComponentsPanelButton(secondScroll, pManager.pDataManager.playerMechs.Values.ToList<Mech>()[0]);
        spawnPartsPanelButton(thirdScroll, Part.slot.weapon1);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

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

            Button buttonComponent = newButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => testclick());
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

            Button buttonComponent = newButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => testclick());
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

            Button buttonComponent = newButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => testclick());
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


    public void rootPanelSelect()
    {
        if (activePanel != null)
        {
            activePanel.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(minimizedPanelSize, 0);
            rootScroll.scrollPanel.gameObject.SetActive(false);
        }
        if (activePanel != rootPanel)
        {
            rootPanel.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(maximizedPanelSize, 0);
            this.activePanel = rootPanel;
            rootScroll.scrollPanel.gameObject.SetActive(true);
            secondScroll.scrollPanel.gameObject.SetActive(false);
            thirdScroll.scrollPanel.gameObject.SetActive(false);
        }
        else
        {
            this.activePanel = null;
        }
    }

    public void secondaryPanelSelect()
    {
        if (activePanel != null)
        {
            activePanel.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(minimizedPanelSize, 0);
            secondScroll.scrollPanel.gameObject.SetActive(false);
        }
        if (activePanel != secondPanel)
        {
            secondPanel.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(maximizedPanelSize, 0);
            this.activePanel = secondPanel;
            rootScroll.scrollPanel.gameObject.SetActive(false);
            secondScroll.scrollPanel.gameObject.SetActive(true);
            thirdScroll.scrollPanel.gameObject.SetActive(false);
        }
        else
        {
            this.activePanel = null;
        }
    }

    public void tertiaryPanelSelect()
    {
        if (activePanel != null)
        {
            activePanel.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(minimizedPanelSize, 0);
            thirdScroll.scrollPanel.gameObject.SetActive(false);
        }
        if (activePanel != thirdPanel)
        {
            thirdPanel.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(maximizedPanelSize, 0);
            this.activePanel = thirdPanel;
            rootScroll.scrollPanel.gameObject.SetActive(false);
            secondScroll.scrollPanel.gameObject.SetActive(false);
            thirdScroll.scrollPanel.gameObject.SetActive(true);
        }
        else
        {
            this.activePanel = null;
        }
    }

    private void spawnDynamicPanel(Transform activeP, panelFunc func)
    {
        GameObject newInnerPanel = (GameObject)Instantiate(innerPanelPrefab);
        newInnerPanel.transform.SetParent(activeP);
        newInnerPanel.transform.localScale = new Vector3(innerPanelXScale, innerPanelYScale, 1);

        RectTransform innerPanelRect = newInnerPanel.GetComponent<RectTransform>();
        innerPanelRect.anchorMin = new Vector2(0.5f, 0.5f);
        innerPanelRect.anchorMax = new Vector2(0.5f, 0.5f);
        innerPanelRect.anchoredPosition = new Vector2(0, 0);
        innerPanelRect.sizeDelta = new Vector2(maximizedPanelSize * innerPanelXScale, 500);
        innerPanelRect.tag = dynamicTag;

        RectTransform viewport = (RectTransform) innerPanelRect.Find("Viewport");
        viewport.anchorMin = new Vector2(0, 0);
        viewport.anchorMax = new Vector2(1, 1);
        viewport.anchoredPosition = new Vector2(0, 0);

        RectTransform contentPanel = (RectTransform) viewport.Find("Content");
        contentPanel.anchorMin = new Vector2(0, 0);
        contentPanel.anchorMax = new Vector2(1, 1);
        contentPanel.anchoredPosition = new Vector2(0, 0);

        GameObject newButton = (GameObject)Instantiate(buttonPrefab);
        newButton.transform.SetParent(contentPanel);
        newButton.transform.localScale = new Vector3(1, 1, 1);

        RectTransform buttonTransform = newButton.GetComponent<RectTransform>();
        buttonTransform.anchorMin = new Vector2(0.45f, 0.5f);
        buttonTransform.anchorMax = new Vector2(0.45f, 0.5f);
        buttonTransform.anchoredPosition = new Vector2(0, 0);

        Button buttonComponent = newButton.GetComponent<Button>();
        buttonComponent.onClick.AddListener(() => testclick());
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

    public void testclick()
    {
        Debug.Log("button is clicked");
    }

    class ScrollPanel
    {
        public string dynamicTag = "dynamicUI";

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
            scrollPanel.tag = dynamicTag;

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

