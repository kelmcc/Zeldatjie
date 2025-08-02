﻿using System;
using System.Reflection;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// Causes the field to only be exposed in the editor if some other field or property condition is true.
    /// </summary>
    public class ExposeIfAttribute : PropertyAttribute
    {
        private string _fieldName;
        private object _value;
        private object[] _values;


        /// <summary>
        /// Causes the field to only be exposed in the editor if some other field or property condition is true. 
        /// </summary>
        /// <param name="fieldName">The field name to check</param>
        /// <param name="value">The value the field must be equal to</param>
        public ExposeIfAttribute(string fieldName, object value)
        {
            _fieldName = fieldName;
            _value = value;
            _values = null;

        }
        /// <summary>
        /// Causes the field to only be exposed in the editor if some other field or property is true.
        /// </summary>
        /// <param name="fieldName">The field name to check</param>
        public ExposeIfAttribute(string fieldName)
        {
            _fieldName = fieldName;
            _value = true;
            _values = null;
        }

        /// <summary>
        /// Causes the field to only be exposed in the editor if some other field or property is true.
        /// </summary>
        /// <param name="fieldName">The field name to check</param>
        ///    /// <param name="values">Any value the field can be equal to</param>
        public ExposeIfAttribute(string fieldName, params object[] values)
        {
            _fieldName = fieldName;
            _value = null;
            _values = values;
        }


        public bool EvaluateCondition(object obj)
        {

            FieldInfo field = obj.GetType().GetField(_fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (field != null)
            {
                object fieldValue = field.GetValue(obj);
                if (_values != null)
                {
                    for (int i = 0; i < _values.Length; i++)
                    {
                        if (fieldValue.Equals(_values[i])) return true;
                    }

                    return false;
                }
                if (fieldValue == null && _value == null) return true;
                return fieldValue.Equals(_value);
            }


            PropertyInfo prop = obj.GetType().GetProperty(_fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (prop != null)
            {
                object propValue = prop.GetValue(obj, null);
                if (_values != null)
                {
                    for (int i = 0; i < _values.Length; i++)
                    {
                        if (propValue.Equals(_values[i])) return true;
                    }

                    return false;
                }
                if (propValue == null && _value == null) return true;
                return propValue.Equals(_value);
            }

            return true;

        }


    }
}
