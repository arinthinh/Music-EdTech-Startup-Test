using UnityEngine;
using UnityEngine.Pool;

public class NoteFactory : MonoBehaviour
{
    [SerializeField] private MovingNote _notePrefab;
    private ObjectPool<MovingNote> _notePool;

    private void Start()
    {
        _notePool = new(
            () => Instantiate(_notePrefab, transform),
            obj => obj.gameObject.SetActive(true),
            obj => obj.gameObject.SetActive(false),
            obj => Destroy(obj.gameObject));
    }

    public MovingNote Get()
    {
        return _notePool.Get();
    }

    public void Release(MovingNote note)
    {
        _notePool.Release(note);
    }
}