using UnityEngine;

[CreateAssetMenu()]
public class Level_ScriptableObject : ScriptableObject
{
    public int level;
    public Texture[] tileTexture;
    public GameObject tilePrefab;
}