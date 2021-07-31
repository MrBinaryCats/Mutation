using UnityEditor;
using UnityEngine;

namespace Mutations.Editor
{
    /// <summary>
    ///     Shows a warning box if the field is empty
    /// </summary>
    [CustomPropertyDrawer(typeof(WarnIfEmptyAttribute))]
    public class WarnIfEmptyDrawer : PropertyDrawer
    {
        private bool ShowHelpBox(SerializedProperty property)
        {
            return string.IsNullOrEmpty(property.stringValue);
        }

        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            var helpBoxPos = position;
            var fieldPos = position;

            if (ShowHelpBox(property))
            {
                helpBoxPos.height = EditorGUIUtility.singleLineHeight * 2;
                fieldPos.y = helpBoxPos.yMax;
                fieldPos.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.HelpBox(helpBoxPos, $"{property.displayName} does not have a value. This is not recommended.",
                    MessageType.Warning);
            }

            EditorGUI.PropertyField(fieldPos, property, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * (ShowHelpBox(property) ? 3 : 1);
        }
    }
}