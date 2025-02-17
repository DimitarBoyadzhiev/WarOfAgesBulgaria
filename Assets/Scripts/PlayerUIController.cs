using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private BaseScript playerBase;
    private Button archerUpgradeButton;

    private void Awake()
    {
        archerUpgradeButton = transform.Find("ArcherButtonUpgrade").GetComponent<Button>();
    }

    public void DisableArcherUpgradeButton()
    {
        if(playerBase.archerLevel == 3)
        {
            archerUpgradeButton.interactable = false;
        }
    }
}
