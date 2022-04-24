using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptableObjectConsumable), true)]
public class CustomEditorConsumable : Editor
{
    ScriptableObjectConsumable script;
    SerializedProperty ammountChange;
    SerializedProperty objectColor;


    private void OnEnable()
    {
        script = (ScriptableObjectConsumable)target;
        ammountChange = serializedObject.FindProperty("_ammountToChange");
        objectColor = serializedObject.FindProperty("objectColor");

    }


    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Object Settings", EditorStyles.boldLabel);
        script.type = (ScriptableObjectConsumable.ObjectType)EditorGUILayout.EnumPopup("Object Type", script.type);
        
        switch (script.type)
        {
            case ScriptableObjectConsumable.ObjectType.special:
                script.special = (ScriptableObjectConsumable.SpecialType)EditorGUILayout.EnumPopup("Special Type", script.special);
                break;
            default:
                EditorGUILayout.PropertyField(ammountChange);
                break;
        }

        EditorGUILayout.PropertyField(objectColor);
        serializedObject.ApplyModifiedProperties();
    }
}
