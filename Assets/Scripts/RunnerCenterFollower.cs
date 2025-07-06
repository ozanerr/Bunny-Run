using System.Collections.Generic;
using UnityEngine;

public class RunnerCenterFollower : MonoBehaviour
{
    private List<Transform> runners = new List<Transform>();

    public void SetRunners(List<Transform> newRunners)
    {
        runners = newRunners;
    }

    void LateUpdate()
    {
        if (runners.Count == 0) return;

        Vector3 center = Vector3.zero;
        Vector3 averageForward = Vector3.zero;
        int validCount = 0;

        foreach (var runner in runners)
        {
            if (runner != null)
            {
                center += runner.position;
                averageForward += runner.forward;
                validCount++;
            }
        }

        if (validCount == 0) return;

        center /= validCount;
        averageForward.Normalize();

        transform.position = new Vector3(center.x, transform.position.y, center.z);
        if (averageForward != Vector3.zero)
            transform.forward = new Vector3(averageForward.x, 0, averageForward.z); 
    }

}
