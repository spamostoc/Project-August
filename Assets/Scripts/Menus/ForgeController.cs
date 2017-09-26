using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ForgeController : MonoBehaviour {


    public RectTransform managementPanel;
    private int lineIndex;
    private int stageIndex;

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
    }

    public void selectLine(RectTransform line)
    {      
        line.sizeDelta = new Vector2(0, 400);
        this.managementPanel.SetParent(line);
        this.managementPanel.anchoredPosition = new Vector2(0, 0);
        this.managementPanel.gameObject.SetActive(true);

        if (stageIndex == 0)
        {
            populateOptions(this.managementPanel.Find("optionDropdown1").GetComponent<Dropdown>(), null);
        }
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
        drop.ClearOptions();

        List<Dropdown.OptionData> optList = new List<Dropdown.OptionData>();
        
        foreach (CraftingComponent c in options)
        {
            optList.Add(new Dropdown.OptionData(c.getName()));
        }

        drop.AddOptions(optList);
    }
}
