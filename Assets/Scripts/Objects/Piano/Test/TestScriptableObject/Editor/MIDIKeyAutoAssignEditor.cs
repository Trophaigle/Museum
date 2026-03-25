using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MIDIPianoPlayer))]
public class MIDIKeyAutoAssignEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var script = (MIDIPianoPlayer)target;

         if (GUILayout.Button("Auto Assign and configure keys"))
        {
            //contruire musique
            script.SetupKeys();
            EditorUtility.SetDirty(script); // marque l'objet comme modifié
            //Debug.Log("✅ Music Data Build");
        }
    }
}
