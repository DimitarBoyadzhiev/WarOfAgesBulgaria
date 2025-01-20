using UnityEngine;

public class BaseScript : MonoBehaviour
{
    [SerializeField]
    private GameObject soldier;
    [SerializeField]
    private GameObject archer;
    [SerializeField]
    private GameObject archerSpawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnSolider()
    {
        Instantiate(soldier);
    }

    public void SpawnArcher()
    {

        Instantiate(archer);
        archer.transform.position = archerSpawnPoint.transform.position;
    }
}
