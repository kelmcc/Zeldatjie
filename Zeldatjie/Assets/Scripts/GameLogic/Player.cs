using UnityEngine;
using UnityEngine.InputSystem;

namespace Zeldatjie.Gameplay
{
    public class Player : MonoBehaviour
    {
        public bool IsAlive => _currentHealth > 0;

        private int _currentHealth;
        private int _currentLevel;

        [SerializeField] private int _maxHealth;
    
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        [SerializeField] private Sprite _idleSprite;
        [SerializeField] private Sprite _attackSprite;
        [SerializeField] private Sprite _shieldSprite;
        [SerializeField] private Sprite _hitSprite;
        [SerializeField] private Sprite _titleSprite;
        
        private InputAction _leftAction;
        private InputAction _rightAction;
        private System<GameManager> _gameManager;

        private CharacterState _characterState = CharacterState.None;
        private enum CharacterState // what are you doing... really another enum? fuck yooooouuu
        {
            None,
            Idle,
            Attack,
            Defend,
            Hit,
            Title,
            Dead
        }
        
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
            if (!IsAlive)
            {
                return;
            }
            
            if (_leftAction.WasPressedThisFrame())
            {
                //HandleSelection(SelectedState.Left, LeftAction, _leftSelected);
                Debug.Log("Left Arrow Pressed");
            }
            else if (_rightAction.WasPressedThisFrame())
            {
                //HandleSelection(SelectedState.Right, RightAction, _rightSelected);
                Debug.Log("Right Arrow Pressed");

            }

            /*if (_selectedState != SelectedState.None && Time.time - _stateChangedTime > _resetDelay)
            {
                ResetSelection();
            }*/
        }

        /*private void HandleSelection(SelectedState state, Action action, GameObject selectedObject)
        {
            if (_selectedState != state)
            {               
                ResetSelection();
                _selectedState = state;
                _stateChangedTime = Time.time;
                selectedObject?.SetActive(true);
            }
            else
            {
                action?.Invoke();
            }
        }

        private void ResetSelection()
        {
            _selectedState = SelectedState.None;
            _leftSelected?.SetActive(false);
            _rightSelected?.SetActive(false);
        }*/
        
        

    }
}