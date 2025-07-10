using UnityEngine;
using TMPro;
using System.Collections;

public class GateManager : MonoBehaviour
{
    [Header("Tipe Objek")]
    // Saklar baru: centang ini jika objek adalah tembok yang bisa dihancurkan
    public bool isBreakableWall = false;

    [Header("Pengaturan Gerbang & Tembok")]
    public bool multiply = false;
    public bool isSubtract = false;
    public bool isDivision = false;
    // Untuk Tembok, nilai ini akan menjadi jumlah pengurangan
    public int gateValue = 0;
    public GateGroup gateGroup;

    [Header("UI")]
    public TextMeshPro GateNo;

    private bool triggered = false;

    void Awake()
    {
        // Hanya inisialisasi teks jika ini BUKAN tembok
        if (!isBreakableWall)
        {
            InitGate();
        }
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

        // --- PERUBAHAN TAMPILAN TEKS ---
        if (GateNo != null)
        {
            if (isSubtract)
                GateNo.text = $"SUB {gateValue}";
            else if (multiply)
                GateNo.text = $"MPY {gateValue}";
            else if (isDivision)
                GateNo.text = $"DIV {gateValue}";
            else
                GateNo.text = $"PLUS {gateValue}";
        }
        triggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Runner"))
        {
            triggered = true;

            if (isBreakableWall)
            {
                HandleWallCollision(other);
            }
            else
            {
                HandleGateCollision(other);
            }
        }
    }

    // --- PERUBAHAN LOGIKA TEMBOK ---
    private void HandleWallCollision(Collider runnerCollider)
    {
        Debug.Log("Runner menabrak tembok! Mengurangi runner sebanyak: " + gateValue);
        AudioManager.instance.PlayWallBreakSound();

        // Temukan PlayerManager
        PlayerManager playerManager = FindObjectOfType<PlayerManager>();
        if (playerManager != null)
        {
            // Kurangi runner sejumlah gateValue
            playerManager.KillRunners(gateValue);
        }

        // Hancurkan tembok itu sendiri setelah mengurangi runner
        Destroy(gameObject);
    }

    private void HandleGateCollision(Collider runnerCollider)
    {
        if (gateGroup != null && gateGroup.HasTriggered)
        {
            triggered = false;
            return;
        }

        AudioManager.instance.PlayGateSound();
        StartCoroutine(TriggerGate(runnerCollider));
    }

    private IEnumerator TriggerGate(Collider other)
    {
        if (gateGroup != null)
        {
            gateGroup.MarkTriggered();
        }

        PlayerManager playerManager = other.GetComponentInParent<PlayerManager>();
        if (playerManager != null)
        {
            int currentCount = playerManager.RunnerCount;
            int addAmount = 0;

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
