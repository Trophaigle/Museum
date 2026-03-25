using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SubtitleClip : PlayableAsset
{
    [TextArea]
    public string text;

    public float fadeIn = 0.2f;
    public float fadeOut = 0.2f;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SubtitleBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour.text = text;

        behaviour.fadeIn = fadeIn;
        behaviour.fadeOut = fadeOut;
        
        return playable;
    }
}
