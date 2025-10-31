using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float timeLeft = 10f; // se

    void Update()
    {
        timeLeft = Mathf.Clamp(timeLeft - Time.deltaTime, 0f, 999f);

        int totalSeconds = Mathf.CeilToInt(timeLeft);

        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
