using System.Collections.Generic;
using System;

[Serializable]
public class Level
{
    public int level;
    public int time;
    public List<string> nameTextures;
}

[Serializable]
public class Levels
{
    public List<Level> level_List;
}

public class TileComparer : IComparer<Tile>
{
    public int Compare(Tile tile1, Tile tile2)
    {
        return string.Compare(tile1.name, tile2.name, StringComparison.Ordinal);
    }
}