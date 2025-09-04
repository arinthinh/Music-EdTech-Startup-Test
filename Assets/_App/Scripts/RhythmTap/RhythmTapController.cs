using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MidiPlayerTK;
using UnityEngine;
using UnityEngine.UI;

public class RhythmTapController : MonoBehaviour
{
    [SerializeField] private SongData _songData;
    [SerializeField] private MidiFilePlayer _midiFilePlayer;
    [SerializeField] private NoteFactory _noteFactory;
    [SerializeField] private Transform[] _lanes;
    [SerializeField] private Button _startButton;

    private List<MovingNote> _activeNotes = new();
    [SerializeField] private List<NoteData> _noteDatas = new();

    private void OnEnable()
    {
        _startButton.onClick.AddListener(StartSong);
        EventBus.Subscribe<NoteStartHoldEvent>(OnNoteStartHold);
        EventBus.Subscribe<NoteStopHoldEvent>(OnNoteStopHold);
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(StartSong);
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
    }

    private void SpawnNotes()
    {
        _noteDatas.Clear();

        var midiEvents = _midiFilePlayer.MPTK_MidiEvents;
        foreach (var midiEvent in midiEvents)
        {
            if (midiEvent.Command == MPTKCommand.NoteOn)
            {
                var note = NoteHelper.ConvertMidiNoteToNoteType(midiEvent.Value);
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
        _startButton.gameObject.SetActive(false);
        _midiFilePlayer.MPTK_Play();
    }

    private void Update()
    {
        if (!_midiFilePlayer.MPTK_IsPlaying) return;
        UpdateActiveNotes(_midiFilePlayer.MPTK_MidiLoaded.MPTK_TickPlayer);
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
}