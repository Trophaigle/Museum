using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MIDIPianoPlayer : MonoBehaviour
{
    [Header("Music Data (ScriptableObject)")]
    public MIDIMusicData musicData;

    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Sync")]
    public double visualOffset = 0.02; // ajuste si besoin

    private Dictionary<int, PianoKey> noteToKey = new Dictionary<int, PianoKey>();

    public RuntimeAnimatorController animatorController;

    [Header("Keys")]
    public Transform whiteKeysParent;
    public Transform blackKeysParent;

    public List<GameObject> whiteKeys = new List<GameObject>();
    public List<GameObject> blackKeys = new List<GameObject>();

    [Header("# ou b")]
    public bool useFlats = true;

    public int lowestNote = 21;  // A0
    public int highestNote = 108; // C8

    private Coroutine playCoroutine;

    public UnityEvent onMusicFinished;


    // =========================
    // INIT
    // =========================
    void Start()
    {
        if (musicData == null)
        {
            Debug.LogError("Pas de MidiMusicData assigné !");
            return;
        }
    }

    // =========================
    // PLAY
    // =========================
   public void PlayMusic()
    {
        BuildMapping();

        // si déjà en train de jouer, stoppe d'abord
        if (playCoroutine != null)
            StopMusic();

        playCoroutine = StartCoroutine(PlayMusicWithDelay());
    }

    public void StopMusic()
{
    // arrête la coroutine
    if (playCoroutine != null)
    {
        StopCoroutine(playCoroutine);
        playCoroutine = null;
    }

    // arrête l'audio
    if (audioSource.isPlaying)
        audioSource.Stop();

    // reset touches du piano si nécessaire
   /* foreach (var key in allKeys)
    {
        key.Release(); // à adapter selon ton code
    }*/

    //Debug.Log("Music stopped!");
}

    IEnumerator PlayMusicWithDelay()
    {
        for (int i = 1; i > 0; i--)
        {
            Debug.Log($"Starting in {i}...");
            yield return new WaitForSeconds(1f);
        }

        StartCoroutine(PlayAudioAndMIDI());
    }

    IEnumerator PlayAudioAndMIDI()
    {
        var musicArray = musicData.music;

        double startDspTime = AudioSettings.dspTime + 0.1;

        audioSource.clip = musicData.audioClip;
        // utile seulement si pas streaming
        #if UNITY_WEBGL
        audioSource.clip.LoadAudioData();
        #endif

        audioSource.PlayScheduled(startDspTime);

        double lastNoteEndTime = startDspTime;

        foreach (var group in musicArray)
        {
            double noteTime = startDspTime + group.time - visualOffset;

            // attente précise
            while (AudioSettings.dspTime < noteTime){
                if(!audioSource.isPlaying)
                    yield break;

                yield return null;
            }

            // 🔥 construire la ligne de debug
           // string notesStr = "";

            foreach (var note in group.notes)
            {
                if(!audioSource.isPlaying)
                    yield break;

                PianoKey key = GetKey(note.noteNumber);
                key.Press(noteTime, note.duration);

                //notesStr += $"{NoteName(note.noteNumber)}({note.duration:F2}s) ";
            }

            // 🎯 afficher UNE seule ligne pour tout le groupe
            //Debug.Log($"t={group.time:F2}s → {notesStr}");
        }
        
        while (AudioSettings.dspTime < lastNoteEndTime)
            yield return null;

        onMusicFinished?.Invoke();

        playCoroutine = null;
        
        Debug.Log("MIDI playback finished!");
    }

    string NoteName(int noteNumber)
    {
        string[] sharpNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        string[] flatNames  = { "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B" };

        string[] names = useFlats ? flatNames : sharpNames;

        return names[noteNumber % 12] + (noteNumber / 12 - 1); //Combine le nom de la note avec le numéro d’octave
    }

    public void BuildMapping()
    {
        noteToKey.Clear();

        for (int n = lowestNote; n <= highestNote; n++)
        {
            GameObject keyObj = GetKeyGameObjectFromNoteNumber(n);

            if (keyObj != null)
            {
                PianoKey key = keyObj.GetComponent<PianoKey>();
                noteToKey[n] = key;
            }
        }
    }

    private GameObject GetKeyGameObjectFromNoteNumber(int noteNumber)
    {
        bool isWhite = IsWhiteKey(noteNumber);

        if (isWhite)
        {
            int index = GetWhiteKeyIndex(noteNumber);
            if (index >= 0 && index < whiteKeys.Count)
                return whiteKeys[index];
        }
        else
        {
            int index = GetBlackKeyIndex(noteNumber);
            if (index >= 0 && index < blackKeys.Count)
                return blackKeys[index];
        }

        return null;
    }

    int GetWhiteKeyIndex(int noteNumber)
    {
        int count = 0;

        for (int i = lowestNote; i < noteNumber; i++)
        {
            if (IsWhiteKey(i))
                count++;
        }

        return count;
    }

    int GetBlackKeyIndex(int noteNumber)
    {
        int count = 0;

        for (int i = lowestNote; i < noteNumber; i++)
        {
            if (!IsWhiteKey(i))
                count++;
        }

        return count;
    }

    bool IsWhiteKey(int noteNumber)
    {
        int pos = noteNumber % 12;
        return pos == 0 || pos == 2 || pos == 4 || pos == 5 || pos == 7 || pos == 9 || pos == 11;
    }

    public PianoKey GetKey(int noteNumber)
    {
        if (noteToKey.TryGetValue(noteNumber, out var key))
            return key;

        return null;
    }

       public void SetupKeys()
    {
        if(whiteKeysParent == null || blackKeysParent == null)
        {
            Debug.LogError("Assigne les parents des touches !");
            return;
        }

        //récupère les touches
        // Auto-remplissage par ordre des enfants
        whiteKeys = new List<GameObject>();
        blackKeys = new List<GameObject>();

        foreach (Transform t in whiteKeysParent){
           SetupKey(t.gameObject);
           whiteKeys.Add(t.gameObject);
        }
         

        foreach (Transform t in blackKeysParent){
            SetupKey(t.gameObject);
            blackKeys.Add(t.gameObject);
        }
    }

    /*Ajoute scripts nécessaires et complete les champs*/
    private void SetupKey(GameObject key)
    {
        // Animator
        Animator animator = key.GetComponent<Animator>();
        if(animator == null)
        {
            animator = key.AddComponent<Animator>();
        }

        //Assign le controller
        if(animatorController != null)
        {
            animator.runtimeAnimatorController = animatorController;
        }

        //Piano key script
        if(key.GetComponent<PianoKey>() == null)
        {
            key.AddComponent<PianoKey>();
        }
    } 

}
