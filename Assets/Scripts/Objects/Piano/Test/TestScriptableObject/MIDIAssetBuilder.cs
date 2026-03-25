using System.Collections.Generic;
using System.Linq;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class MIDIAssetBuilder : MonoBehaviour
{
    [Header("MIDI file")]
    public TextAsset midiFile;  

    [Header("Audio")]
    public AudioClip audioClip;

    public void BuildAsset()
    {
        if (midiFile == null)
        {
            Debug.LogError("Pas de MIDI !");
            return;
        }

        var midi = MidiFile.Read(new System.IO.MemoryStream(midiFile.bytes));
        var tempoMap = midi.GetTempoMap();
    

       //afficher tempo pour voir, j'ai l'impression qu'il est plus rapide que l'audio

        var notes = midi.GetNotes();

        var groupedNotes = notes
            .GroupBy(n => n.Time)
            .OrderBy(g => g.Key);

        MIDIMusicData data = ScriptableObject.CreateInstance<MIDIMusicData>();

        data.audioClip = audioClip;

        foreach (var group in groupedNotes)
        {
            float timeSec = (float)TimeConverter
                .ConvertTo<MetricTimeSpan>(group.Key, tempoMap)
                .TotalSeconds;

            NoteGroup noteGroup = new NoteGroup();
            noteGroup.time = timeSec;

            foreach (var note in group)
            {
                float duration = (float)TimeConverter
                    .ConvertTo<MetricTimeSpan>(note.Length, tempoMap)
                    .TotalSeconds;

                noteGroup.notes.Add(new NoteData
                {
                    noteNumber = note.NoteNumber,
                    duration = duration,
                });
            }

            data.music.Add(noteGroup);
        }
        #if UNITY_EDITOR
        string folderPath = "Assets/MIDI";

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets", "MIDI");
        }

        string assetPath = folderPath + "/" + midiFile.name + ".asset";
        assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

        AssetDatabase.CreateAsset(data, assetPath);

        AssetDatabase.SaveAssets();
        #endif

        Debug.Log("✅ Asset MIDI créé !");
    }
}