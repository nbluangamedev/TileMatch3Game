using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : BaseManager<GameManager>
{
    private int scores = 0;
    public int Scores => scores;

    private bool isPlaying = false;
    public bool IsPlaying => isPlaying;

    public void UpdateScores(int v)
    {
        scores = v;
    }

    public void StartGame()
    {
        isPlaying = true;
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        if (isPlaying)
        {
            isPlaying = false;
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        isPlaying = true;
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        scores = 0;
        ChangeScene("Menu");

        //if (UIManager.HasInstance)
        //{
        //    UIManager.Instance.ActiveVictoryPanel(false);
        //    UIManager.Instance.ActiveGamePanel(false);
        //    UIManager.Instance.ActiveLosePanel(false);
        //    UIManager.Instance.ActiveMenuPanel(true);
        //    UIManager.Instance.GamePanel.NumberOfDiamond.SetText("0");
        //}
    }

    public void EndGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}