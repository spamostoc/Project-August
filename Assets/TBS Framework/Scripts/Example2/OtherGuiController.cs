using System;
using UnityEngine;
using UnityEngine.UI;

class OtherGuiController : MonoBehaviour
{
    public CellGrid CellGrid;
    public Transform UnitsParent;
    public Image FullMarkerImage;
    public Image EmptyMarkerImage;
    public Image FullHPBar;
    public Image EmptyHPBar;
    public Button NextTurnButton;

    public Text SelectedInfoText;
    public Text SelectedHPText;
    public Text SelectedShieldText;
    public Text DefenceText;
    public Text RangeText;

    public Text HoverInfoText;
    public Text HoverHPText;
    public Text HoverShieldText;
    public Image HoverPanel;

    public Sprite emptySprite;
    public Button AbilityButton;
    public Image AbilityIcon;
    public Transform AbilityPanel;

    private Unit selectedUnit;
    public Text weaponText;
    public Text weaponSubText1;
    public Text weaponSubText2;
    public Text weaponSubText3;

    public int scrollBuffer;
    public int scrollSpeed;
    public Transform mainCamera;

    private void Start()
    {
        CellGrid.GameStarted += OnGameStarted;
        CellGrid.TurnEnded += OnTurnEnded;
        CellGrid.GameEnded += OnGameEnded;
    }

    private void Update()
    {
        if(Input.GetMouseButton(2))
        {

        }

        if(Input.mousePosition.x < scrollBuffer)
        {
            mainCamera.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
        }
        else if(Input.mousePosition.x > Screen.width - scrollBuffer)
        {
            mainCamera.Translate(Vector3.right * scrollSpeed * Time.deltaTime);
        }

        if (Input.mousePosition.y < scrollBuffer)
        {
            mainCamera.Translate(Vector3.down * scrollSpeed * Time.deltaTime);
        }
        else if (Input.mousePosition.y > Screen.height - scrollBuffer)
        {
            mainCamera.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        }
    }

    private void OnUnitAttacked(object sender, AttackEventArgs e)
    {
        if (!(CellGrid.CurrentPlayer is HumanPlayer)) return;

        OnUnitDehighlighted(sender, e);

        if ((sender as Unit).dynamicAttributes.health <= 0) return;

        OnUnitHighlighted(sender, e);
    }
    private void OnGameStarted(object sender, EventArgs e)
    {
        HoverPanel.enabled = false;
        HoverInfoText.enabled = false;
        HoverShieldText.enabled = false;
        HoverHPText.enabled = false;

        foreach (Button b in this.AbilityPanel.GetComponentsInChildren<Button>())
        {
            b.GetComponentInChildren<Image>().sprite = null;
            b.enabled = false;
            b.interactable = false;
        }

        foreach (Transform unit in UnitsParent)
        {
            unit.GetComponent<Unit>().UnitHighlighted += OnUnitHighlighted;
            unit.GetComponent<Unit>().UnitDehighlighted += OnUnitDehighlighted;
            unit.GetComponent<Unit>().UnitAttacked += OnUnitAttacked;
            unit.GetComponent<Unit>().UnitSelected += OnUnitSelected;
            unit.GetComponent<Unit>().UnitDeselected += OnUnitDeselected;
        }
        SelectedInfoText.text = "Player " + (CellGrid.CurrentPlayerNumber + 1);

        foreach (Button b in this.AbilityPanel.GetComponentsInChildren<Button>())
        {
            b.GetComponentInChildren<Image>().sprite = null;
            b.interactable = false;
        }

        OnTurnEnded(sender, e);
    }
    private void OnGameEnded(object sender, EventArgs e)
    {
        SelectedInfoText.text = "Player " + ((sender as CellGrid).CurrentPlayerNumber + 1) + " wins!";
    }
    private void OnTurnEnded(object sender, EventArgs e)
    {
        NextTurnButton.interactable = ((sender as CellGrid).CurrentPlayer is HumanPlayer);
        SelectedInfoText.text = "Player " + ((sender as CellGrid).CurrentPlayerNumber + 1);
    }

    private void OnUnitDehighlighted(object sender, EventArgs e)
    {
        HoverPanel.enabled = false;
        HoverInfoText.enabled = false;
        HoverShieldText.enabled = false;
        HoverHPText.enabled = false;

        foreach (Transform shieldBar in HoverShieldText.transform)
        {
            Destroy(shieldBar.gameObject);
        }

        foreach (Transform hpBar in HoverHPText.transform)
        {
            Destroy(hpBar.gameObject);
        }

    }

