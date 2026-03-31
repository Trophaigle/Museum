using TMPro;
using UnityEngine;

public class AudioFrame : InteractableAudioBase
{
     public AudioSource audioSource;

    protected override void Play() => audioSource.Play();
    protected override void Stop() => audioSource.Stop();
    protected override bool IsPlaying() => audioSource.isPlaying;
}
