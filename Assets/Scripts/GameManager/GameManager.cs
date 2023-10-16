using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : BaseManager<GameManager>
{
    private int scores = 0;
    public int Scores => scores;

    [SerializeField] private List<Level_ScriptableObject> level_ScriptableObjects = new();
    private int level;
    private Texture[] tileTexture;
    private GameObject tilePrefab;
    private int levelSelected;

    private void Start()
    {
        levelSelected = 1;
    }

    private void Update()
    {
        switch (levelSelected)
        {
            case 1:
                level = 1;
                break;
            case 2:
                level = 2; 
                break;
            case 3:
                level = 3; 
                break;
            default: break;
        }
    }

    public void UpdateScores(int v)
    {
        scores = v;
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