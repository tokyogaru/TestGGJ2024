using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    [Header("Sfx")]
    [SerializeField] private AudioClip hover;
    [SerializeField] private AudioClip press;

    public void Pressed()
    {
        SoundManager.Instance.PlaySound(press, transform, 1f, 0);
    }

    public void Hover()
    {
        SoundManager.Instance.PlaySound(hover, transform, 1f, 0);
    }
}
