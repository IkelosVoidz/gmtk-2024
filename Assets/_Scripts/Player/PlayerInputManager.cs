using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// I am aware that this is not great design, as someone could just access everything public about the player, including its components
/// but im unable to do it any other way due to the way that we have our player input setup 
/// </summary>
public class PlayerInputManager : StaticSingleton<PlayerInputManager>
{
    private PlayerInput _playerInput;

    protected override void Awake()
    {
        base.Awake();
        _playerInput = GetComponent<PlayerInput>();
        HandleUiInputSwitch(false);
    }

    public bool SwitchActionMap(string actionMap) //en general, por si hubiese que hacer coches o lo que sea 
    {
        if (actionMap == _playerInput.currentActionMap.name)
            return false;

        _playerInput.SwitchCurrentActionMap(actionMap);
        return true;
    }
    public void HandleUiInputSwitch(bool openUI)
    {
        if (!openUI)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            SwitchActionMap("Player");
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            SwitchActionMap("UI");

        }
    }
}
