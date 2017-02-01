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

    public Text InfoText;
    public Text HPText;
    public Text AttackText;
    public Text DefenceText;
    public Text RangeText;


    public Sprite emptySprite;
    public Button AbilityButton;
    public Image AbilityIcon;
    public Transform AbilityPanel;

    private void Start()
    {
        CellGrid.GameStarted += OnGameStarted;
        CellGrid.TurnEnded += OnTurnEnded;
        CellGrid.GameEnded += OnGameEnded;      
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
        foreach (Transform unit in UnitsParent)
        {
            unit.GetComponent<Unit>().UnitHighlighted += OnUnitHighlighted;
            unit.GetComponent<Unit>().UnitDehighlighted += OnUnitDehighlighted;
            unit.GetComponent<Unit>().UnitAttacked += OnUnitAttacked;
            unit.GetComponent<Unit>().UnitSelected += OnUnitSelected;
            unit.GetComponent<Unit>().UnitDeselected += OnUnitDeselected;
        }
        InfoText.text = "Player " + (CellGrid.CurrentPlayerNumber + 1);

        foreach(Button b in this.AbilityPanel.GetComponentsInChildren<Button>())
        {
            b.GetComponentInChildren<Image>().sprite = null;
            b.interactable = false;
        }

        OnTurnEnded(sender,e);
    }
    private void OnGameEnded(object sender, EventArgs e)
    {
        InfoText.text = "Player " + ((sender as CellGrid).CurrentPlayerNumber + 1) + " wins!";
    }
    private void OnTurnEnded(object sender, EventArgs e)
    {
        NextTurnButton.interactable = ((sender as CellGrid).CurrentPlayer is HumanPlayer);
        InfoText.text = "Player " + ((sender as CellGrid).CurrentPlayerNumber + 1);
    }

    private void OnUnitDehighlighted(object sender, EventArgs e)
    {
        foreach (Transform marker in AttackText.transform)
        {
            Destroy(marker.gameObject);
        }

        foreach (Transform marker in DefenceText.transform)
        {
            Destroy(marker.gameObject);
        }

        foreach (Transform marker in RangeText.transform)
        {
            Destroy(marker.gameObject);
        }

        foreach (Transform hpBar in HPText.transform)
        {
            Destroy(hpBar.gameObject);
        }

    }
    private void OnUnitHighlighted(object sender, EventArgs e)
    {
        var defence = 0;
        //var range = (sender as Unit).AttackRange;

        float hpScale = (sender as Unit).dynamicAttributes.health / (sender as Unit).dynamicAttributes.maxHealth;

        Image fullHpBar = Instantiate(FullHPBar);
        Image emptyHpBar = Instantiate(EmptyHPBar);
        fullHpBar.rectTransform.localScale = new Vector3(hpScale, 1, 1);
        emptyHpBar.rectTransform.SetParent(HPText.rectTransform,false);
        fullHpBar.rectTransform.SetParent(HPText.rectTransform, false);
        
        for (int i = 0; i < 7; i++)
        {
            /*Image AttackMarker;
            AttackMarker = Instantiate(i<attack ? FullMarkerImage : EmptyMarkerImage);

                AttackMarker.rectTransform.SetParent(AttackText.rectTransform,false);
                AttackMarker.rectTransform.anchorMin = new Vector2(i * 0.14f,0.1f);
                AttackMarker.rectTransform.anchorMax = new Vector2((i * 0.14f)+0.13f, 0.6f);*/

            Image DefenceMarker;
            DefenceMarker = Instantiate(i < defence ? FullMarkerImage : EmptyMarkerImage);

                DefenceMarker.rectTransform.SetParent(DefenceText.rectTransform, false);
                DefenceMarker.rectTransform.anchorMin = new Vector2(i * 0.14f, 0.1f);
                DefenceMarker.rectTransform.anchorMax = new Vector2((i * 0.14f) + 0.13f, 0.6f);

           /* Image RangeMarker;
            RangeMarker = Instantiate(i < range ? FullMarkerImage : EmptyMarkerImage);

                RangeMarker.rectTransform.SetParent(RangeText.rectTransform, false);
                RangeMarker.rectTransform.anchorMin = new Vector2(i * 0.14f, 0.1f);
                RangeMarker.rectTransform.anchorMax = new Vector2((i * 0.14f) + 0.13f, 0.6f); */            
        }
    }

    public void OnUnitSelected(object sender, EventArgs e)
    {
        if ((sender as Unit).abilities != null && (sender as Unit).abilities.Count > 0)
        {
            Debug.Log("loading ability icon");

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
                buttons[n].interactable = true;
            }
        }
    }

    public void OnUnitDeselected(object sender, EventArgs e)
    {
        foreach (Button b in this.AbilityPanel.GetComponentsInChildren<Button>())
        {
            b.GetComponentInChildren<Image>().sprite = null;
            b.interactable = false;
        }
    }

    public void RestartLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}

