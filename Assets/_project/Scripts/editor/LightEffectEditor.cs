using System;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(LightEffect))]
public class LightEffectEditor : Editor
{
    private SerializedObject so;
    private SerializedProperty propType;
    private SerializedProperty propCustomString;
    private SerializedProperty propCustomEffect;
    private SerializedProperty propEditorPreview;

    private bool showDefaults = false;
    private void OnEnable()
    {
        so = serializedObject;
        propType = so.FindProperty("effectType");
        propCustomString = so.FindProperty("customString");
        propCustomEffect = so.FindProperty("useCustomEffect");
        propEditorPreview = so.FindProperty("editorPreview");
    }

    public override void OnInspectorGUI()
    {
        LightEffect lightEffect = target as LightEffect;
        
        so.Update();
        GUILayout.Label("Set an effect for the light" );
        EditorGUILayout.PropertyField( propType );
        showDefaults = EditorGUILayout.Foldout(showDefaults, "Available effects", true);
        if (showDefaults)
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Label("Default effects:", EditorStyles.boldLabel);
                EditorGUILayout.Separator();
                foreach (var lightStyle in lightEffect.lightStylesDefault)
                {
                    GUILayout.Label(lightStyle.name);
                }
            }
            
            EditorGUILayout.Separator();
        }
        EditorGUILayout.PropertyField( propCustomEffect );
        if (lightEffect.useCustomEffect)
        {
            propCustomString.stringValue = EditorGUILayout.TextField("Custom string: ", propCustomString.stringValue);
            GUILayout.Label("a = lowest brightness | z = highest brightness | interval = 0.1s" );
            EditorGUILayout.Separator();
        }
        EditorGUILayout.PropertyField( propEditorPreview );
        if (so.ApplyModifiedProperties())
        {
            // if something changed
        }
        if (GUILayout.Button("Refresh effect"))
            lightEffect.StartEffect();
        /*
         Intuitive way of doing this, does not support undo and does not get marked dirty. No multi editing...
        GUILayout.Label("Set an effect for the light" );
        GUILayout.Label("Custom effects:" );

        lightEffect.effectType = EditorGUILayout.TextField("Effect type: ", lightEffect.effectType);
        lightEffect.useCustomEffect = EditorGUILayout.Toggle("Use custom effect: ", lightEffect.useCustomEffect);
        if (lightEffect.useCustomEffect)
        {
            lightEffect.customString = EditorGUILayout.TextField("Custom string: ", lightEffect.customString);
            GUILayout.Label("a = lowest brightness | z = highest brightness | interval = 0.1s" );
        }

        lightEffect.editorPreview = EditorGUILayout.Toggle("Editor preview: ", lightEffect.editorPreview);
        if (GUILayout.Button("Refresh effect"))
        {
            lightEffect.StartEffect();
        }
        */
        // explicit positioning using Rect
        // GUI
        // EditorGUI

        // implicit positioning, auto-layout
        // GUILayout 
        // EditorGUILayout

        // base.OnInspectorGUI();

    }
}
