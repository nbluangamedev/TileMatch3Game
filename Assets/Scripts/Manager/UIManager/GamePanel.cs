using TMPro;
using UnityEngine;

public class GamePanel : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;

    private bool timerIsRunning = false;
    private float timeRemaining;
    private int timer;

    private void Start()
    {
        if (GameManager.HasInstance)
        {
            timer = GameManager.Instance.levels.levels[GameManager.Instance.CurrentLevel].time;
        }
        SetTimeRemain(timer);
    }

    private void OnEnable()
    {
        if (GameManager.HasInstance)
        {
            timer = GameManager.Instance.levels.levels[GameManager.Instance.CurrentLevel].time;
        }
        SetTimeRemain(timer);
        timerIsRunning = true;
        TileManager.matchThreeDelegate += OnMatchThree;
    }

    private void OnDisable()
    {
        TileManager.matchThreeDelegate -= OnMatchThree;
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                if (UIManager.HasInstance && AudioManager.HasInstance)
                {
                    UIManager.Instance.ActiveLosePanel(true);
                    AudioManager.Instance.PlaySE(AUDIO.SE_LOSE);
                }
            }
        }
    }

    private void OnMatchThree(int value)
    {
        scoreText.SetText("Score: " + value.ToString());
    }

    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetTimeRemain(float v)
    {
        timeRemaining = v;
    }

    public void OnSettingButtonClick()
    {
        if (UIManager.HasInstance && GameManager.HasInstance)
        {
            UIManager.Instance.ActiveSettingPanel(true);
            GameManager.Instance.PauseGame();
        }
    }
}