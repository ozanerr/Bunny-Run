using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using PathCreation;

public class PlayerManager : MonoBehaviour
{
    public Transform player;
    [SerializeField] private TextMeshPro counterText;
    [SerializeField] private GameObject runner;
    [SerializeField] private RunnerCenterFollower runnerCenter;

    [Range(0f, 1f)][SerializeField] private float distanceFactor, radius;
    [SerializeField] private float yOffset = 1f;

    private List<GameObject> runners = new List<GameObject>();
    public int RunnerCount => runners.Count;

    // Timer untuk optimasi
    private float heavyUpdateTimer = 0f;
    private float heavyUpdateInterval = 0.1f;

    void Start()
    {
        player = transform;
        RefreshRunners();
        UpdateCenterReference();
        SetRunnerAnimations(false);
    }

    void Update()
    {
        UpdateCounter();
        CheckForGameOver();

        heavyUpdateTimer += Time.deltaTime;
        if (heavyUpdateTimer >= heavyUpdateInterval)
        {
            RemoveDeadRunners();
            UpdateCenterReference();
            heavyUpdateTimer = 0f;
        }
    }

    // --- FUNGSI INI DIPERBAIKI UNTUK SPAWN INSTAN ---
    private IEnumerator SpawnRunnerWithDelay(int number)
    {
        Quaternion baseRotation = runners.Count > 0 ? runners[0].transform.rotation : transform.rotation;

        for (int i = 0; i < number; i++)
        {
            Vector3 formOffset = GetFormationOffset(runners.Count);
            GameObject newRunner = Instantiate(runner, transform.position + formOffset, baseRotation, transform);
            runners.Add(newRunner);

            // Jeda 'yield return null' dihapus dari dalam loop agar instan
        }

        // Coroutine sekarang hanya menunggu satu frame setelah SEMUA runner dibuat.
        yield return null;
    }

    // --- SISA SCRIPT ANDA TIDAK DIUBAH ---

    private void CheckForGameOver()
    {
        if (GameManager.instance != null && GameManager.instance.IsGameStarted && !GameManager.instance.IsGameOver)
        {
            if (runners.Count == 0)
            {
                GameManager.instance.GameOver();
                this.enabled = false;
            }
        }
    }

    private void UpdateCounter()
    {
        if (counterText != null)
        {
            counterText.text = runners.Count.ToString();
        }
    }

    private void RemoveDeadRunners()
    {
        for (int i = runners.Count - 1; i >= 0; i--)
        {
            if (runners[i] == null || runners[i].transform.position.y < -10f)
            {
                if (runners[i] != null)
                {
                    Destroy(runners[i]);
                }
                runners.RemoveAt(i);
            }
        }
    }

    private void MakeRunner(int number)
    {
        StartCoroutine(SpawnRunnerWithDelay(number));
    }

    private Vector3 GetFormationOffset(int index)
    {
        if (index == 0) return Vector3.zero;

        index -= 1;
        float x = distanceFactor * Mathf.Sqrt(index) * Mathf.Cos(index * radius);
        float z = distanceFactor * Mathf.Sqrt(index) * Mathf.Sin(index * radius);
        return new Vector3(x, yOffset, z);
    }

    public void CloneFromGate(int amount)
    {
        Debug.Log($"[PlayerManager] Cloning {amount} runners");
        MakeRunner(amount);
    }

    private void RefreshRunners()
    {
        runners.Clear();
        foreach (Transform child in player)
        {
            if (child.CompareTag("Runner"))
            {
                runners.Add(child.gameObject);
            }
        }
    }

    private void UpdateCenterReference()
    {
        List<Transform> runnerTransforms = new List<Transform>();
        foreach (var r in runners)
        {
            if (r != null)
                runnerTransforms.Add(r.transform);
        }
        if (runnerCenter != null)
        {
            runnerCenter.SetRunners(runnerTransforms);
        }
    }

    public void RemoveRunner(GameObject runnerToRemove)
    {
        if (runners.Contains(runnerToRemove))
        {
            runners.Remove(runnerToRemove);
            UpdateCounter();
        }
    }

    public void KillRunners(int amount)
    {
        int count = Mathf.Min(amount, runners.Count);
        for (int i = 0; i < count; i++)
        {
            GameObject runner = runners[runners.Count - 1];
            runners.RemoveAt(runners.Count - 1);
            if (runner != null)
            {
                Destroy(runner);
            }
        }
        UpdateCounter();
    }

    public void SetRunnerAnimations(bool isEnabled)
    {
        foreach (GameObject runnerInstance in runners)
        {
            if (runnerInstance != null)
            {
                Animator anim = runnerInstance.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.enabled = isEnabled;
                }
            }
        }
    }

    public void FreezeAllRunners()
    {
        Debug.Log("Executing TOTAL FREEZE on all runners.");
        foreach (GameObject runnerInstance in runners)
        {
            if (runnerInstance != null)
            {
                Rigidbody rb = runnerInstance.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.isKinematic = true;
                }
                Animator anim = runnerInstance.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.applyRootMotion = false;
                    anim.enabled = false;
                }
            }
        }
    }

    public float GetFrontRunnerDistance(PathCreator pathCreator)
    {
        float maxDistance = 0f;
        if (pathCreator == null || runners.Count == 0)
        {
            return 0f;
        }
        foreach (GameObject runner in runners)
        {
            if (runner != null)
            {
                float distance = pathCreator.path.GetClosestDistanceAlongPath(runner.transform.position);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                }
            }
        }
        return maxDistance;
    }
}
