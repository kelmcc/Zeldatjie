using UnityEngine;

namespace Zeldatjie.Gameplay
{
    public class BattleLogic : MonoBehaviour
    {
        public Player Player;
        public BaseEnemy Enemy;

        private float _enemyAttackCooldown = 2.5f;
        private float _enemyTimer;
        private System<GameManager> _gameManager;

        private void Start()
        {
            Enemy.LIVE();

            Player = _gameManager.Value.Player;
            Player.OnAttack += HandlePlayerAttack;
            Player.OnDefend += HandlePlayerDefend;
        }

        private void Update()
        {
            if (Player == null || Enemy == null || !Player.IsAlive || !Enemy.IsAlive)
                return;
            
            if (_enemyTimer >= _enemyAttackCooldown)
            {
                _enemyTimer = 0f;
                Enemy.ResetState();
                EnemyAttack();
            }
        }

        private void HandlePlayerAttack()
        {
            if (Enemy.IsAlive)
            {
                Enemy.TakeDamage(Player.GetAttackPower());
            }
        }

        private void HandlePlayerDefend()
        {
            
        }

        private void EnemyAttack()
        {
            if (Enemy.IsAlive && Player.IsAlive)
            {
                Enemy.TakeTurn(Player);
            }
        }
    }
}