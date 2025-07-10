using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class CreditScene : MonoBehaviour
{
  [Header("UI References")]
  public TextMeshProUGUI creditText;
  public GameObject playAgainButtonObject;

  private Button playAgainButton;

  [Header("Credit Settings")]
  public float delayBetweenLines = 1.5f;

  private string[] creditLines = new string[]
  {
    "Tugas ini dibuat untuk memenuhi",
    "salah satu tugas mata kuliah",
    "Pengembangan Aplikasi Gim",
    "Tugas ini dibuat oleh Kelompok Traiblazer",
    "Anggota:",
    "Mhd. Fauzan Aditya (221110425)",
    "Nicholas Tandri (221111758)",
    "Filbert Febrian Tanada (221110476)",
    "Filbert Wijaya (221112207)",
    "Kevin Wijaya (221113189)",
  };

  void Start()
  {
    creditText.text = "";
    playAgainButtonObject.SetActive(false);

    playAgainButton = playAgainButtonObject.GetComponent<Button>();
    playAgainButton.onClick.AddListener(OnPlayAgainButtonClicked);

    StartCoroutine(ShowCredits());
  }

  private IEnumerator ShowCredits()
  {
    foreach (string line in creditLines)
    {
      creditText.text = line + "\n";
      yield return new WaitForSeconds(delayBetweenLines);
    }

    yield return new WaitForSeconds(1f);
    creditText.text = "Want to play again?";
    playAgainButtonObject.SetActive(true);
  }

  public void OnPlayAgainButtonClicked()
  {
    SceneManager.LoadScene("level1");
  }
}
