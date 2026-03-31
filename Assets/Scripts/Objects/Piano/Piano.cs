using System;
using TMPro;
using UnityEngine;

public class Piano : InteractableAudioBase
{
    protected override void Play()
    {
        GameManager.Instance.StartPlayPiano();
    }

    protected override void Stop()
    {
        GameManager.Instance.StopPlayPiano();
    }

    protected override bool IsPlaying()
    {
        return GameManager.Instance.isPianoPlaying;
    }

    public void MarkMusicFinished()
    {
        GameManager.Instance.isPianoPlaying = false;
        isPlaying = false;
        textMeshPro.text = displayText;
        Debug.Log("Music finished");
    }
}
