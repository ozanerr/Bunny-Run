using UnityEngine;

public class GateGroup : MonoBehaviour
{
    private bool triggered = false;

    public bool HasTriggered => triggered;

    public void MarkTriggered()
    {
        triggered = true;
    }

    public void ResetTrigger()
    {
        triggered = false;
    }
}
