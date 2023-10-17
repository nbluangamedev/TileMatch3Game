using DG.Tweening;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : BaseManager<GameManager>
{
    private readonly string filePath = Path.Combine("Assets", "Resources", "LevelData", "levels.json");
    private readonly string CURRENT_LEVEL = "CurrentLevel";
    private readonly string CURRENT_SCORE = "CurrentScore";

    public GameObject tilePrefab;
    public Levels levels = new();

    private int currentLevel;
    public int CurrentLevel => currentLevel;

    private int scores = 0;
    public int Scores => scores;

    private bool isPlaying = false;
    public bool IsPlaying => isPlaying;

    private void Start()
    {
        DOTween.SetTweensCapacity(1000, 50);
        PlayerPrefs.DeleteKey(CURRENT_LEVEL);

        currentLevel = PlayerPrefs.GetInt(CURRENT_LEVEL, 0);
        scores = PlayerPrefs.GetInt(CURRENT_SCORE, 0);
        Debug.Log("currenLevel " + currentLevel);

        //load file
        //string loadedData = File.ReadAllText(filePath);
        //levels = JsonUtility.FromJson<Levels>(loadedData);

        //save file
        string jsonToSave = JsonUtility.ToJson(levels);
        File.WriteAllText(filePath, jsonToSave);
    }

    public List<Texture2D> GetTextureByLevel(int levelIndex)
    {
        return levels.levels[levelIndex].tileTextures;
    }

    public int GetNumberTextureByLevel(int levelIndex)
    {
        return levels.levels[levelIndex].tileTextures.Count;
    }

    public void UpdateLevel(int level)
    {
        currentLevel = level;
        PlayerPrefs.SetInt(CURRENT_LEVEL, currentLevel);
    }

    public void UpdateScores(int score)
    {
        scores = score;
        PlayerPrefs.SetInt(CURRENT_SCORE, scores);
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
        ChangeScene("Main");

        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveWinPanel(false);
            UIManager.Instance.ActiveGamePanel(false);
            UIManager.Instance.ActiveLosePanel(false);
            UIManager.Instance.ActiveMenuPanel(true);
        }
    }

    public void EndGame()
    {
        PlayerPrefs.SetInt(CURRENT_LEVEL, currentLevel);
        PlayerPrefs.SetInt(CURRENT_SCORE, scores);

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