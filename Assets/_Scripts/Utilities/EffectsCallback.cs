using UnityEngine;

public class EffectsCallback : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        ObjectPoolingManager.Instance.ReturnObjectToPool(gameObject);
    }
}
