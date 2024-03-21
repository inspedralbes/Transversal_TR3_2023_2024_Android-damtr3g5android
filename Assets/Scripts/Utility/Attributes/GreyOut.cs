using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor; // Editor-specific using directive
#endif

public class GreyOut : PropertyAttribute
{
    // This class intentionally left empty or can contain attribute-specific logic
}

#if UNITY_EDITOR
// Ensure that all editor-specific code is within this block, including the using directives relevant to the editor.
[CustomPropertyDrawer(typeof(GreyOut))]
public class GreyOutDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false; // Disable GUI elements within this block
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true; // Re-enable GUI elements after drawing the property
    }
}
#endif
