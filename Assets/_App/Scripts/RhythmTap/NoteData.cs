using System;

[Serializable]
public class NoteData
{
    public NoteType NoteType;
    public double Tick;
    public bool IsSpawned;
    
    public NoteData(NoteType noteType, double tick)
    {
        NoteType = noteType;
        Tick = tick;
    }

}