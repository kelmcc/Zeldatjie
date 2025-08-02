using UnityEngine;

namespace Zeldatjie.Gameplay
{
    [CreateAssetMenu(fileName = "EnemyDataBase", menuName = "ScriptableObjects/EnemyDataBase", order = 0)]
    public class EnemyDataBase : ScriptableObject
    {
        public int BaseHealth;
        public int BaseAttackPower;
    }
}