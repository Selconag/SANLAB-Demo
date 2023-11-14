using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(SerializableDictionaryComponent))]
public class SerializableDictionaryComponentEditor : Editor
{
    private SerializedProperty connectionDictionary;

    private void OnEnable()
    {
        connectionDictionary = serializedObject.FindProperty("ConnectionDictionary");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(connectionDictionary, true);

        serializedObject.ApplyModifiedProperties();
    }
}

