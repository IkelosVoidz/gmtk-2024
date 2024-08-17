using UnityEngine;

namespace TarodevController.Demo
{
    public class LapTrigger : MonoBehaviour
    {
        public bool IsHit { get; private set; }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out IPlayerController _)) IsHit = true;
        }

        public void Reset() => IsHit = false;
    }
}