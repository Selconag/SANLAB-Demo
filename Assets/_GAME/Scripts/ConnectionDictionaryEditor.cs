using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(ConnectionDictionary))]
public class ConnectionDictionaryEditor : Editor
{
    private SerializedProperty serializedDictionary;

    private void OnEnable()
    {
        serializedDictionary = serializedObject.FindProperty("ConnectionInformations");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedDictionary, true);

        serializedObject.ApplyModifiedProperties();
    }
}

