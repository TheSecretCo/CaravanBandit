using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class LevelConfigData
{
    public List<LevelConfig> levels { get; set; }
}

[Serializable]
public class LevelConfig
{
    public int level { get; set; }
    public string trackPath { get; set; }
    public bool randomTrack { get; set; }
    public List<string> tracks { get; set; }
    public int targetScore { get; set; }
}