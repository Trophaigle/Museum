using System.Collections;
using System.Linq;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;

public class MidiNoteTestBasic : MonoBehaviour
{
   public TextAsset midiFile;      // Fichier MIDI à tester
    public GameObject key;          // La touche que tu veux animer
    public float pressAngle = 20f;
    public float pressSpeed = 10f;

    void Start()
{
    if (midiFile == null)
    {
        Debug.LogError("Pas de MIDI !");
        return;
    }

    var midi = MidiFile.Read(new System.IO.MemoryStream(midiFile.bytes));
    var tempoMap = midi.GetTempoMap();

    var notes = midi.GetNotes();

    // Grouper les notes qui arrivent en même temps
    var groupedNotes = notes
        .GroupBy(n => n.Time)
        .OrderBy(g => g.Key);

    Debug.Log("===== NOTES MIDI =====");

    int index = 1;

    foreach (var group in groupedNotes)
    {
        // temps en secondes
        float timeSec = (float)TimeConverter
            .ConvertTo<MetricTimeSpan>(group.Key, tempoMap)
            .TotalSeconds;

        string chord = "";

        foreach (var note in group)
        {
            float duration = (float)TimeConverter
                .ConvertTo<MetricTimeSpan>(note.Length, tempoMap)
                .TotalSeconds;

            chord += $"{note}({duration:F2}s) ";
        }

        Debug.Log($"({index}) t={timeSec:F2}s → {chord}");
        index++;
    }
}

    IEnumerator TestPressKey(float delay, float duration)
    {
        // attendre le temps de début
        yield return new WaitForSeconds(delay);

        Quaternion startRot = key.transform.localRotation;
        Quaternion downRot = startRot * Quaternion.Euler(pressAngle, 0f, 0f);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * pressSpeed;
            key.transform.localRotation = Quaternion.Slerp(startRot, downRot, t);
            yield return null;
        }

        // rester enfoncé le temps de la note
        yield return new WaitForSeconds(duration);

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * pressSpeed;
            key.transform.localRotation = Quaternion.Slerp(downRot, startRot, t);
            yield return null;
        }

        Debug.Log("Test terminé !");
    }
}
