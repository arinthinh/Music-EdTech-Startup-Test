using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SongConfigSO", menuName = "ScriptableObject/SongConfigSO")]
public class SongConfigSO : ScriptableObject
{
    public SongConfig[] Songs;
}


[Serializable]
public class SongConfig
{
    public string SongId;
    public string SongName;
    public string MidiFile;
}