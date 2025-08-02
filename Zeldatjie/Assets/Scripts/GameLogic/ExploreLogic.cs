using System;
using UnityEngine;

namespace Zeldatjie.Gameplay
{
    public class ExploreLogic : MonoBehaviour
    {
        [SerializeField] private TwoOptionBehaviour _twoOptionBehaviour;
        
        public void Init(Action leftSelection, Action rightSelection)
        {
            _twoOptionBehaviour.LeftAction += leftSelection;
            _twoOptionBehaviour.RightAction += rightSelection;
        }
    }
}