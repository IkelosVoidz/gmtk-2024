using UnityEngine;

public interface IInteractable
{
    /// <summary>
    /// Reference to the game object on which this Interface is implemented on
    /// </summary>
    GameObject gameObject { get; }

    void Interact();

    void OnSelect()
    {
        gameObject.SwapLayer("Outline", true);
    }

    void OnDeselect()
    {
        gameObject.SwapLayer("Interactable", true);
    }
}
