using UnityEngine;
using UnityEngine.Events;

public class TriggerDoSomething : MonoBehaviour
{
    public UnityEvent onTriggerDoSomething;

    private  void OnTriggerEnter2D(Collider2D other) {
         onTriggerDoSomething?.Invoke();

        BoxCollider[] boxColliders = GetComponents<BoxCollider>();

        foreach (BoxCollider collider in boxColliders)
        {
            if (collider != null) collider.enabled = false;
        }

    }
}
       
