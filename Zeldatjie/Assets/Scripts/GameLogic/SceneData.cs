using UnityEngine;
using UnityEngine.SceneManagement;

namespace Zeldatjie.Gameplay
{
    [CreateAssetMenu(fileName = "SceneData", menuName = "Zeldatjie/Scene Data", order = 0)]
    public class SceneData : ScriptableObject
    {
        public Scene Scene;
    
    }
}