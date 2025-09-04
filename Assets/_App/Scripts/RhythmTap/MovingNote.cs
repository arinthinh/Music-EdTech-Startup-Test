using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MovingNote : MonoBehaviour
{
    [SerializeField] private NoteSpriteConfigSO _noteSpriteConfigSO;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image _image;

    private NoteData _noteData;

    public NoteType NoteType => _noteData.NoteType;
    public float CurrentPositionX => _rectTransform.anchoredPosition.x;
    public double NoteTick => _noteData.Tick;
    public bool IsScored => _noteData.IsScored;


    public void OnSpawn(NoteData noteData, Transform laneTransform)
    {
        gameObject.SetActive(true);
        _noteData = noteData;
        _noteData.IsSpawned = true;
        _image.sprite = _noteSpriteConfigSO.GetSprite(_noteData.NoteType);
        _image.color = Color.white;
        transform.SetParent(laneTransform, true);
    }

    public void UpdatePosition(double currentTick, float distantPerTick)
    {
        var offset = (float)(_noteData.Tick - currentTick) * distantPerTick;
        _rectTransform.anchoredPosition = new Vector2(offset, 0);
    }

    public void SetScored(bool isHit)
    {
        _noteData.IsScored = true;
        _image.sprite = _noteSpriteConfigSO.GetSprite(NoteType.None);
        _image.DOFade(0, 0.25f);
    }
}