using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private Transform target;
    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        Transform nearestRunner = FindNearestRunner();
        if (nearestRunner != null)
        {
            Vector3 dir = (nearestRunner.position - transform.position).normalized;
            dir.y = 0;
            transform.rotation = Quaternion.LookRotation(dir);
        }

        if (animator != null)
        {
            animator.enabled = false;
        }
    }

    void Update()
    {
        if (isChasing && target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    public void StartChasing(Transform runner)
    {
        target = runner;
        isChasing = true;

        if (animator != null)
        {
            animator.enabled = true;
        }
    }

    private Transform FindNearestRunner()
    {
        GameObject[] runners = GameObject.FindGameObjectsWithTag("Runner");
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject r in runners)
        {
            float dist = Vector3.Distance(transform.position, r.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = r.transform;
            }
        }

        return closest;
    }
}