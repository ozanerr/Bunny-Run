using System.Collections.Generic;
using UnityEngine;

public class EnemyZoneTrigger : MonoBehaviour
{
    public List<EnemyController> enemiesInZone = new List<EnemyController>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Runner"))
        {
            Debug.Log("Player masuk zona!");

            GameObject[] allRunners = GameObject.FindGameObjectsWithTag("Runner");

            foreach (EnemyController enemy in enemiesInZone)
            {
                if (enemy != null)
                {
                    Transform closestRunner = GetClosestRunner(enemy.transform, allRunners);
                    if (closestRunner != null)
                    {
                        enemy.StartChasing(closestRunner);
                    }
                }
            }
        }
    }

    private Transform GetClosestRunner(Transform enemy, GameObject[] runners)
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject runnerObj in runners)
        {
            if (runnerObj == null) continue;
            float dist = Vector3.Distance(enemy.position, runnerObj.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = runnerObj.transform;
            }
        }

        return closest;
    }
}
