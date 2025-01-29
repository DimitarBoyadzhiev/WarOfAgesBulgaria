using UnityEngine;

public class TargetingScriptArcherList : MonoBehaviour
{
    [SerializeField] private float range;

    private ArcherAI archerAI;

    private void Awake()
    {
        archerAI = GetComponent<ArcherAI>();
    }

    private void Update()
    {
        foreach(EnemyCharacterScript enemy in EnemyCharacterScript.GetEnemyList())
        {
            if(Vector3.Distance(transform.position, enemy.transform.position) < range)
            {
                //Enemy within range
                archerAI.SetEnemy(enemy);
            }
        }
    }
}
