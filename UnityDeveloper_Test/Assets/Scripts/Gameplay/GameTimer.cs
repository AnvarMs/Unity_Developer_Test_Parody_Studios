using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public int time = 180; // 3 minutes
    private float timer = 1;
    private bool isTimerEnd = false;

    void Start()
    {
        GameManager.instance.StartGame();
        time = 180;
        timer = 1;
    }

    void Update()
    {
        if (isTimerEnd) return;

        if (time <= 0)
        {
            UIManager.instance.OnLoose("Time's Up");
            isTimerEnd = true;
        }

        if (timer <= 0)
        {
            timer = 1;
            time--;
            UpdateUI();
        }

        timer -= Time.deltaTime;
    }

    void UpdateUI()
    {
        int minutes = time / 60;
        int seconds = time % 60;
        UIManager.instance.UpdateTimerText($"{minutes:00}:{seconds:00} / 03:00");
    }
}
