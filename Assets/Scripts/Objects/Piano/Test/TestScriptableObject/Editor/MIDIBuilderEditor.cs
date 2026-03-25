using System.Collections.Generic;
using System.Linq;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MIDIAssetBuilder))]
public class MIDIBuilderEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var script = (MIDIAssetBuilder)target;

         if (GUILayout.Button("Build Music Data Asset"))
        {
            //contruire musique
            script.BuildAsset();
            EditorUtility.SetDirty(script); // marque l'objet comme modifié
            //Debug.Log("✅ Music Data Build");
        }
    }
}
