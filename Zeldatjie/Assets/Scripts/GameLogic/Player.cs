using UnityEngine;
using UnityEngine.InputSystem;

namespace Zeldatjie.Gameplay
{
    public class Player : MonoBehaviour
    {
        public bool IsAlive => _currentHealth > 0;
        public bool IsBlocking { get; private set; }

        private int _currentHealth;
        private int _currentLevel;

        [SerializeField] private int _maxHealth;
        [SerializeField] private int _attackPower = 2;

        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private Sprite _idleSprite;
        [SerializeField] private Sprite _attackSprite;
        [SerializeField] private Sprite _blockSprite;
        [SerializeField] private Sprite _hitSprite;
        [SerializeField] private Sprite _titleSprite;

        private InputAction _leftAction;
        private InputAction _rightAction;

        private CharacterState _characterState = CharacterState.None;
        private enum CharacterState
        {
            None,
            Idle,
            Attack,
            Defend,
            Hit,
            Title,
            Dead
        }

        private float _stateChangedTime;
        [SerializeField] private float _resetDelay = 0.5f;

        public System.Action OnAttack;
        public System.Action OnDefend;

        private void Awake()
        {
            _currentHealth = _maxHealth;
            _spriteRenderer.sprite = _titleSprite;
        }

        private void OnEnable()
        {
            _leftAction = new InputAction(binding: "<Keyboard>/leftArrow");
            _rightAction = new InputAction(binding: "<Keyboard>/rightArrow");

            _leftAction.Enable();
            _rightAction.Enable();
        }

        private void OnDisable()
        {
            _leftAction.Disable();
            _rightAction.Disable();
        }

        public void UpdatePlayer()
        {
            if (!IsAlive) return;

            if (_leftAction.WasPressedThisFrame())
            {
                Defend();
            }
            else if (_rightAction.WasPressedThisFrame())
            {
                Attack();
            }

            // Reset state to idle after delay
            if (_characterState != CharacterState.Idle &&
                Time.time - _stateChangedTime > _resetDelay)
            {
                SetState(CharacterState.Idle, _idleSprite);
                IsBlocking = false;
            }
        }

        private void SetState(CharacterState state, Sprite sprite)
        {
            _characterState = state;
            _stateChangedTime = Time.time;
            _spriteRenderer.sprite = sprite;
        }

        private void Attack()
        {
            Debug.Log("Player attacks!");
            SetState(CharacterState.Attack, _attackSprite);
            OnAttack?.Invoke();
        }

        private void Defend()
        {
            Debug.Log("Player blocks!");
            IsBlocking = true;
            SetState(CharacterState.Defend, _blockSprite);
            OnDefend?.Invoke();
        }

        public void TakeDamage(int amount)
        {
            if (IsBlocking)
            {
                Debug.Log("Player blocked the attack!");
                return;
            }

            _currentHealth -= amount;
            Debug.Log($"Player takes {amount} damage! Remaining HP: {_currentHealth}");
            SetState(CharacterState.Hit, _hitSprite);

            if (_currentHealth <= 0)
            {
                _characterState = CharacterState.Dead;
                _spriteRenderer.sprite = _hitSprite;
                Debug.Log("Player died.");
            }
        }

        public int GetAttackPower() => _attackPower;
        
    }
}
