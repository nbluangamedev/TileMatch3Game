 using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LevelData
{    
    public int level;
    public int time;    
    public List<Texture2D> tileTextures;
}

[Serializable]
public class LevelDatas
{
    public List<LevelData> levelDatas;
}

public class TileComparer : IComparer<Tile>
{
    public int Compare(Tile tile1, Tile tile2)
    {
        return string.Compare(tile1.name, tile2.name, StringComparison.Ordinal);
    }
}