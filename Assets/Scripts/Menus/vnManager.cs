using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class vnManager : MonoBehaviour {

    public static vnManager vnDataManager;

    private int testVNNumber;
    private bool uiActive;
    public GameObject vnUI;
    public Text dialogueText;
    public Image sprite1;
    public Sprite ssprite;

    void Awake()
    {
        if (vnDataManager == null)
        {
            DontDestroyOnLoad(gameObject);
            vnDataManager = this;
        }
        else if (vnDataManager != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        this.testVNNumber = 1;
        this.uiActive = false;
        vnUI.SetActive(this.uiActive);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.uiActive = !this.uiActive;
            vnUI.SetActive(this.uiActive);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            dialogueText.text = "this is some new text";
            sprite1.sprite = ssprite;
        }
    }

    public void setTestNumber(int num)
    {
        this.testVNNumber = num;
    }

    public int getTestNumber()
    {
        return this.testVNNumber;
    }
}
