using Cysharp.Threading.Tasks;
using MidiPlayerTK;
using UnityEngine;
using UnityEngine.Pool;

public class RhythmTapController : MonoBehaviour
{
    [SerializeField] private MidiFilePlayer _midiFilePlayer;
    [SerializeField] private Transform[] _lanes;
    
    [SerializeField] private MovingNote _notePrefab;

    private ObjectPool<MovingNote> _notePool;

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
        _midiFilePlayer.MPTK_Load();
        /*_notePool = new(() => Instantiate(_notePrefab, transform),
            obj => obj.gameObject.SetActive(true),
            obj => obj.gameObject.SetActive(false),
            obj => Destroy(obj.gameObject));

        await UniTask.WaitForSeconds(2f);

        StartSong();*/
    }


    private void StartSong()
    {
    }

    private void Update()
    {
        var currentTick = _midiFilePlayer.MPTK_TickCurrent;
    }

    private void OnNoteStartHold(NoteStartHoldEvent evt)
    {
    }


    private void OnNoteStopHold(NoteStopHoldEvent obj)
    {
    }
}