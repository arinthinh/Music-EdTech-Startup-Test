using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SongConfigSO", menuName = "ScriptableObject/SongConfigSO")]
public class SongDataSO : ScriptableObject
{
    public List<SongData> Songs;
}
 

[Serializable]
public class SongData
{
    public string SongId;
    public string SongName;
    public int MidiIndex;
    public int BPM;
    public int MainOctave;
}