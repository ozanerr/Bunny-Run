using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LivesUIManager : MonoBehaviour
{
    public List<Image> heartImages;

    public void UpdateLives(int currentLives)
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < currentLives)
            {
                heartImages[i].enabled = true;
            }
            else
            {
                heartImages[i].enabled = false;
            }
        }
    }
}