using UnityEngine;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(MidiNoteTestPlayPianoWithAnim))]
public class PianoAutoAssignEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // affiche le reste de l'inspecteur normalement

        MidiNoteTestPlayPianoWithAnim piano = (MidiNoteTestPlayPianoWithAnim)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Auto Assign Keys"))
        {
            SetupKeys(piano);
            EditorUtility.SetDirty(piano); // marque l'objet comme modifié
            Debug.Log("✅ Touches auto assignées et ajout des components PianoKey et Animator");
        }

        if (GUILayout.Button("Build Music Data"))
        {
            //contruire musique
            piano.BuildMusicData();
            EditorUtility.SetDirty(piano); // marque l'objet comme modifié
            Debug.Log("✅ Music Data Build");
        }
    }

    void SetupKeys(MidiNoteTestPlayPianoWithAnim script)
    {
        if(script.whiteKeysParent == null || script.blackKeysParent == null)
        {
            Debug.LogError("Assigne les parents des touches !");
            return;
        }

        //récupère les touches
        // Auto-remplissage par ordre des enfants
        script.whiteKeys = new List<GameObject>();
        script.blackKeys = new List<GameObject>();

        foreach (Transform t in script.whiteKeysParent){
           SetupKey(t.gameObject, script);
           script.whiteKeys.Add(t.gameObject);
        }
         

        foreach (Transform t in script.blackKeysParent){
            SetupKey(t.gameObject, script);
            script.blackKeys.Add(t.gameObject);
        }
    }

    /*Ajoute scripts nécessaires et complete les champs*/
    void SetupKey(GameObject key, MidiNoteTestPlayPianoWithAnim script)
    {
        // Animator
        Animator animator = key.GetComponent<Animator>();
        if(animator == null)
        {
            animator = key.AddComponent<Animator>();
        }

        //Assign le controller
        if(script.animatorController != null)
        {
            animator.runtimeAnimatorController = script.animatorController;
        }

        //Piano key script
        if(key.GetComponent<PianoKey>() == null)
        {
            key.AddComponent<PianoKey>();
        }
    } 
}
#endif
