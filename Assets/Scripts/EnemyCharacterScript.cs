using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacterScript : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private int hitPoints = 20;
    [SerializeField] private int currHP;
    private bool isDead = false;

    [SerializeField]
    private int attackDamage = 2;

    public float attackRange = 0.5f;

    private bool moveTrue = true;
    private bool enemyFound = false;

    public float attackRate = 2f;
    private float nextAttackTime = 1f;

    private Animator m_animator;

    public Transform attackPoint;
    public LayerMask enemyLayers;

    private int score = 10;

    private int goldDropAmout = 1;

    public static List<EnemyCharacterScript> enemyList = new List<EnemyCharacterScript>();

    public static List<EnemyCharacterScript> GetEnemyList()
    {
        return enemyList;
    }

    public static EnemyCharacterScript GetClosestEnemy(Vector3 position, float maxRange)
    {
        EnemyCharacterScript closest = null;
        foreach (EnemyCharacterScript enemy in enemyList)
        {
            if(enemy.isDead) continue;
            if(Vector3.Distance(position, enemy.GetPosition()) <= maxRange)
            {
                if(closest == null) closest = enemy;
                else
                {
                    if(Vector3.Distance(position, enemy.GetPosition()) < Vector3.Distance(position, closest.GetPosition())){
                        closest = enemy;
                    }
                }
            }
        }

        return closest;
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }

    private void Awake()
    {
        enemyList.Add(this);
    }

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
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        this.enabled = false;
        isDead = true;
        ScoreManager.Instance.AddScore(score);
        GameManager.instance.AddGold(goldDropAmout);
        enemyList.Remove(this);
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

    private void Attack()
    {
        //Attack animation
        m_animator.SetTrigger("Attack");

        //Detects enemies in attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Damage enemies
        if (hitEnemies[0].GetComponent<CharacterScript>())
        {
            hitEnemies[0].GetComponent<CharacterScript>().TakeDamage(attackDamage);
        }

        if (hitEnemies[0].GetComponent<BaseScript>())
        {
            hitEnemies[0].GetComponent<BaseScript>().TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            moveTrue = false;

            m_animator.SetInteger("AnimState", 1);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemyFound = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemyFound = false;
        moveTrue = true;
    }
}
