using NUnit.Framework;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ArcherAI : MonoBehaviour
{
    public static ArcherAI instance;

    private Animator animator;
    private GameObject currentTarget;

    //Combat stats
    [SerializeField]  private float shootTimerMax;
    [SerializeField]  private float shootTimer;
    [SerializeField]  private int attackDamage = 2;
    public float attackRange;
    private EnemyCharacterScript enemy;
    private Vector3 projectileSpawnPoint;
    

    private void Awake()
    {
        instance = this;
        projectileSpawnPoint = transform.Find("ProjectileSpawnPoint").position;

        attackRange = 10f;
        shootTimerMax = .5f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        //Anim state 1 = Idle animation
        animator.SetInteger("AnimState", 1);
    }

    // Update is called once per frame
    void Update()
    {

        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            shootTimer = shootTimerMax;

            EnemyCharacterScript enemy = GetClosestEnemy();
            if (enemy != null)
            {
                ProjectileArrow.CreateArrow(projectileSpawnPoint, enemy.GetPosition());
                Attack(enemy);
            }
            else
            {
                animator.SetInteger("AnimState", 1);
            }
        }
    }

    public void LevelUp()
    {
        attackDamage += 2;
        attackRange += 1f;
    }

    private EnemyCharacterScript GetClosestEnemy()
    {
        return EnemyCharacterScript.GetClosestEnemy(transform.position, attackRange);
    }

    private void Attack(EnemyCharacterScript enemy)
    {
        //Anim state 2 = Attack animation
        animator.SetInteger("AnimState", 2);

        enemy.TakeDamage(attackDamage);
        
    }

    public void SetEnemy(EnemyCharacterScript enemy)
    {
        this.enemy = enemy;
    }

    
}
