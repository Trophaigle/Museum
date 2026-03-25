using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using UnityEngine;

public class MidiNoteTestDataBase : MonoBehaviour
{
    [Header("Fichier MIDI")]
    public TextAsset midiFile;          // ton fichier MIDI (.bytes)
    public AudioSource audioSource;

    [Header("Animation")]
    public float pressAngle = 1.5f;
    public float pressSpeed = 10f;

    [Header("# ou b")]
    public bool useFlats = true;

    public class NoteGroup
    {
        public float time;
        public List<NoteData> notes = new List<NoteData>();
    }

    string NotesToString(List<NoteData> notes)
{
    string result = "";
    foreach (var note in notes)
    {
        result += $"{NoteName(note.noteNumber)}({note.duration:F2}s) "; // transforme le numéro MIDI en nom + durée (C4 duration1, D#3… duration2)
    }
    return result.Trim(); // enlève l’espace final
}

    public class NoteData
    {
        public int noteNumber;
        public float duration;
    }

    public List<NoteGroup> music = new List<NoteGroup>();

  void Start()
    {
        if (midiFile == null)
        {
            Debug.LogError("Pas de MIDI !");
            return;
        }

        var midi = MidiFile.Read(new System.IO.MemoryStream(midiFile.bytes));
        var tempoMap = midi.GetTempoMap(); //contient changement de tempo

        var notes = midi.GetNotes(); //ICollection<Note> with Note having attributs such as Time, length, noteNumber (midi number), velocity (a prendre en compte dans le futur peut etre)

        // Grouper les notes qui arrivent en même temps
        var groupedNotes = notes
            .GroupBy(n => n.Time) //group notes played at the same time (1 or more notes) => (t1, note1 note2), (t2, note3 note4), ...
            .OrderBy(g => g.Key); //sort note groups by ascending Time (order with key=Time) => (t1 < t2, note1 note2), (t2, note 3 note4), ...

        //collection group trié: IOrderedEnumerable<IGrouping<TimeType, Note>>

        music.Clear();

        Debug.Log("===== NOTES MIDI =====");

        int index = 1;

        foreach (var group in groupedNotes)
        {
            // temps en secondes
            float timeSec = (float)TimeConverter //temps en secondes correspondant au tick MIDI
                .ConvertTo<MetricTimeSpan>(group.Key, tempoMap) //group.key position en ticks
                .TotalSeconds;
            //ex: group.key = 96 ticks
            //tempo = 120 bpm

            //ConvertTo -> 1 seconde
            //TotalSeconds -> 1.0
            //timeSec -> 1.0f (float)

            NoteGroup noteGroup = new NoteGroup();
            noteGroup.time = timeSec;

            string chord = "";

            foreach (var note in group)
            {
                //duration in second
                float duration = (float)TimeConverter
                    .ConvertTo<MetricTimeSpan>(note.Length, tempoMap)
                    .TotalSeconds;

                NoteData noteData = new NoteData()
                {
                    noteNumber = note.NoteNumber,
                    duration = duration
                };

                noteGroup.notes.Add(noteData);

                chord += $"{NoteName(note.NoteNumber)}({duration:F2}s) "; //duration:F2 afficher durée avec 2 chiffres après la virgule  
            }

            music.Add(noteGroup);
            Debug.Log($"({index}) t={noteGroup.time:F2}s -> {NotesToString(noteGroup.notes)}");
            
            //Debug.Log($"({index}) t={timeSec:F2}s → {chord}");
            index++;
        }
        Debug.Log("MIDI chargé dans la base !");

       // PlayMusic();
    }

    /* Convertit un numéro de note MIDI (0–127) en un nom musical lisible (C4, F#3, etc.)
        en choisissant entre dièses ou bémols selon useFlats
    */
    string NoteName(int noteNumber)
    {
        string[] sharpNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        string[] flatNames  = { "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B" };

        string[] names = useFlats ? flatNames : sharpNames;

        return names[noteNumber % 12] + (noteNumber / 12 - 1); //Combine le nom de la note avec le numéro d’octave
    }
}
