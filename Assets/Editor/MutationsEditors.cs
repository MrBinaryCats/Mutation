using System;
using Mutations.Mutations;
using Mutations.Mutations.Core;
using UnityEditor;
using UnityEngine;

namespace Mutations.Editor
{
    //Define custom editor for any Mutations which match the inheritance Mutations<Color, MeshRenderer>
    [CustomEditor(typeof(Mutations<Color, MeshRenderer>), true)]
    //Ensure to inherit from MutationsEditor which does all the heavy lifting 
    public class ColorEditor : MutationsEditor
    { 
        protected override string[] ExcludedProperties => new []
        {
            "m_Script" , "DefaultValue", "DefaultIndex", "Values",
            "ColorPropName", "ColorPropID"
        };
    };
    //Define a custom editor for any Mutations inheriting from StrengthMutation
    [CustomEditor(typeof(StrengthMutation), true)]
    //Ensure to inherit from MutationsEditor which does all the heavy lifting 
    public class StrengthEditor : MutationsEditor
    { };
    
    /// <summary>
    /// Main Editor class for drawing Mutations
    /// </summary>
    public class MutationsEditor : UnityEditor.Editor
    {
        private SerializedProperty _defaultValueProp, _defaultIndexProp, _valuesProp, _nameProp, _IDProp;
        private string[] _excludedProperties;
        
        protected virtual string[] ExcludedProperties => new []
        {
            "m_Script" , "DefaultValue", "DefaultIndex", "Values"
        };
        public void OnEnable()
        {
            _defaultValueProp = serializedObject.FindProperty("DefaultValue");
            _defaultIndexProp = serializedObject.FindProperty("DefaultIndex");
            _valuesProp = serializedObject.FindProperty("Values");
            _nameProp = serializedObject.FindProperty("ColorPropName");
            _IDProp = serializedObject.FindProperty("ColorPropID");
            _excludedProperties = ExcludedProperties;
        }
        public override void OnInspectorGUI()
        {
            using (var masterCheck = new EditorGUI.ChangeCheckScope())
            {
                //Draw the editor bar the properties we want to manually control
                DrawPropertiesExcluding(serializedObject, _excludedProperties );

                if (_nameProp!= null)
                {
                    using (var check = new EditorGUI.ChangeCheckScope())
                    {
                        //When the user enters the property name, serialise the ID of that property
                        //Maybe not a good idea to do this at time of serialisation
                        EditorGUILayout.DelayedTextField(_nameProp);
                        if (check.changed)
                            _IDProp.intValue = Shader.PropertyToID(_nameProp.stringValue);
                    }

                }
  

                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    using (new EditorGUI.DisabledScope(_valuesProp.arraySize == 0))
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.PropertyField(_defaultIndexProp);
                            //We want to show the user the color, but not allow them to edit this field 
                            using (new EditorGUI.DisabledScope(true))
                            {
                                var tmp  = EditorGUIUtility.labelWidth;
                                EditorGUIUtility.labelWidth = 0;
                                EditorGUILayout.PropertyField(_defaultValueProp, new GUIContent());
                                EditorGUIUtility.labelWidth = tmp;
                            }
                        }
                    }

                    //draw the array of values
                    EditorGUILayout.PropertyField(_valuesProp);
                    if (check.changed)
                    {
                        //only allow valid indices of the array
                        _defaultIndexProp.intValue =
                            Mathf.Clamp(_defaultIndexProp.intValue, 0, _valuesProp.arraySize - 1);
                        //serialise the default colour encase the user wants to access that property
                        CopyValueToDefault();
                    }
                }

                if (masterCheck.changed)
                {
                    //save the changed values
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }

