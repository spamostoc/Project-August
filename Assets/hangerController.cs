using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class hangerController : MonoBehaviour {

    public Transform rootPanel;
    public Transform secondaryPanel;
    public Transform tertiaryPanel;

    private Transform activePanel;

    public GameObject buttonPrefab;
    public GameObject innerPanelPrefab;

    public int minimizedPanelSize = 50;
    public int maximizedPanelSize = 500;
    public float innerPanelXScale = 0.9f;
    public float innerPanelYScale = 0.9f;

    public string dynamicTag = "dynamicUI";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private enum panelFunc { first, second, third };

    public void rootPanelSelect()
    {
        if (activePanel != null)
        {
            activePanel.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(minimizedPanelSize, 0);
            cleanPanel(rootPanel);
        }
        if (activePanel != rootPanel)
        {
            rootPanel.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(maximizedPanelSize, 0);
            cleanPanel(activePanel);
            this.activePanel = rootPanel;
            spawnDynamicPanel(rootPanel, panelFunc.first);
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
            cleanPanel(secondaryPanel);
        }
        if (activePanel != secondaryPanel)
        {
            secondaryPanel.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(maximizedPanelSize, 0);
            cleanPanel(activePanel);
            this.activePanel = secondaryPanel;
            spawnDynamicPanel(secondaryPanel, panelFunc.second);
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
            cleanPanel(tertiaryPanel);
        }
        if (activePanel != tertiaryPanel)
        {
            tertiaryPanel.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(maximizedPanelSize, 0);
            cleanPanel(activePanel);
            this.activePanel = tertiaryPanel;
            spawnDynamicPanel(tertiaryPanel, panelFunc.second);
        }
        else
        {
            this.activePanel = null;
        }
    }

    public void scroll()
    {
       // activePanel
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
}
