using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoteButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static event Action<NoteType> OnNoteButtonPressed;

    [SerializeField] private Image _renderer;

    [field: SerializeField] public NoteType Key { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnNoteButtonPressed?.Invoke(Key);
        var sound = Key switch
        {
            NoteType.C => ESound.C,
            NoteType.D => ESound.D,
            NoteType.E => ESound.E,
            NoteType.F => ESound.F,
            NoteType.G => ESound.G,
            NoteType.A => ESound.A,
            NoteType.B => ESound.B,
            _ => ESound.None
        };
        if (sound != ESound.None)
        {
            AudioManager.Instance.PlaySound(sound);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var sound = Key switch
        {
            NoteType.C => ESound.C,
            NoteType.D => ESound.D,
            NoteType.E => ESound.E,
            NoteType.F => ESound.F,
            NoteType.G => ESound.G,
            NoteType.A => ESound.A,
            NoteType.B => ESound.B,
            _ => ESound.None
        };
        if (sound != ESound.None)
        {
            AudioManager.Instance.StopSound(sound);
        }
    }
}