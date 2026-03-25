using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using UnityEngine;
using UnityEngine.InputSystem;

public class MidiNoteTestPlayPianoWithAnim : MonoBehaviour
{
    [Header("Fichier MIDI")]
    public TextAsset midiFile;          // ton fichier MIDI (.bytes)
    public AudioSource audioSource;

   /* [Header("Animation")]
    public float pressAngle = 1.5f;
    public float pressSpeed = 8f;*/

    [Header("# ou b")]
    public bool useFlats = true;

    [Header("Keys")]
    public Transform whiteKeysParent; // parent qui contient toutes les touches blanches
    public Transform blackKeysParent; // parent qui contient toutes les touches noires
    public List<GameObject> whiteKeys;
    public List<GameObject> blackKeys;

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
        public PianoKey pianoKey;
    }

    [SerializeField]
    public List<NoteGroup> music = new List<NoteGroup>();

    //cache: à un noteNumber (MIDI) associe un GameObject (piano key)
    private Dictionary<int, PianoKey> noteToKey = new Dictionary<int, PianoKey>();
    public int lowestNote = 21; // ex: A0 (début du piano)
    public int highestNote = 108; //ex: C8

    public RuntimeAnimatorController animatorController;

    string NotesToString(List<NoteData> notes)
    {
        string result = "";
        foreach (var note in notes)
        {
            result += NoteName(note.noteNumber) + " "; // transforme le numéro MIDI en nom (C4, D#3…)
        }
        return result.Trim(); // enlève l’espace final
    }

    void BuildMapping() //construire le cache
    {
        for (int n = lowestNote; n <= highestNote; n++)
        {
            noteToKey[n] = GetKeyGameObjectFromNoteNumber(n).GetComponent<PianoKey>();
        }
    }

    public void BuildMusicData()
    {
        BuildMapping();

        if (midiFile == null)
        {
            Debug.LogError("Pas de MIDI !");
            return;
        }

        //Read MIDI
        var midi = MidiFile.Read(new System.IO.MemoryStream(midiFile.bytes)); //fichier.bytes
        var tempoMap = midi.GetTempoMap(); //contient changement de tempo

        var notes = midi.GetNotes(); //ICollection<Note> with Note having attributs such as Time, length, noteNumber (midi number), velocity (a prendre en compte dans le futur peut etre)

        // Grouper les notes qui arrivent en même temps
        var groupedNotes = notes
            .GroupBy(n => n.Time) //group notes played at the same time (1 or more notes) => (t1, note1 note2), (t2, note3 note4), ...
            .OrderBy(g => g.Key); //sort note groups by ascending Time (order with key=Time) => (t1 < t2, note1 note2), (t2, note 3 note4), ...

        //collection group trié: IOrderedEnumerable<IGrouping<TimeType, Note>>

        music.Clear();

        Debug.Log("===== NOTES MIDI =====");

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

            foreach (var note in group)
            {
                //duration in second
                float duration = (float)TimeConverter
                    .ConvertTo<MetricTimeSpan>(note.Length, tempoMap)
                    .TotalSeconds;

                NoteData noteData = new NoteData()
                {
                    noteNumber = note.NoteNumber,
                    duration = duration,
                    pianoKey = noteToKey[note.NoteNumber]
                };

                noteGroup.notes.Add(noteData);
            }

            music.Add(noteGroup);
        }
        Debug.Log("MIDI chargé dans la base !");
    }

    private GameObject GetKeyGameObjectFromNoteNumber(int noteNumber)
    {

        bool isWhite = IsWhiteKey(noteNumber);

        if (isWhite)
        {
            int index = GetWhiteKeyIndex(noteNumber);
            return whiteKeys[index];
        }
        else
        {
            int index = GetBlackKeyIndex(noteNumber);
            return blackKeys[index];
        }
    }

    private int GetWhiteKeyIndex(int noteNumber)
    {
        int count = 0;

        for (int i = lowestNote; i < noteNumber; i++)
        {
            if (IsWhiteKey(i))
                count++;
        }

        return count;
    }

    private int GetBlackKeyIndex(int noteNumber)
    {
        int count = 0;

        for (int i = lowestNote; i < noteNumber; i++)
        {
            if (!IsWhiteKey(i))
                count++;
        }

        return count;
    }

    private bool IsWhiteKey(int noteNumber)
    {
        int pos = noteNumber % 12;
        return pos == 0 || pos == 2 || pos == 4 || pos == 5 || pos == 7 || pos == 9 || pos == 11;
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

    public void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        StartCoroutine(PlayMusicWithDelay());
    }

    IEnumerator PlayMusicWithDelay()
    {
        for (int i = 5; i > 0; i--)
        {
            Debug.Log($"Starting in {i}...");
            yield return new WaitForSeconds(1f);
        }

        StartCoroutine(PlayAudioAndMIDI());
    }

    // Coroutine pour jouer les notes du MIDI avec synchro audio
    IEnumerator PlayAudioAndMIDI()
    {
        double visualOffset = 0.02;

        // Convertit la liste en array pour accès plus rapide
        NoteGroup[] musicArray = music.ToArray();

        // 1️⃣ Récupère le temps DSP de départ
        double startDspTime = AudioSettings.dspTime;
        Debug.Log($"Start playing MIDI and audio at DSP time: {startDspTime:F3}s");

        // 2️⃣ Joue le fichier audio WAV exactement au temps DSP de départ
        audioSource.PlayScheduled(startDspTime);

        // 3️⃣ Parcours chaque groupe de notes
        for (int index = 0; index < musicArray.Length; index++)
        {
            var group = musicArray[index];

            // 4️⃣ Calcul du moment exact DSP de cette note
            double notePlayTime = startDspTime + group.time /*- visualOffset*/;

            // 5️⃣ Attente active jusqu'au moment exact du DSP
            /*while (AudioSettings.dspTime < notePlayTime)
            {
                yield return null;
            }*/
            double delay = notePlayTime - AudioSettings.dspTime;
            if(delay > 0)
            {
                yield return new WaitForSeconds((float) delay);
            }

            // 6️⃣ Parcours chaque note du groupe
            foreach (var note in group.notes)
            {
                // Affiche la note jouée avec son temps et sa durée
                Debug.Log($"Play: {NoteName(note.noteNumber)}({note.duration:F2}s) at t={group.time:F2}s");

                // Joue l'animation de la touche synchronisée
                note.pianoKey.Press(notePlayTime, note.duration);
            }
        }

        // 7️⃣ Optionnel : attendre la fin de la dernière note pour terminer proprement
        double lastNoteEndTime = startDspTime;
        foreach (var group in musicArray)
        {
            foreach (var note in group.notes)
            {
                double noteEnd = startDspTime + group.time + note.duration;
                if (noteEnd > lastNoteEndTime)
                    lastNoteEndTime = noteEnd;
            }
        }
        while (AudioSettings.dspTime < lastNoteEndTime)
            yield return null;

        Debug.Log("MIDI playback finished!");
    }
}
