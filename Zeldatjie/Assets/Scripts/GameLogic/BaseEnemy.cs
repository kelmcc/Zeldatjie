using UnityEngine;

namespace Zeldatjie.Gameplay
{
    public class BaseEnemy : MonoBehaviour
    {
        [SerializeField] private EnemyDataBase _enemyData;
        [SerializeField] private int _health;
        [SerializeField] private int _attackPower;

        [Header("Sprites")] [SerializeField] private Sprite _idleSprite;
        [SerializeField] private Sprite _attackWindUpSprite;
        [SerializeField] private Sprite _attackSprite;
        [SerializeField] private Sprite _blockSprite;
        [SerializeField] private Sprite _hitSprite;

        private SpriteRenderer _spriteRenderer;
        private float _stateTimer;
        private float _stateDuration = 1.5f;

        public bool IsBlocking { get; private set; }
        public bool IsAlive => _health > 0;
        public int EnemyAttackPower => _attackPower;

        private enum State
        {
            Idle,
            WindUp,
            Attack,
            Block
        }

        private State _currentState;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void LIVE()
        {
            _health = _enemyData.BaseHealth;
            _attackPower = _enemyData.BaseAttackPower;
            _currentState = State.Idle;
            SetSprite(_idleSprite);
        }

        public void TakeDamage(int damage)
        {
            if (IsBlocking)
            {
                Debug.Log("Enemy blocked the attack!");
                return;
            }

            _health -= damage;
            Debug.Log($"Enemy takes {damage} damage! Remaining HP: {_health}");

            SetSprite(_hitSprite);

            if (_health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Enemy died!");
            gameObject.SetActive(false);
        }

        private void SetSprite(Sprite sprite)
        {
            if (_spriteRenderer != null && sprite != null)
            {
                _spriteRenderer.sprite = sprite;
            }
        }

        public void TakeTurn(Player player)
        {
            if (!IsAlive || !player.IsAlive)
            {
                return;
            }

            float roll = Random.value;

            if (roll < 0.5f)
            {
                Block();
            }
            else
            {
                StartCoroutine(PerformAttack(player));
            }
        }

        private void Block()
        {
            IsBlocking = true;
            SetSprite(_blockSprite);
            Debug.Log("Enemy blocks this turn.");
        }

        private System.Collections.IEnumerator PerformAttack(Player player)
        {
            IsBlocking = false;

            SetSprite(_attackWindUpSprite);
            yield return new WaitForSeconds(0.5f);

            SetSprite(_attackSprite);
            player.TakeDamage(_attackPower);
        }

        public void ResetState()
        {
            IsBlocking = false;
            SetSprite(_idleSprite);
        }
    }
}