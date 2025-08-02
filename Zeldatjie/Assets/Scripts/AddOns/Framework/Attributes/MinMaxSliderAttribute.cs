using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{

    public class MinMaxSliderAttribute : PropertyAttribute
    {

        public float Min => _min;
        public float Max => _max;
        
        public float Snap => _snap;

        private float _min;
        private float _max;
        private float _snap;

        public MinMaxSliderAttribute(float min, float max, float snap)
        {
            _min = min;
            _max = max;
            _snap = snap;
        }
    }

}
