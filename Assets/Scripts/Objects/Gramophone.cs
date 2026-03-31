using TMPro;
using UnityEngine;

public class Gramophone : InteractableAudioBase
{
     public AudioSource audioSource;
    public GameObject lightPoint;

    protected override void Play()
    {
        audioSource.Play();
        lightPoint.SetActive(true);
    }

    protected override void Stop()
    {
        audioSource.Stop();
        lightPoint.SetActive(false);
    }

    protected override bool IsPlaying()
    {
        return audioSource.isPlaying;
    }

    protected override void OnMusicFinished()
    {
        lightPoint.SetActive(false);
        Debug.Log("Music finished");
    }
}
