using UnityEngine;

public class EnemyBaseScript : MonoBehaviour
{

    [SerializeField]
    private GameObject soldier;

    public float spawnTime;
    public float spawnDelay;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnSolider", spawnTime, spawnDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnSolider()
    {
        Instantiate(soldier);
    }
}
