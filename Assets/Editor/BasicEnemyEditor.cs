using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BasicEnemy))]
[CanEditMultipleObjects]
public class BasicEnemyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BasicEnemy mp = (BasicEnemy)target;
        ProgressBar(mp.Definition.MaxHealth / 100.0f, "Max Health");
        ProgressBar(mp.Definition.LightAttackDamage / 100.0f, "Light Damage");
        ProgressBar(mp.Definition.SpecialAttackDamage / 100.0f, "Special Damage");
        ProgressBar(mp.Definition.MovementSpeed / 100.0f, "Movement Speed");

        serializedObject.Update();
        base.OnInspectorGUI();        
    }

    // Custom GUILayout progress bar.
    void ProgressBar(float value, string label)
    {
        // Get a rect for the progress bar using the same margins as a textfield:
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        EditorGUI.ProgressBar(rect, value, label);
        EditorGUILayout.Space();
    }
}
