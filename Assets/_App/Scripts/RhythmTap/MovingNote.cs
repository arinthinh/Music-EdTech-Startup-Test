using UnityEngine;

public class MovingNote : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    
    private NoteData _noteData;
    
    public float CurrentPositionX => _rectTransform.anchoredPosition.x;
    public double NoteTick => _noteData.Tick;
    public bool IsScored => _noteData.IsScored;

    public void OnSpawn(NoteData noteData, Transform laneTransform)
    {
        gameObject.SetActive(true);
        _noteData = noteData;
        _noteData.IsSpawned = true;
        transform.SetParent(laneTransform, true);
        UpdatePosition(0);
    }

    public void UpdatePosition(double currentTick)
    {
        var offset = (float)(_noteData.Tick - currentTick) * Define.DISTANCE_PER_TICK;
        _rectTransform.anchoredPosition = new Vector3(offset, _rectTransform.anchoredPosition.y);
    }

    public void SetScored(bool isHit)
    {
        _noteData.IsScored = true;
    }
}