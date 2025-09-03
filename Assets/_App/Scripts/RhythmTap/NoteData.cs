using System;

[Serializable]
public class NoteData
{
    public NoteType NoteType;
    public double Time;
    public bool IsSpawned;
    
    public NoteData(NoteType noteType, double time)
    {
        NoteType = noteType;
        Time = time;
    }

}