using System;

public class PickableHealth : PickableBase
{
    public static event Action OnHealthPickup;

    public override void Interact()
    {
        OnHealthPickup?.Invoke();
        base.Interact();
    }
}

