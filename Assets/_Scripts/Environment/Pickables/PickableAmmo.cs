using System;

public class PickableAmmo : PickableBase
{
    public static event Action OnAmmoPickup;
    public override void Interact()
    {
        OnAmmoPickup?.Invoke();

        base.Interact();
    }
}