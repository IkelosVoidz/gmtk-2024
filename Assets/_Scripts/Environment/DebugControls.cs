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
        [SerializeField] private GameObject _interfaceObject, _uninstalledWarning;
        [SerializeField] private Toggle _preserveStateToggle;
        
        [SerializeField] private StateLoader _stateLoader;
        

#if ENABLE_INPUT_SYSTEM
        private PlayerInputActions _actions;
        private InputAction _move, _jump, _dash;

        [SerializeField] private Image _upVisual, _downVisual, _leftVisual, _rightVisual, _jumpVisual;
        [SerializeField] private Color _defaultColor, _pressedColor;
        [SerializeField] private Text _fpsCapText, _timeScaleText;
        [SerializeField] private Text _dashBindText, _jumpBindText;

        private readonly int[] _fpsCaps = { -1, 10, 30, 60 };
        private readonly float[] _timeScales = { 0.2f, 1, 2 };

        private int _fpsCapIndex = 0;
        private int _timeScaleIndex = 1;

        private void Start()
        {
            _actions = new PlayerInputActions();
            _move = _actions.Player.Move;
            _jump = _actions.Player.Jump;
            _dash = _actions.Player.Dash;
            _actions.Enable();

            Set();
            SetControlText();
            
            _preserveStateToggle.isOn = _stateLoader.PreserveState;
        }

        private void OnDisable() => _actions.Disable();

        private void SetControlText()
        {
            SetBinding(_jump, _jumpBindText);
            SetBinding(_dash, _dashBindText);

            void SetBinding(InputAction action, Text text)
            {
                var builder = new StringBuilder();
                builder.Append(action.name).Append(": ");
                foreach (var bind in action.bindings)
                {
                    var bindText = bind.effectivePath[(bind.effectivePath.IndexOf('/') + 1)..];
                    builder.Append(char.ToUpper(bindText[0]) + bindText[1..]);
                    if (bind != action.bindings[^1]) builder.Append(", ");
                }

                text.text = builder.ToString();
            }
        }

        private void Update()
        {
            var input = new FrameInput
            {
                JumpDown = _jump.WasPressedThisFrame(),
                JumpHeld = _jump.IsPressed(),
                DashDown = _dash.WasPressedThisFrame(),
                Move = _move.ReadValue<Vector2>()
            };

            _upVisual.color = input.Move.y > 0 ? _pressedColor : _defaultColor;
            _downVisual.color = input.Move.y < 0 ? _pressedColor : _defaultColor;
            _leftVisual.color = input.Move.x < 0 ? _pressedColor : _defaultColor;
            _rightVisual.color = input.Move.x > 0 ? _pressedColor : _defaultColor;

            _jumpVisual.color = input.JumpHeld ? _pressedColor : _defaultColor;
        }

        public void CycleTimeScale(int dir)
        {
            _timeScaleIndex = (_timeScaleIndex + dir + _timeScales.Length) % _timeScales.Length;
            Set();
        }

        public void CycleFpsCap(int dir)
        {
            _fpsCapIndex = (_fpsCapIndex + dir + _fpsCaps.Length) % _fpsCaps.Length;
            Set();
        }

        private void Set()
        {
            Application.targetFrameRate = _fpsCaps[_fpsCapIndex];
            Time.timeScale = _timeScales[_timeScaleIndex];

            _fpsCapText.text = _fpsCapIndex == 0 ? "None" : _fpsCaps[_fpsCapIndex].ToString();
            _timeScaleText.text = $"{_timeScales[_timeScaleIndex]:F1}x";
        }

        public void ToggleSaveState(bool on)
        {
            _stateLoader.TogglePreserve(on);
        }

#else
#endif
        
        private void OnValidate()
        {
            if (Application.isPlaying) return;

#if ENABLE_INPUT_SYSTEM
            var installed = true;
#else
            var installed = false;
#endif

            _interfaceObject.SetActive(installed);
            _uninstalledWarning.SetActive(!installed);
        }
    }
}