        /// <summary>
        /// Copies the the value of the array at the default index value, to the default value
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void CopyValueToDefault()
        {
            //Because the class is generic there is no way of knowing which data you want to copy across
            //we do know that the array type and the default value type is the same
            //check the SerializedPropertyType and use that to determin what field should be copied over
            switch (_defaultValueProp.propertyType)
            {
                case SerializedPropertyType.Integer:
                case SerializedPropertyType.LayerMask:
                case SerializedPropertyType.Character:
                    _defaultValueProp.intValue = _valuesProp.arraySize == 0
                        ? 0
                        : _valuesProp.GetArrayElementAtIndex(_defaultIndexProp.intValue).intValue;
                    break;
                case SerializedPropertyType.Boolean:
                    _defaultValueProp.boolValue = _valuesProp.arraySize == 0
                        ? false
                        : _valuesProp.GetArrayElementAtIndex(_defaultIndexProp.intValue).boolValue;
                    break;
                case SerializedPropertyType.Float:
                    _defaultValueProp.floatValue = _valuesProp.arraySize == 0
                        ? 0
                        : _valuesProp.GetArrayElementAtIndex(_defaultIndexProp.intValue).floatValue;
                    break;
                case SerializedPropertyType.String:
                    _defaultValueProp.stringValue = _valuesProp.arraySize == 0
                        ? string.Empty
                        : _valuesProp.GetArrayElementAtIndex(_defaultIndexProp.intValue).stringValue;
                    break;
                case SerializedPropertyType.Color:
                    _defaultValueProp.colorValue = _valuesProp.arraySize == 0
                        ? Color.black
                        : _valuesProp.GetArrayElementAtIndex(_defaultIndexProp.intValue).colorValue;
                    break;
                case SerializedPropertyType.ObjectReference:
                    _defaultValueProp.objectReferenceValue = _valuesProp.arraySize == 0
                        ? null
                        : _valuesProp.GetArrayElementAtIndex(_defaultIndexProp.intValue).objectReferenceValue;
                    break;
                case SerializedPropertyType.Enum:
                    _defaultValueProp.intValue = _valuesProp.arraySize == 0
                        ? 0
                        : _valuesProp.GetArrayElementAtIndex(_defaultIndexProp.intValue).intValue;
                    break;
                case SerializedPropertyType.Vector2:
                    _defaultValueProp.vector2Value = _valuesProp.arraySize == 0
                        ? Vector2.zero
                        : _valuesProp.GetArrayElementAtIndex(_defaultIndexProp.intValue).vector2Value;
                    break;
                case SerializedPropertyType.Vector3:
                    _defaultValueProp.vector3Value = _valuesProp.arraySize == 0
                        ? Vector3.zero
                        : _valuesProp.GetArrayElementAtIndex(_defaultIndexProp.intValue).vector3Value;
                    break;
                case SerializedPropertyType.Vector4:
                    _defaultValueProp.vector4Value = _valuesProp.arraySize == 0
                        ? Vector4.zero
                        : _valuesProp.GetArrayElementAtIndex(_defaultIndexProp.intValue).vector4Value;
                    break;
                case SerializedPropertyType.Rect:
                    _defaultValueProp.rectValue = _valuesProp.arraySize == 0
                        ? Rect.zero
                        : _valuesProp.GetArrayElementAtIndex(_defaultIndexProp.intValue).rectValue;
                    break;
                case SerializedPropertyType.ArraySize:
                    _defaultValueProp.arraySize = _valuesProp.arraySize == 0
                        ? 0
                        : _valuesProp.GetArrayElementAtIndex(_defaultIndexProp.intValue).arraySize;
                    break;
                case SerializedPropertyType.AnimationCurve:
                    _defaultValueProp.animationCurveValue = _valuesProp.arraySize == 0
                        ? AnimationCurve.Linear(0, 0, 1, 1)
                        : _valuesProp.GetArrayElementAtIndex(_defaultIndexProp.intValue).animationCurveValue;
                    break;
                case SerializedPropertyType.Bounds:
                    _defaultValueProp.boundsValue = _valuesProp.arraySize == 0
                        ? new Bounds()
                        : _valuesProp.GetArrayElementAtIndex(_defaultIndexProp.intValue).boundsValue;
                    break;
                case SerializedPropertyType.Quaternion:
                    _defaultValueProp.quaternionValue = _valuesProp.arraySize == 0
                        ? Quaternion.identity
                        : _valuesProp.GetArrayElementAtIndex(_defaultIndexProp.intValue).quaternionValue;
                    break;
                case SerializedPropertyType.ExposedReference:
                    _defaultValueProp.exposedReferenceValue = _valuesProp.arraySize == 0
                        ? null
                        : _valuesProp.GetArrayElementAtIndex(_defaultIndexProp.intValue).exposedReferenceValue;
                    break;
                case SerializedPropertyType.Vector2Int:
                    _defaultValueProp.vector2IntValue = _valuesProp.arraySize == 0
                        ? Vector2Int.zero
                        : _valuesProp.GetArrayElementAtIndex(_defaultIndexProp.intValue).vector2IntValue;
                    break;
                case SerializedPropertyType.Vector3Int:
                    _defaultValueProp.vector3IntValue = _valuesProp.arraySize == 0
                        ? Vector3Int.zero
                        : _valuesProp.GetArrayElementAtIndex(_defaultIndexProp.intValue).vector3IntValue;
                    break;
                case SerializedPropertyType.RectInt:
                    _defaultValueProp.rectIntValue = _valuesProp.arraySize == 0
                        ? new RectInt()
                        : _valuesProp.GetArrayElementAtIndex(_defaultIndexProp.intValue).rectIntValue;
                    break;
                case SerializedPropertyType.BoundsInt:
                    _defaultValueProp.boundsIntValue = _valuesProp.arraySize == 0
                        ? new BoundsInt()
                        : _valuesProp.GetArrayElementAtIndex(_defaultIndexProp.intValue).boundsIntValue;
                    break;
                default:
                case SerializedPropertyType.Gradient:
                case SerializedPropertyType.FixedBufferSize:
                case SerializedPropertyType.Generic:
                case SerializedPropertyType.ManagedReference:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}