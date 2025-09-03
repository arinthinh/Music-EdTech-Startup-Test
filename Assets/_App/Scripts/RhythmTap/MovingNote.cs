using UnityEngine;

public class MovingNote : MonoBehaviour
{
    private NoteData _noteData;

    private float _speed = 3f;

    public void OnSpawn(NoteData noteData, Transform laneTransform)
    {
        _noteData = noteData;
        transform.SetParent(laneTransform, false);
        var distance = 20;
        transform.localPosition = new Vector3(distance, 0, 0); // Start above the lane
        _speed = distance;
        gameObject.SetActive(true);
    }

    public void Move(double currentSongTime)
    {
        // The time when the note should be hit
        double noteTime = _noteData.Time;

        // The distance from the lane where the note starts
        float startDistance = _speed;

        // Calculate how much time has passed since the note started moving
        float timeToHit = startDistance / _speed; // time to reach 0, but _speed is used as distance, so set duration as needed

        // Calculate the offset based on song time
        float offset = (float)(noteTime - currentSongTime) / 1000f;

        // When currentSongTime == noteTime, offset = 0, so localPosition.x = 0
        // Before noteTime, offset > 0, so localPosition.x > 0
        // After noteTime, offset < 0, so localPosition.x < 0 (keeps moving)
        transform.localPosition = new Vector3(offset * _speed, 0, 0);
    }
}