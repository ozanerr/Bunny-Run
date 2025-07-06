using UnityEngine;

public class CounterFollower : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, 0);
    public float smoothSpeed = 10f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
