using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemyCount = 10;
    public Transform spawnArea; 
    public EnemyZoneTrigger zoneTrigger;

    private void Start()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 pos = spawnArea.position + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
            GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
            zoneTrigger.enemiesInZone.Add(enemy.GetComponent<EnemyController>());
        }
    }
}
