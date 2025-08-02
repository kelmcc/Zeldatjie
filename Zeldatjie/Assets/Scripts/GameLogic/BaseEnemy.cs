using UnityEngine;

namespace Zeldatjie.Gameplay
{
    public class BaseEnemy : MonoBehaviour
    {
        [SerializeField] EnemyDataBase _enemyData;
        [SerializeField] private int _health;
        [SerializeField] private int _attackPower;
        
        public bool IsAlive => _health > 0;

        public void LIVE()
        {
            _health = _enemyData.BaseHealth;
            _attackPower = _enemyData.BaseAttackPower;
        }
    }
}