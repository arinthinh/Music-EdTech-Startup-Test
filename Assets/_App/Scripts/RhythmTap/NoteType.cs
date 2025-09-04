public enum NoteType
{
    None = 0,
    C = 1,
    D = 2,
    E = 3,
    F = 4,
    G = 5,
    A = 6,
    B = 7
}

public class NoteHelper
{
    
    /// <summary>
    /// Convert MIDI number in octave 5 to NoteType. Returns null if not octave 5 or not a natural note.
    /// </summary>
    public static NoteType ConvertMidiNoteToNoteType(int midiNote, int octave = 5)
    {
        midiNote += 12;
        
        if (midiNote < octave * 12 || midiNote >= (octave + 1) * 12)
        {
            return NoteType.None;
        }

        int noteInOctave = midiNote % 12;
        switch (noteInOctave)
        {
            case 0: return NoteType.C;
            case 2: return NoteType.D;
            case 4: return NoteType.E;
            case 5: return NoteType.F;
            case 7: return NoteType.G;
            case 9: return NoteType.A;
            case 11: return NoteType.B;
            default: return NoteType.None;
        }
    }
}