using UnityEngine;

public class BaseScript : MonoBehaviour
{
    [SerializeField]
    private GameObject soldier;
    [SerializeField]
    private GameObject archer;
    [SerializeField]
    private GameObject archerSpawnPoint;
    public bool isSpawnedArcher;
    public int archerLevel;


    [SerializeField] private int maxHP = 250;
    [SerializeField] private int currentHP;

    public float cooldown;
    float lastSpawn;

    public HealthBar healthBar;

    private void Awake()
    {
        currentHP = maxHP;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar.SetMaxHealth(maxHP);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP <= 0)
        {
            Die();
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    TakeDamage(100);
        //}
        
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
        if(Time.time - lastSpawn < cooldown)
        {
            return;
        }
        lastSpawn = Time.time;
        Instantiate(soldier);
    }

    public void SpawnArcher()
    {
        if (!isSpawnedArcher)
        {
            isSpawnedArcher = true;
            Instantiate(archer);
            archer.transform.position = archerSpawnPoint.transform.position;
            archerLevel = 1;
        }
    }

    public void UpgradeArcher()
    {
        if( archerLevel < 3)
        {
            archerLevel++;
            ArcherAI.instance.LevelUp();
        }
    }
}
