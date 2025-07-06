using UnityEngine;
using TMPro;
using System.Collections;

public class GateManager : MonoBehaviour
{
    [Header("Gate Settings")]
    public bool multiply = false;
    public bool isSubtract = false;
    public bool isDivision = false;
    public int gateValue = 0;
    public GateGroup gateGroup;

    [Header("UI")]
    public TextMeshPro GateNo;

    private bool triggered = false;

    void Awake()
    {
        InitGate();
    }

    public void InitGate()
    {
        if (gateValue <= 0)
        {
            if (multiply)
            {
                gateValue = Random.Range(2, 4);
            }
            else
            {
                gateValue = Random.Range(10, 30);
                if (gateValue % 2 != 0) gateValue += 1;
            }
        }

        if (GateNo != null) // Diubah
        {
            if (isSubtract)
                GateNo.text = $"sub {gateValue}";
            else if (multiply)
                GateNo.text = $"mpy {gateValue}";
            else if (isDivision)
                GateNo.text = $"div {gateValue}";
            else
                GateNo.text = $"plus {gateValue}";
        }
        triggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isSubtract && other.CompareTag("Runner") && other.GetComponentInParent<PlayerManager>()?.RunnerCount <= 1)
        {
            return;
        }

        if (triggered || (gateGroup != null && gateGroup.HasTriggered)) return;

        if (other.CompareTag("Runner"))
        {
            AudioManager.instance.PlayGateSound();
            StartCoroutine(TriggerGate(other));
        }
    }

    private IEnumerator TriggerGate(Collider other)
    {
        triggered = true;
        if (gateGroup != null)
        {
            gateGroup.MarkTriggered();
        }

        PlayerManager playerManager = other.GetComponentInParent<PlayerManager>();
        if (playerManager != null)
        {
            int currentCount = playerManager.RunnerCount;
            int addAmount = 0;

            // Blok Kalkulasi Diubah
            if (isSubtract)
            {
                addAmount = -Mathf.Min(gateValue, currentCount);
            }
            else if (multiply)
            {
                addAmount = (currentCount * gateValue) - currentCount;
            }
            else if (isDivision)
            {
                if (gateValue > 0)
                {
                    int newCount = Mathf.FloorToInt((float)currentCount / gateValue);
                    addAmount = newCount - currentCount;
                }
            }
            else
            {
                addAmount = gateValue;
            }

            if (addAmount > 0)
            {
                playerManager.CloneFromGate(addAmount);
            }
            else if (addAmount < 0)
            {
                playerManager.KillRunners(-addAmount);
            }
        }

        yield return new WaitForSeconds(0.05f);
        gameObject.SetActive(false);
    }
}