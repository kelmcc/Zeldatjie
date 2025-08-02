using System;
using UnityEngine;

namespace Zeldatjie.Gameplay
{
    public class SceneRootLogic : MonoBehaviour
    {
        [SerializeField] private BaseEnemy _enemy;
        [SerializeField] private LootingLogic _lootLogic;
        [SerializeField] private ExploreLogic _exploreLogic;
        private System<GameManager> _gameManager;
        
        private void Awake()
        {
            _lootLogic.gameObject.SetActive(false);
            _exploreLogic.gameObject.SetActive(false);
        }

        public void SetToFightMode()
        {
            _enemy.LIVE();
        }

        public void SetToLootMode()
        {
            _lootLogic.gameObject.SetActive(true);
            _lootLogic.Init(() =>
            {
                _gameManager.Value.SetState(GameManager.GameState.Explore);
                SetToExploreMode();
                // do more
            }, () =>
            {
                _gameManager.Value.SetState(GameManager.GameState.Explore);
                SetToExploreMode();
                // do more
            } );
        }
        
        public void SetToExploreMode()
        {
            _lootLogic.gameObject.SetActive(false);
            _exploreLogic.gameObject.SetActive(true);
            _exploreLogic.Init(() =>
            {
                _gameManager.Value.EnterNextBattle();
                // do more
            }, () =>
            {
                _gameManager.Value.EnterNextBattle();
                // do more
            } );

        }

        private void Update()
        {
            if (!_enemy.IsAlive && _gameManager.Value.CurrentGameState == GameManager.GameState.Fight)
            {
                _gameManager.Value.SetState(GameManager.GameState.Loot);
                SetToLootMode();
            }
        }
    }
}