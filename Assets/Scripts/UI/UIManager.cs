using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    [SerializeField] private TextMeshProUGUI numberOfDiamond;
    [SerializeField] private TextMeshProUGUI timeText;

    public TextMeshProUGUI NumberOfDiamond => numberOfDiamond;
    private float timeRemaining;
    private bool timerIsRunning = false;
    private int timer;

    private void Awake()
    {
        timer = tileManager.level_ScriptableObject.level * 60;
    }
    private void Start()
    {
        SetTimeRemain(timer);
    }

    private void OnEnable()
    {
        SetTimeRemain(timer);
        timerIsRunning = true;
        TileManager.collectDiamondDelegate += OnPlayerCollect;
    }

    private void OnDisable()
    {
        TileManager.collectDiamondDelegate -= OnPlayerCollect;
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
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                if (GameManager.HasInstance && AudioManager.HasInstance)
                {
                    AudioManager.Instance.PlaySE(AUDIO.SE_LOSE);
                    //GameManager.Instance.RestartGame();
                }
            }
        }
    }

    private void OnPlayerCollect(int value)
    {
        numberOfDiamond.SetText("Score: " + value.ToString());
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("Time remain: {0:00}:{1:00}", minutes, seconds);
    }

    public void SetTimeRemain(float v)
    {
        timeRemaining = v;
    }
}