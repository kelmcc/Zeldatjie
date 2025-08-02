using System;
using UnityEditor;
using UnityEngine;

namespace Framework
{
    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute), true)]
    public class MinMaxSliderDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            Type type = property.GetValueType();
         
            if (type == typeof(FloatRange) || type == typeof(IntRange))
            {
                float minLimit = ((MinMaxSliderAttribute)attribute).Min;
                float maxLimit = ((MinMaxSliderAttribute)attribute).Max;
                float snapLimit = ((MinMaxSliderAttribute)attribute).Snap;
                float snap = 1/snapLimit;

                SerializedProperty minProperty = property.FindPropertyRelative("_min");
                SerializedProperty maxProperty = property.FindPropertyRelative("_max");

                float minValue = minProperty.floatValue;
                float maxValue = maxProperty.floatValue;
                
                GUIContent labelX = new GUIContent($"{label.text}: ({Mathf.Round(minValue*snap)/snap} - {Mathf.Round(maxValue*snap)/snap})");
                EditorGUI.BeginProperty(rect, labelX, property);
                EditorGUI.BeginChangeCheck();
                
                EditorGUI.MinMaxSlider(new Rect(rect.x, rect.y, rect.width, rect.height), labelX, ref minValue, ref maxValue, minLimit, maxLimit);

                if (EditorGUI.EndChangeCheck())
                {
                   
                    minProperty.floatValue = Mathf.Round(minValue*snap)/snap;
                    maxProperty.floatValue = Mathf.Round(maxValue*snap)/snap;

                    property.serializedObject.ApplyModifiedProperties();
                }

                EditorGUI.EndProperty();
            }

            else
            {
                EditorGUI.LabelField(rect, new GUIContent("Min Max slider only works on ranges!"), "ErrorLabel");
            }
        }




    }
}

