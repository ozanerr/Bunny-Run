using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PathFollower pathFollower;
    [SerializeField] private float limitValue = 1f;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        float halfScreen = Screen.width / 2;
        float xPos = (Input.mousePosition.x - halfScreen) / halfScreen;
        float finalOffset = -xPos * limitValue;

        if (pathFollower != null)
        {
            pathFollower.SetInputOffset(finalOffset);
        }
    }
}