    private void OnUnitHighlighted(object sender, EventArgs e)
    {
        if((sender as Unit) == selectedUnit)
        {
            return;
        }

        HoverInfoText.text = "Player " + ((sender as Unit).PlayerNumber + 1);

        float hpScale = (sender as Unit).dynamicAttributes.health / (sender as Unit).dynamicAttributes.maxHealth;

        Image fullHpBar = Instantiate(FullHPBar);
        Image emptyHpBar = Instantiate(EmptyHPBar);
        fullHpBar.rectTransform.localScale = new Vector3(hpScale, 1, 1);
        emptyHpBar.rectTransform.SetParent(HoverHPText.rectTransform, false);
        fullHpBar.rectTransform.SetParent(HoverHPText.rectTransform, false);

        float shieldScale = (sender as Unit).dynamicAttributes.shieldPoints / (sender as Unit).dynamicAttributes.maxShieldPoints;

        Image fullShieldBar = Instantiate(FullHPBar);
        Image emptyShieldBar = Instantiate(EmptyHPBar);
        fullShieldBar.rectTransform.localScale = new Vector3(shieldScale, 1, 1);
        emptyShieldBar.rectTransform.SetParent(HoverShieldText.rectTransform, false);
        fullShieldBar.rectTransform.SetParent(HoverShieldText.rectTransform, false);


        HoverPanel.enabled = true;
        HoverInfoText.enabled = true;
        HoverShieldText.enabled = true;
        HoverHPText.enabled = true;
    }

    public void OnUnitSelected(object sender, EventArgs e)
    {
        selectedUnit = (sender as Unit);
        weaponText.text = (sender as Mech).activeWeapon.displayName;
        weaponSubText1.text = "Damage: " + (sender as Mech).activeWeapon.damage;
        weaponSubText2.text = "Range: " + (sender as Mech).activeWeapon.range;
        weaponSubText3.text = "Ammo: " + (sender as Mech).activeWeapon.currentAmmo + "/" + (sender as Mech).activeWeapon.maxAmmo;

        float hpScale = (sender as Unit).dynamicAttributes.health / (sender as Unit).dynamicAttributes.maxHealth;

        Image fullHpBar = Instantiate(FullHPBar);
        Image emptyHpBar = Instantiate(EmptyHPBar);
        fullHpBar.rectTransform.localScale = new Vector3(hpScale, 1, 1);
        emptyHpBar.rectTransform.SetParent(SelectedHPText.rectTransform, false);
        fullHpBar.rectTransform.SetParent(SelectedHPText.rectTransform, false);

        float shieldScale = (sender as Unit).dynamicAttributes.shieldPoints / (sender as Unit).dynamicAttributes.maxShieldPoints;

        Image fullShieldBar = Instantiate(FullHPBar);
        Image emptyShieldBar = Instantiate(EmptyHPBar);
        fullShieldBar.rectTransform.localScale = new Vector3(shieldScale, 1, 1);
        emptyShieldBar.rectTransform.SetParent(SelectedShieldText.rectTransform, false);
        fullShieldBar.rectTransform.SetParent(SelectedShieldText.rectTransform, false);
        
        if ((sender as Unit).abilities != null && (sender as Unit).abilities.Count > 0)
        {

            Button[] buttons = this.AbilityPanel.GetComponentsInChildren<Button>();

            for (int n = 0; n < Math.Min(buttons.Length, (sender as Unit).abilities.Count); n++)
            {
                //have to make sure we're not finding the image in the button itself
                foreach (Image i in buttons[n].GetComponentsInChildren<Image>())
                {
                    if (i.gameObject.transform.parent == buttons[n].transform)
                    {
                        i.sprite = (sender as Unit).abilities[n].iconSprite;
                    }
                }
                buttons[n].enabled = true;
                buttons[n].interactable = true;
            }
        }
    }

    public void OnUnitDeselected(object sender, EventArgs e)
    {
        selectedUnit = null;
        weaponText.text = null;

        foreach (Transform shieldBar in SelectedShieldText.transform)
        {
            Destroy(shieldBar.gameObject);
        }

        foreach (Transform marker in DefenceText.transform)
        {
            Destroy(marker.gameObject);
        }

        foreach (Transform marker in RangeText.transform)
        {
            Destroy(marker.gameObject);
        }
 
        foreach (Transform hpBar in SelectedHPText.transform)
        {
            Destroy(hpBar.gameObject);
        }

        foreach (Button b in this.AbilityPanel.GetComponentsInChildren<Button>())
        {
            b.GetComponentInChildren<Image>().sprite = null;
            b.enabled = false;
            b.interactable = false;
        }
    }

    public void RestartLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void OnAbilityButton(int index)
    {
        CellGrid.ActivateAbility(index);
    }

    public void OnEndTurnButton()
    {
        CellGrid.EndTurn();
    }

    public void loadScene(string levelName)
    {
        SceneLoader.sceneLoader.loadScene(levelName);
    }
}

