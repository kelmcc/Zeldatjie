using System;
using UnityEngine;

namespace Zeldatjie.Gameplay
{
    public class SceneRootLogic : MonoBehaviour
    {
        [SerializeField] private BaseEnemy _enemy;
        [SerializeField] private LootingLogic _lootLogic;
        [SerializeField] private ExploreLogic _exploreLogic;

        private void Awake()
        {
            _lootLogic.gameObject.SetActive(false);
            _exploreLogic.gameObject.SetActive(false);
        }

        public void SetToFightMode()
        {
            _enemy.LIVE();
        }
    }
}