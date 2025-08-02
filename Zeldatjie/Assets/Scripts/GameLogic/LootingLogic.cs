using UnityEngine;

namespace Zeldatjie.Gameplay
{
    public class LootingLogic : MonoBehaviour
    {
        [SerializeField] private TwoOptionBehaviour _twoOptionBehaviour;
        private System<GameManager> _gameManager;
        
        private void Init()
        {
            _twoOptionBehaviour.LeftAction = () =>
            {
                Debug.Log("Left action");
            };
            _twoOptionBehaviour.RightAction = () =>
            {
                Debug.Log("Right action");
            };
        }

    }
}