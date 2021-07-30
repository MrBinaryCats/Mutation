using System;
using System.Linq;
using System.Reflection;
using Mutations.Entity;
using Mutations.Mutations.Core;
using UnityEditor;
using UnityEngine;

namespace Mutations.Editor
{
    [CustomEditor(typeof(EntityData))]
    public class EntityEditor : UnityEditor.Editor
    {
        private UnityEditor.Editor[] _editors;
        private SerializedProperty _mutationsProp;

        private GenericMenu _typeMenu;
        private new EntityData target => (EntityData) base.target;

        private void OnEnable()
        {
            _mutationsProp = serializedObject.FindProperty("Mutations");
            CreateTypeMenu();
            CreateEditors();
        }

        private void OnDestroy()
        {
            //make sure we clean up the editors
            foreach (var editor in _editors)
                DestroyImmediate(editor);
        }


        /// <summary>
        ///     Creates a generic menu with all the available mutation types
        ///     When an option is clicked the mutation is added to the object
        /// </summary>
        private void CreateTypeMenu()
        {
            _typeMenu = new GenericMenu();
            var subclassTypes = Assembly
                .GetAssembly(typeof(MutationBase))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(MutationBase)) && !t.IsAbstract);
            foreach (var type in subclassTypes)
                if (!target.HasMutationType(type))
                    _typeMenu.AddItem(new GUIContent(type.Name), false, AddNewMutation, type);
        }

        /// <summary>
        ///     Create the editors which are used to draw the mutations inspectors
        /// </summary>
        private void CreateEditors()
        {
            _editors = new UnityEditor.Editor[target.Mutations.Length];
            for (var i = 0; i < _editors.Length; i++) _editors[i] = CreateEditor(target.Mutations[i]);
        }

        public override void OnInspectorGUI()
        {
            var removeIndex = -1;
            base.OnInspectorGUI();

            //Create the mutations box
            using (new GUILayout.VerticalScope("GroupBox"))
            {
                EditorGUILayout.LabelField(new GUIContent("Mutations"), EditorStyles.largeLabel);

                //go through mutation and draw it
                for (var i = 0; i < _mutationsProp.arraySize; i++)
                {
                    var element = _mutationsProp.GetArrayElementAtIndex(i);
                    using (new GUILayout.VerticalScope("GroupBox"))
                    {
                        //user friendly way of retaining if the foldout should be open or not
                        var current = SessionState.GetBool(element.objectReferenceValue.name, false);
                        current = EditorGUILayout.InspectorTitlebar(current, element.objectReferenceValue);
                        SessionState.SetBool(element.objectReferenceValue.name, current);
                        if (current)
                            using (new EditorGUI.IndentLevelScope())
                            {
                                //Draw the mutation's inspector
                                _editors[i].OnInspectorGUI();
                                //add a button to remove it
                                if (GUILayout.Button("Remove this Mutation"))
                                    removeIndex = i; //if the button was clicked store the index so it can be removed
                            }
                    }
                }


                //If there are no more available options disable the button
                using (new EditorGUI.DisabledScope(_typeMenu.GetItemCount() == 0))
                {
                    //Show the generic menu when the button is clicked
                    if (GUILayout.Button("Add New Mutation"))
                        _typeMenu.ShowAsContext();
                }
            }

            if (removeIndex > -1)
                //if the remove index is >-1 the user must have clicked to remove the Mutation so remove that mutation
                RemoveMutation(removeIndex);
        }

        /// <summary>
        ///     Adds a new mutation to the EntityData
        /// </summary>
        /// <param name="datatype">The type of mutation to create</param>
        private void AddNewMutation(object datatype)
        {
            var t = (Type) datatype;
            //Create a scriptable object instance of the type given in
            var instance = CreateInstance(t);
            instance.name = ObjectNames.NicifyVariableName(t.Name); //give it a nice name :) 

            //Add the created instance to the scriptable object that's on disk.
            //the user will need to save the project before it becomes visible in the project window 
            AssetDatabase.AddObjectToAsset(instance, target);

            //add the new instance to the Mutations array
            _mutationsProp.InsertArrayElementAtIndex(_mutationsProp.arraySize);
            var prop = _mutationsProp.GetArrayElementAtIndex(_mutationsProp.arraySize - 1);
            prop.objectReferenceValue = instance;
            //save the data to the EntityData
            serializedObject.ApplyModifiedProperties();

            //recreate them menu/editor
            CreateTypeMenu();
            CreateEditors();
        }

        /// <summary>
        ///     Removes a mutation from the EntityData
        /// </summary>
        /// <param name="removeIndex">which index to remove</param>
        private void RemoveMutation(int removeIndex)
        {
            if (removeIndex < 0 || removeIndex > _mutationsProp.arraySize - 1)
                throw new ArgumentException("Index is outside the bounds of the array");

            var prop = _mutationsProp.GetArrayElementAtIndex(removeIndex);
            var obj = prop.objectReferenceValue; //store the reference it so we can remove it

            _mutationsProp.DeleteArrayElementAtIndex(removeIndex); //delete the element value
            _mutationsProp.DeleteArrayElementAtIndex(removeIndex); //delete the actual element

            //destroy the asset now the array has been modified
            DestroyImmediate(obj, true);

            //save the data to the EntityData
            serializedObject.ApplyModifiedProperties();

            //recreate them menu/editor
            CreateTypeMenu();
            CreateEditors();
        }
    }
}