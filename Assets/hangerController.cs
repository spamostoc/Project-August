using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class hangerController : MonoBehaviour {

    public Transform rootPanel;
    public Transform secondaryPanel;
    public Transform tertiaryPanel;

    public Transform activePanel;

    public GameObject buttonPrefab;

    public int minimizedPanelSize = 50;
    public int maximizedPanelSize = 500;

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
            spawnPanelButtons(rootPanel, panelFunc.first);
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
            spawnPanelButtons(secondaryPanel, panelFunc.second);
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
            spawnPanelButtons(tertiaryPanel, panelFunc.second);
        }
        else
        {
            this.activePanel = null;
        }
    }

    private void spawnPanelButtons(Transform activeP, panelFunc func)
    {
        GameObject newButton = (GameObject)Instantiate(buttonPrefab);
        newButton.transform.SetParent(activeP);
        newButton.transform.localScale = new Vector3(1, 1, 1);
        newButton.transform.localPosition = new Vector3(maximizedPanelSize / 2, 0, 0);

        Button buttonComponent = newButton.GetComponent<Button>();
        buttonComponent.tag = dynamicTag;
        buttonComponent.onClick.AddListener(() => testclick());
    }

    private void cleanPanel(Transform activeP)
    {
        if (activeP == null)
            return;
        foreach (Button b in activeP.GetComponentsInChildren<Button>())
        {
            if (b.tag.Equals(dynamicTag))
                GameObject.DestroyObject(b.gameObject);
        }
    }

    public void testclick()
    {
        Debug.Log("button is clicked");
    }
}
