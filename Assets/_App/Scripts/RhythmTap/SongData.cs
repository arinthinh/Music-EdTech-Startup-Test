using System;

[Serializable]
public class SongData
{
    public string SongId;
    public string SongName;
    public int MidiIndex;
    public int BPM = 80;
    public int MainOctave = 5;
}