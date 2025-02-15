using UnityEngine;

public class EnemyBaseScript : MonoBehaviour
{

    [SerializeField]
    private GameObject soldier;

    public float spawnTime;
    public float spawnDelay;


    [SerializeField] private int maxHP = 250;
    [SerializeField] private int currentHP;

    public HealthBar healthBar;


    private void Awake()
    {
        currentHP = maxHP;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar.SetMaxHealth(maxHP);

        InvokeRepeating("SpawnSolider", spawnTime, spawnDelay);
    }

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
        Instantiate(soldier);
    }
}
