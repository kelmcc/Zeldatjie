using System;
using UnityEngine;

namespace Zeldatjie.Gameplay
{
    public class LootingLogic : MonoBehaviour
    {
        [SerializeField] private TwoOptionBehaviour _twoOptionBehaviour;
        private System<GameManager> _gameManager;
        
        public void Init(Action leftSelection, Action rightSelection)
        {
            _twoOptionBehaviour.LeftAction += leftSelection;
            _twoOptionBehaviour.RightAction += rightSelection;
        }

    }
}