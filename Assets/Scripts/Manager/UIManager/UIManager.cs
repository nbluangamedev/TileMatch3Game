using UnityEngine;

public class UIManager : BaseManager<UIManager>
{
    [SerializeField]
    private MenuPanel menuPanel;
    public MenuPanel MenuPanel => menuPanel;

    [SerializeField]
    private SettingPanel settingPanel;
    public SettingPanel SettingPanel => settingPanel;

    [SerializeField]
    private LoadingPanel loadingPanel;
    public LoadingPanel LoadingPanel => loadingPanel;

    [SerializeField]
    private GamePanel gamePanel;
    public GamePanel GamePanel => gamePanel;

    [SerializeField]
    private WinPanel winPanel;
    public WinPanel WinPanel => winPanel;

    [SerializeField]
    private LosePanel losePanel;
    public LosePanel LosePanel => losePanel;

    private void Start()
    {
        ActiveMenuPanel(true);
        ActiveLoadingPanel(false);
        ActiveSettingPanel(false);
        ActiveGamePanel(false);
        ActiveWinPanel(false);
        ActiveLosePanel(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.HasInstance && GameManager.Instance.IsPlaying == true)
            {
                GameManager.Instance.PauseGame();
                ActiveSettingPanel(true);
            }
        }
    }

    public void ActiveMenuPanel(bool active)
    {
        menuPanel.gameObject.SetActive(active);
    }

    public void ActiveSettingPanel(bool active)
    {
        settingPanel.gameObject.SetActive(active);
    }

    public void ActiveLoadingPanel(bool active)
    {
        loadingPanel.gameObject.SetActive(active);
    }

    public void ActiveGamePanel(bool active)
    {
        gamePanel.gameObject.SetActive(active);
    }

    public void ActiveWinPanel(bool active)
    {
        winPanel.gameObject.SetActive(active);
    }

    public void ActiveLosePanel(bool active)
    {
        losePanel.gameObject.SetActive(active);
    }
}