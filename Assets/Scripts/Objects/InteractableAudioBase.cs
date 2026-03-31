using TMPro;
using UnityEngine;

public abstract class InteractableAudioBase : InteractableUIBase
{
   public string displayText;
    public string textPlaying;

    protected bool isPlaying = false;

    public override void OnHoverEnter()
    {
        base.OnHoverEnter();
        textMeshPro.text = isPlaying ? textPlaying : displayText;
    }

    public override void OnClick()
    {
        if (!IsPlaying())
        {
            Play();
            isPlaying = true;
            textMeshPro.text = textPlaying;
        }
        else
        {
            Stop();
            isPlaying = false;
            textMeshPro.text = displayText;
        }
    }

    protected abstract void Play();
    protected abstract void Stop();
    protected abstract bool IsPlaying();

    protected virtual void Update()
    {
        if (isPlaying && !IsPlaying())
        {
            isPlaying = false;
            textMeshPro.text = displayText;
            OnMusicFinished();
        }
    }

    protected virtual void OnMusicFinished() {}
}
