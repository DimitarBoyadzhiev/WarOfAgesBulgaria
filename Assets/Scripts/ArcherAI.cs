using NUnit.Framework;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ArcherAI : MonoBehaviour
{

    private Animator animator;
    private GameObject currentTarget;

    //Combat stats
    [SerializeField]  private float attackRate = 1f;
    [SerializeField]  private float nextAttackTime = 1f;
    [SerializeField]  private int attackDamage = 2;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float attackRange = 12f;
    private bool enemyFound = false;
    private Collider2D[] hitEnemies;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();

        animator.SetBool("IsIdle", true);
    }

    // Update is called once per frame
    void Update()
    {
        hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        if (hitEnemies.Length > 0)
        {
            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;

            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");

        hitEnemies[0].GetComponent<EnemyCharacterScript>().TakeDamage(attackDamage);
        
    }
}
