using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "NewMidiMusic", menuName = "MIDI/Music Data")]
public class MIDIMusicData : ScriptableObject
{
    public List<NoteGroup> music = new List<NoteGroup>();

    [Header("Audio Clip")]
    public AudioClip audioClip;
}

[System.Serializable]
public class NoteGroup
{
    public float time;
    public List<NoteData> notes = new List<NoteData>();
}

[System.Serializable]
public class NoteData
{
    public int noteNumber;
    public float duration;
}
