using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MidiPlayerTK;
using UnityEngine;

public class RhythmTapController : MonoBehaviour
{
    [SerializeField] private SongData _songData;
    [SerializeField] private MidiFilePlayer _midiFilePlayer;
    [SerializeField] private NoteFactory _noteFactory;
    [SerializeField] private Transform[] _lanes;

    private SessionData _sessionData;
    private List<MovingNote> _activeNotes = new();
    [SerializeField] private List<NoteData> _noteDatas = new();

    private void OnEnable()
    {
        EventBus.Subscribe<NoteStartHoldEvent>(OnNoteStartHold);
        EventBus.Subscribe<NoteStopHoldEvent>(OnNoteStopHold);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<NoteStartHoldEvent>(OnNoteStartHold);
        EventBus.Unsubscribe<NoteStopHoldEvent>(OnNoteStopHold);
    }

    private async UniTaskVoid Start()
    {
        await UniTask.WaitForSeconds(2f);
        _midiFilePlayer.MPTK_MidiIndex = _songData.MidiIndex;
        _midiFilePlayer.MPTK_Load();
        await UniTask.WaitUntil(() => _midiFilePlayer.MPTK_StatusLastMidiLoaded == LoadingStatusMidiEnum.Success);
        _midiFilePlayer.MPTK_EnableChangeTempo = true;
        _midiFilePlayer.MPTK_Tempo = _songData.BPM;
        SpawnNotes();
        await UniTask.Yield();
        StartSong();
        await UniTask.WaitForSeconds(1f);
        _midiFilePlayer.MPTK_Tempo += 20;
    }

    private void SpawnNotes()
    {
        _noteDatas.Clear();

        var midiEvents = _midiFilePlayer.MPTK_MidiEvents;
        foreach (var midiEvent in midiEvents)
        {
            if (midiEvent.Command == MPTKCommand.NoteOn)
            {
                var note = ConvertMidiNoteToNoteType(midiEvent.Value);
                if (note != NoteType.None)
                {
                    _noteDatas.Add(new(note, midiEvent.Tick));
                }
            }
        }

        foreach (var noteData in _noteDatas)
        {
            var laneIndex = (int)noteData.NoteType - 1;
            var laneTransform = _lanes[laneIndex];
            var note = _noteFactory.Get();
            note.OnSpawn(noteData, laneTransform);
            _activeNotes.Add(note);
        }
    }


    private void StartSong()
    {
        _midiFilePlayer.MPTK_Play();
    }

    private void Update()
    {
        if (!_midiFilePlayer.MPTK_IsPlaying) return;
        UpdateActiveNotes(_midiFilePlayer.MPTK_MidiLoaded.MPTK_TickPlayer);
    }

    /// <summary>
    /// Convert MIDI number in octave 5 to NoteType. Returns null if not octave 5 or not a natural note.
    /// </summary>
    private NoteType ConvertMidiNoteToNoteType(int midiNote)
    {
        midiNote += 12;
        if (midiNote is < 72 or > 83)
            return NoteType.None;

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

    private void UpdateActiveNotes(double currentTick)
    {
        foreach (var note in _activeNotes.ToArray())
        {
            note.UpdatePosition(currentTick);
            if (note.CurrentPositionX <= -Define.NOTE_SPAWN_DISTANCE)
            {
                _activeNotes.Remove(note);
                _noteFactory.Release(note);
            }
        }
    }


    private void OnNoteStartHold(NoteStartHoldEvent evt)
    {
    }


    private void OnNoteStopHold(NoteStopHoldEvent obj)
    {
    }

    /*
    public static float CalculateNoteSpeed(double bpm, int ticksPerQuarterNote = 480)
    {
        return (float)(bpm * ticksPerQuarterNote / 60) / 200f;
    }
    */

    public static float CalculateNoteSpeed(double noteTick, double currentTick, double bpm)
    {
        double ticksPerSecond = bpm * Define.TICKS_PER_QUARTER_NOTE / 60.0;
        double secondsToArrive = (noteTick - currentTick) / ticksPerSecond;
        if (secondsToArrive <= 0) return 0f;
        return (float)(Define.NOTE_SPAWN_DISTANCE / secondsToArrive);
    }

    public static float CalculateTicksPerSecond(float bpm)
    {
        return Define.TICKS_PER_QUARTER_NOTE * bpm / 60f;
    }
}