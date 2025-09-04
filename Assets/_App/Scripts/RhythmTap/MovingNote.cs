using UnityEngine;

public class MovingNote : MonoBehaviour
{
    private NoteData _noteData;
    public float CurrentPositionX => transform.localPosition.x;
    public double NoteTick => _noteData.Tick;

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
        transform.localPosition = new Vector3(offset, 0, 0);
    }
}