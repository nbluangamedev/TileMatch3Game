using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private List<Level_ScriptableObject> level_ScriptableObjects;

    private int level;
    private Texture[] tileTexture;
    private GameObject tilePrefab;

    private int levelSelected;

    private void Start()
    {
        
    }
}
