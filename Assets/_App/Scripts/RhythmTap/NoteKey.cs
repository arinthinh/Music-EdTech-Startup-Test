using System;
using UnityEngine;

public class NoteKey : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;
    
    [field: SerializeField] public NoteType Key { get; private set; }

    private void Start()
    {
        _audioSource.playOnAwake = false;
        _audioSource.clip = _audioClip;
        _audioSource.loop = false;
    }
    
    private void OnMouseDown()
    {
        _renderer.color = Color.gray;
        _audioSource.Play();
    }
    
    private void OnMouseUp()
    {
        _renderer.color = Color.white;
        _audioSource.Stop();
    }

    private void Reset()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }
}
 
