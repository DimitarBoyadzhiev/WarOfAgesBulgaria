using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private BaseScript playerBase;
    public Button archerButton;
    public Button backButton;
    private Button archerUpgradeButton;
    public Button archerUpgradeMAX;
    private TextMeshProUGUI goldText;
    private int gold;
    public GoldBar soldierGoldBar;
    public GoldBar archerGoldBar;
    public GoldBar archerUpgradeGoldBar;


    private void Awake()
    {
        goldText = transform.Find("GoldText").GetComponent<TextMeshProUGUI>();
        soldierGoldBar = transform.Find("SoldierButton").Find("goldSlider").GetComponent<GoldBar>();
        archerGoldBar = transform.Find("ArcherButton").Find("goldSliderArcher").GetComponent<GoldBar>();
        archerUpgradeButton = transform.Find("ArcherButtonUpgrade").GetComponent<Button>();
        archerUpgradeGoldBar = transform.Find("ArcherButtonUpgrade").Find("goldSliderArcher").GetComponent<GoldBar>();
    }

    private void Update()
    {
        gold = GameManager.instance.gold;
        goldText.text = "Gold: " + gold;
        archerGoldBar.SetGold(gold);
        soldierGoldBar.SetGold(gold);
        archerUpgradeGoldBar.SetGold(gold);
    }

    public void DisableArcherBuyButton()
    {
        archerButton.gameObject.SetActive(false);
        archerUpgradeButton.gameObject.SetActive(true);
    }

    public void DisableArcherUpgradeButton()
    {
        if (playerBase.archerLevel == 3)
        {
            archerUpgradeButton.interactable = false;
            archerUpgradeButton.gameObject.SetActive(false);
            archerUpgradeMAX.gameObject.SetActive(true);
            archerUpgradeMAX.interactable = false;
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
        ScoreManager.Instance.ResetScore();
        GameManager.instance.DestroyGameManager();
    }
}
