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

    private readonly double _spawnLeadTime = 1000; // Time in miliseconds before the note should be hit to spawn it

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
        _midiFilePlayer.MPTK_Load();
        _midiFilePlayer.MPTK_Tempo = 80;
        LoadNoteData();
        await UniTask.Yield();
        StartSong();
    }

    private void LoadNoteData()
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
                    _noteDatas.Add(new( note, midiEvent.RealTime));
                }
            }
        }
    }


    private void StartSong()
    {
        _activeNotes.Clear();
        _midiFilePlayer.MPTK_Play();
    }

    private void Update()
    {
        var currentSongTime = _midiFilePlayer.MPTK_RealTime;
        HandleNoteSpawning(currentSongTime);
        UpdateActiveNotes(currentSongTime);
    }

    private void HandleNoteSpawning(double currentSongTime)
    {
        foreach (var noteData in _noteDatas)
        {
            if (ShouldSpawnNote(noteData, currentSongTime))
            {
                SpawnNote(noteData);
            }
        }
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

    private void SpawnNote(NoteData noteData)
    {
        var laneIndex = (int)noteData.NoteType;
        var laneTransform = _lanes[laneIndex];
        var note = _noteFactory.Get();
        note.transform.position = laneTransform.position;
        note.OnSpawn(noteData, laneTransform);
        _activeNotes.Add(note);
    }

    private bool ShouldSpawnNote(NoteData noteData, double currentSongTime)
    {
        return !noteData.IsSpawned && noteData.Time >= currentSongTime &&
               noteData.Time < currentSongTime + _spawnLeadTime;
    }

    private void UpdateActiveNotes(double currentSongTime)
    {
        foreach (var note in _activeNotes)
        {
            note.Move(currentSongTime);
        }
    }


    private void OnNoteStartHold(NoteStartHoldEvent evt)
    {
    }


    private void OnNoteStopHold(NoteStopHoldEvent obj)
    {
    }
}