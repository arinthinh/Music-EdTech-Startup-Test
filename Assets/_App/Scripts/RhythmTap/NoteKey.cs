using System;
using UnityEngine;

public class NoteKey : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    
    [field: SerializeField] public NoteType Key { get; private set; }

    private void OnMouseDown()
    {
        _renderer.color = Color.gray;
    }
    
    private void OnMouseUp()
    {
        _renderer.color = Color.white;
    }
}
 
