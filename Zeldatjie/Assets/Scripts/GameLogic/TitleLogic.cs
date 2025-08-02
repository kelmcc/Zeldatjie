using UnityEngine;

namespace Zeldatjie.Gameplay
{
    public class TitleLogic : MonoBehaviour
    {
        [SerializeField] private TwoOptionBehaviour _twoOptionBehaviour;
        private System<GameManager> _gameManager;
        private void Awake()
        {
            _twoOptionBehaviour.LeftAction = () =>
            {
                Debug.Log("Left action");
                _gameManager.Value.EnterNextBattle();
            };
            _twoOptionBehaviour.RightAction = () =>
            {
                Debug.Log("Right action");
                Application.Quit();
            };
        }
    }
}