using UnityEngine;

namespace Zeldatjie.Gameplay
{
    public class ExploreLogic : MonoBehaviour
    {
        [SerializeField] private TwoOptionBehaviour _twoOptionBehaviour;
        
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