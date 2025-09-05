using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MidiPlayerTK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Act as a main controller for the Rhythm Tap game mode.
/// </summary>
public class RhythmTapScreen : UIScreen
{
    [Header("CONFIGS")]
    [SerializeField] private SongData _songData;

    [Header("CONTROLLERS")]
    [SerializeField] private MidiFilePlayer _midiFilePlayer;
    [SerializeField] private NoteFactory _noteFactory;

    [Header("OBJECTS")]
    [SerializeField] private RectTransform _contentPanel;
    [SerializeField] private Transform[] _lanes;

    [Header("UI OBJECTS")]
    [SerializeField] private Button _startButton;
    [SerializeField] private TextMeshProUGUI _scoreTMP;

    private float _hitOffset;
    private bool _isPlaying;
    private double _lastTick;
    private int _score;
    private int _streakCount;
    private int _modifiedBMP;

    private readonly List<MovingNote> _activeNotes = new();
    private readonly List<NoteData> _noteDatas = new();

    private void OnEnable()
    {
        _startButton.onClick.AddListener(HandlePlayButtonClicked);
        NoteButton.OnNoteButtonPressed += HandleNoteButtonPressed;
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(HandlePlayButtonClicked);
        NoteButton.OnNoteButtonPressed -= HandleNoteButtonPressed;
    }

    public override void OnInit(UIManager uiManager)
    {
        base.OnInit(uiManager);
    }

    public override void Show()
    {
        base.Show();
        _contentPanel.gameObject.SetActive(false);
        ShowScreenAsync().Forget();
    }

    private async UniTask ShowScreenAsync()
    {
        await InitSong();
        SpawnNotes();
        PlayShowAnimation();
    }

    private async UniTask InitSong()
    {
        _midiFilePlayer.MPTK_MidiIndex = _songData.MidiIndex;
        _midiFilePlayer.MPTK_Load();
        await UniTask.Yield();
        _midiFilePlayer.MPTK_EnableChangeTempo = true;
        _midiFilePlayer.MPTK_Tempo = _songData.BPM;
        _lastTick = _midiFilePlayer.MPTK_TickLast;
    }

    private void SpawnNotes()
    {
        _noteDatas.Clear();

        // Load midi data to note data
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

        // Spawn notes
        foreach (var noteData in _noteDatas)
        {
            var laneIndex = (int)noteData.NoteType - 1;
            var laneTransform = _lanes[laneIndex];
            var note = _noteFactory.Get();
            note.OnSpawn(noteData, laneTransform);
            _activeNotes.Add(note);
            note.UpdatePosition(0, _songData.BPM / 160f);
        }
    }

    private void PlayShowAnimation()
    {
        _contentPanel.gameObject.SetActive(true);
        _contentPanel.DOAnchorPosX(2000, 0.5f)
            .From()
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _startButton.gameObject.SetActive(true);
                _startButton.interactable = true;
                _startButton.image.rectTransform.DOAnchorPosY(1000, 0)
                    .From()
                    .SetEase(Ease.OutBack);
            });
    }

    private void Update()
    {
        if (!_isPlaying) return;
        if (!_midiFilePlayer.MPTK_IsPlaying) return;

        var currentTick = _midiFilePlayer.MPTK_MidiLoaded.MPTK_TickPlayer;
        UpdateActiveNotes(currentTick);

        _midiFilePlayer.MPTK_Tempo = _songData.BPM + _modifiedBMP;
        if (currentTick >= _lastTick) HandleEndSong();
    }

    private void UpdateActiveNotes(double currentTick)
    {
        foreach (var note in _activeNotes.ToArray())
        {
            // Move note
            note.UpdatePosition(currentTick, _songData.BPM / 160f);

            // Check for miss
            if (!note.IsScored && note.CurrentPositionX <= -170)
            {
                if (_streakCount > 0)
                {
                    _modifiedBMP = 0;
                    _streakCount = 0;
                }
                _streakCount--;
                if (_streakCount < -3 && _modifiedBMP > -30)
                {
                    _modifiedBMP -= 10;
                }

                note.SetScored(false);
            }

            // If note is out of screen, remove it
            if (note.CurrentPositionX <= -500)
            {
                _activeNotes.Remove(note);
                _noteFactory.Release(note);
            }
        }
    }

    private void ClearLevel()
    {
        _score = 0;
        _scoreTMP.text = "0";
        foreach (var note in _activeNotes)
        {
            _noteFactory.Release(note);
        }
        _activeNotes.Clear();
        _noteDatas.Clear();
    }

    private void HandleEndSong()
    {
        _isPlaying = false;
        ClearLevel();
        Show();
    }

    private void PlayHideAnimation()
    {
    }

    private void HandlePause()
    {
        _midiFilePlayer.MPTK_Stop();
    }

    private void HandlePlayButtonClicked()
    {
        _startButton.gameObject.SetActive(false);
        _isPlaying = true;
        _midiFilePlayer.MPTK_Play();
    }

    private void HandleNoteButtonPressed(NoteType noteType)
    {
        if (!_isPlaying) return;

        foreach (var note in _activeNotes)
        {
            if (note.IsScored) continue;
            if (note.CurrentPositionX is < 40 and > -100)
            {
                // Hit
                if (noteType == note.NoteType)
                {
                    note.SetScored(true);
                    _score += 100;

                    if (_streakCount < 0)
                    {
                        _modifiedBMP = 0;
                        _streakCount = 0;
                    }
                    _streakCount++;
                    if (_streakCount > 3 && _modifiedBMP < 30)
                    {
                        _modifiedBMP += 10;
                    }
                    
                    _scoreTMP.text = _score.ToString();
                    return;
                }
                else
                {
                    // Wrong key
                    return;
                }
            }
        }
    }
}