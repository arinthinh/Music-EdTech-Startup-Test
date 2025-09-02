using Cysharp.Threading.Tasks;
using MidiPlayerTK;
using UnityEngine;

public class RhythmTapController : MonoBehaviour
{
    [SerializeField] private MidiFilePlayer _midiFilePlayer;
    
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
        
        StartSong();
    }

    private void StartSong()
    {
        _midiFilePlayer.MPTK_Play();
        Debug.Log(_midiFilePlayer.MPTK_MidiName);
    }

    private void OnNoteStartHold(NoteStartHoldEvent evt)
    {
    }


    private void OnNoteStopHold(NoteStopHoldEvent obj)
    {
    }
}