using Unity.VisualScripting;
using UnityEngine;

public class EnemyBaseScript : MonoBehaviour
{

    [SerializeField]
    private GameObject soldier;

    [SerializeField] private int maxHP = 250;
    [SerializeField] private int currentHP;

    public HealthBar healthBar;
    public float cooldown;
    float lastSpawn;

    private int deathScore = 1000;

    private void Awake()
    {
        currentHP = maxHP;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar.SetMaxHealth(maxHP);
    }

    void Update()
    {

        SpawnSolider();

    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        healthBar.SetHealth(currentHP);
        ScoreManager.Instance.AddScore(damage);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        ScoreManager.Instance.AddScore(deathScore);
        GameManager.instance.EndGame();
    }

    public void SpawnSolider()
    {
        if (Time.time - lastSpawn < cooldown)
        {
            return;
        }
        lastSpawn = Time.time;
        Instantiate(soldier);
    }
}
