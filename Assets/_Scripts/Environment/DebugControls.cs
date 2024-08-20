using System.Text;
using UnityEngine;
using UnityEngine.UI;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace TarodevController.Demo
{
    public class DebugControls : MonoBehaviour
    {
        [SerializeField] private GameObject _interfaceObject;
        

        private PlayerInputActions _actions;
        private InputAction _move, _jump, _squish , _squash;

        [SerializeField] private Image _upVisual, _downVisual, _leftVisual, _rightVisual, _jumpVisual , _squishVisual , _squashVisual;
        [SerializeField] private Color _defaultColor, _pressedColor;


        private void Start()
        {
            _actions = new PlayerInputActions();
            _move = _actions.Player.Move;
            _jump = _actions.Player.Jump;
            _squish = _actions.Player.Squish;
            _squash = _actions.Player.Squash;
            _actions.Enable();    
        }

        private void OnDisable() => _actions.Disable();

      

        private void Update()
        {
            var input = new FrameInput
            {
                JumpDown = _jump.WasPressedThisFrame(),
                JumpHeld = _jump.IsPressed(),
                SquishHeld = _squish.IsPressed(),
                SquashHeld = _squash.IsPressed(),
                Move = _move.ReadValue<Vector2>()
            };

            _upVisual.color = input.Move.y > 0 ? _pressedColor : _defaultColor;
            _downVisual.color = input.Move.y < 0 ? _pressedColor : _defaultColor;
            _leftVisual.color = input.Move.x < 0 ? _pressedColor : _defaultColor;
            _rightVisual.color = input.Move.x > 0 ? _pressedColor : _defaultColor;

            _squishVisual.color = input.SquishHeld ? _pressedColor : _defaultColor;
            _squashVisual.color = input.SquashHeld ? _pressedColor : _defaultColor;

            _jumpVisual.color = input.JumpHeld ? _pressedColor : _defaultColor;
        }
    }
}
