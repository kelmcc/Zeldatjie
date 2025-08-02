using System;
using UnityEngine;

namespace Zeldatjie.Gameplay
{
    public class SceneRootLogic : MonoBehaviour
    {
        [SerializeField] private LootingLogic _lootLogic;
        [SerializeField] private BaseEnemy _enemy;
        [SerializeField] private ExploreLogic _exploreLogic;

        private void Awake()
        {
            
            
        }
    }
}