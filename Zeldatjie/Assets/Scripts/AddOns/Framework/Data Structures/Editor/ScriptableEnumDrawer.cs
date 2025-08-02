using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Framework
{
    //[CustomPropertyDrawer(typeof(ScriptableEnum), true)]
    //[CanEditMultipleObjects]
    public class ScriptableEnumDrawer : PropertyDrawer
    {
        const float PICKER_WIDTH = 36f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Type type = fieldInfo.FieldType; 
            if (fieldInfo.FieldType.IsArray)
            {
                type = fieldInfo.FieldType.GetElementType();
            }
            else if (fieldInfo.FieldType.IsGenericType && (fieldInfo.FieldType.GetGenericTypeDefinition() == typeof(List<>)))
            {
                type = fieldInfo.FieldType.GetGenericArguments()[0];
            }
          
            EditorGUI.BeginProperty(position, GUIContent.none, property);


            DrawPopup(position.WithWidth(position.width - PICKER_WIDTH), GUIContent.none, type, (ScriptableEnum)property.objectReferenceValue, selectedValue =>
              {
                  property.objectReferenceValue = selectedValue;
                  property.serializedObject.ApplyModifiedProperties();
              });


            EditorGUI.indentLevel = 0;

            Rect rect = new Rect(position.xMax - PICKER_WIDTH, position.y, PICKER_WIDTH, position.height);
            EditorGUI.PropertyField(rect, property, GUIContent.none);

            EditorGUI.EndProperty();

        }

        static int SetupValuesAndLabels(Type type, Object currentValue, out List<ScriptableEnum> values, out GUIContent[] labels, bool allowNull)
        {
            values = new List<ScriptableEnum>(ScriptableEnum.GetValues(type));
            values.Sort((x, y) => x.name.CompareTo(y.name));

            if (allowNull)
            {
                values.Insert(0, null);
            }

            int selectedIndex = 0;
            if (currentValue != null)
            {
                for (int i = 0; i < values.Count; i++)
                {
                    if (values[i] == currentValue)
                    {
                        selectedIndex = i;
                        break;
                    }
                }
            }

            labels = new GUIContent[values.Count];
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] == null)
                {
                    labels[i] = new GUIContent("NONE");
                }
                else
                {
                    labels[i] = new GUIContent(values[i].name);
                }
            }

            return selectedIndex;
        }


        public static void DrawPopup(Rect rect, GUIContent label, Type type, ScriptableObject currentValue, Action<ScriptableObject> onOptionSelected, bool allowNull = true)
        {
            int selectedIndex = SetupValuesAndLabels(type, currentValue, out List<ScriptableEnum> values, out GUIContent[] labels, allowNull);
            SearchablePopup.DrawButton(rect, label, labels.ToStringArray(), selectedIndex, index => onOptionSelected(values[index]));
        }

        public static T DrawPopup<T>(Rect position, GUIContent label, T currentValue, bool allowNull = true, bool showMixedValue = false) where T : ScriptableEnum
        
        {
            int selectedIndex = SetupValuesAndLabels(typeof(T), currentValue, out List<ScriptableEnum> values, out GUIContent[] labels, allowNull);

            if (showMixedValue)
            {
                EditorGUI.showMixedValue = showMixedValue;
                EditorGUI.BeginChangeCheck();
                int getSelection = EditorGUI.Popup(position.WithWidth(position.width), label, selectedIndex, labels);
                EditorGUI.showMixedValue = false;
                currentValue = null;
                if (EditorGUI.EndChangeCheck())
                {
                    currentValue = (T)values[getSelection];
                }
            }
            else
            {
                selectedIndex = EditorGUI.Popup(position.WithWidth(position.width), label, selectedIndex, labels);
                currentValue = (T)values[selectedIndex];
            }

            return currentValue;
        }

    }
}

