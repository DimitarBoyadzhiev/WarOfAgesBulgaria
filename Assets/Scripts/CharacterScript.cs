using System.Collections;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private int hitPoints = 10;
    [SerializeField]
    private int currHP;
    [SerializeField]
    private int attackDamage = 2;

    public float attackRange = 0.5f;

    public float attackRate = 1f;
    private float nextAttackTime = 1f;

    public Transform attackPoint;
    public LayerMask enemyLayers;

    protected bool moveTrue = true;
    public bool enemyFound = false;

    private Animator m_animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currHP = hitPoints;
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (moveTrue)
        {
            Move(moveSpeed);
        }

        if (enemyFound)
        {
            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    private void Move(float speed)
    {

        if (gameObject.tag == "Enemy")
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            m_animator.SetInteger("AnimState", 2);
        }
        if (gameObject.tag == "Player")
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            m_animator.SetInteger("AnimState", 2);
        }
    }

    private void Attack()
    {
        //Attack animation
        m_animator.SetTrigger("Attack");

        //Detects enemies in attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        if (hitEnemies[0].GetComponent<EnemyCharacterScript>())
        {
            hitEnemies[0].GetComponent<EnemyCharacterScript>().TakeDamage(attackDamage);
        }

        if (hitEnemies[0].GetComponent<EnemyBaseScript>())
        {
            hitEnemies[0].GetComponent<EnemyBaseScript>().TakeDamage(attackDamage);
        }

    }

    public void TakeDamage(int damage)
    {
        currHP -= damage;

        if (currHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        m_animator.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        DoDelayAction(1);
    }

    void DoDelayAction(float delayTime)
    {
        StartCoroutine(DelayAction(delayTime));
    }
    IEnumerator DelayAction(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);
        //Do the action after the delay time has finished.
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            moveTrue = false;
            enemyFound = true;
            m_animator.SetInteger("AnimState", 1);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            moveTrue = false;
            enemyFound = true;
        }

        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<CharacterScript>())
            {
                if (collision.gameObject.GetComponent<CharacterScript>().enemyFound == true)
                {
                    moveTrue = false;
                }
                else
                {
                    moveTrue = true;
                }
            }
            
        }

        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemyFound = false;
        moveTrue = true;
    }
}
