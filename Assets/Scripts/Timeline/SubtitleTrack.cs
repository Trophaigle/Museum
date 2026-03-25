using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackClipType(typeof(SubtitleClip))]
[TrackBindingType(typeof(TextMeshProUGUI))]
public class SubtitleTrack : TrackAsset
{
     public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<SubtitleMixerBehaviour>.Create(graph, inputCount);
    }
}
