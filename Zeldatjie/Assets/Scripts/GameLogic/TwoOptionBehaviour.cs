using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Zeldatjie.Gameplay
{
    public class TwoOptionBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject _leftSelected;
        [SerializeField] private GameObject _rightSelected;
        [SerializeField] private float _resetDelay = 3f;

        private SelectedState _selectedState = SelectedState.None;
        private float _stateChangedTime;

        public Action LeftAction { get; set; }
        public Action RightAction { get; set; }

        private InputAction _leftAction;
        private InputAction _rightAction;

        private enum SelectedState
        {
            None,
            Left,
            Right
        }

        private void Awake()
        {
            _leftSelected.SetActive(false);
            _rightSelected.SetActive(false);
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

        private void Update()
        {
            if (_leftAction.WasPressedThisFrame())
            {
                HandleSelection(SelectedState.Left, LeftAction, _leftSelected);
            }
            else if (_rightAction.WasPressedThisFrame())
            {
                HandleSelection(SelectedState.Right, RightAction, _rightSelected);
            }

            if (_selectedState != SelectedState.None && Time.time - _stateChangedTime > _resetDelay)
            {
                ResetSelection();
            }
        }

        private void HandleSelection(SelectedState state, Action action, GameObject selectedObject)
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
        }
    }
}
