using UnityEngine;
using System.Collections;

public class hangerController : MonoBehaviour {

    public Transform rootPanel;
    public Transform secondaryPanel;
    public Transform tertiaryPanel;

    public Transform activePanel;

    public int minimizedPanelSize = 50;
    public int maximizedPanelSize = 500;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void rootPanelSelect()
    {
        if (activePanel != null)
        {
            activePanel.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(minimizedPanelSize, 0);
        }
        if (activePanel != rootPanel)
        {
            rootPanel.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(maximizedPanelSize, 0);
            this.activePanel = rootPanel;
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
        }
        if (activePanel != secondaryPanel)
        {
            secondaryPanel.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(maximizedPanelSize, 0);
            this.activePanel = secondaryPanel;
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
        }
        if (activePanel != tertiaryPanel)
        {
            tertiaryPanel.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(maximizedPanelSize, 0);
            this.activePanel = tertiaryPanel;
        }
        else
        {
            this.activePanel = null;
        }
    }
}
