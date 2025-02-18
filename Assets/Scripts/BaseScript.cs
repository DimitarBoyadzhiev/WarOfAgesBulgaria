using UnityEngine;

public class BaseScript : MonoBehaviour
{
    [SerializeField]
    private GameObject soldier;
    [SerializeField]
    private GameObject archer;
    [SerializeField]
    private GameObject archerSpawnPoint;

    public PlayerUIController controllerUI;

    public bool isSpawnedArcher;
    public int archerLevel;

    public GoldBar soldierGoldBar;
    public GoldBar archerGoldBar;
    public GoldBar archerUgradeGoldBar;


    [SerializeField] private int maxHP = 250;
    [SerializeField] private int currentHP;

    public float cooldown;
    float lastSpawn;

    public HealthBar healthBar;

    public int soldierGoldCost = 5;
    public int archerGoldCost = 15;

    private void Awake()
    {
        currentHP = maxHP;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar.SetMaxHealth(maxHP);
        soldierGoldBar.SetMaxGold(soldierGoldCost);
        archerGoldBar.SetMaxGold(archerGoldCost);
        archerUgradeGoldBar.SetMaxGold(archerGoldCost);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP <= 0)
        {
            Die();
        }        
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        healthBar.SetHealth(currentHP);
    }

    private void Die()
    {
        GameManager.instance.EndGame();
    }

    public void SpawnSolider()
    {
        if(GameManager.instance.gold >= soldierGoldCost)
        {
            if(Time.time - lastSpawn < cooldown)
            {
                return;
            }
            lastSpawn = Time.time;
            Instantiate(soldier);
            GameManager.instance.gold -= soldierGoldCost;
        }
    }

    public void SpawnArcher()
    {
        if (GameManager.instance.gold >= archerGoldCost)
        {
            if (!isSpawnedArcher)
            {
                isSpawnedArcher = true;
                Instantiate(archer);
                archer.transform.position = archerSpawnPoint.transform.position;
                archerLevel = 1;
                GameManager.instance.gold -= archerGoldCost;
                controllerUI.DisableArcherBuyButton();
            }
        }
    }

    public void UpgradeArcher()
    {
        if (GameManager.instance.gold >= archerGoldCost)
        {
            archerLevel++;
            ArcherAI.instance.LevelUp();
            GameManager.instance.gold -= archerGoldCost;
        }
    }
}
