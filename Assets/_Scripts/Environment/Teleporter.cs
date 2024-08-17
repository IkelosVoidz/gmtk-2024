using System.Collections;
using UnityEngine;

namespace TarodevController.Demo
{
    public class Teleporter : MonoBehaviour
    {
        [SerializeField] private Vector2 _teleportPosition;
        [SerializeField] private AudioClip _teleportOutClip,_teleportInClip;
        [SerializeField] private GameObject _teleportParticlePrefab;
        [SerializeField] private float _teleportDelay = 0.5f;
        
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.TryGetComponent(out IPlayerController controller)) return;
            
            Instantiate(_teleportParticlePrefab, collision.transform.position, Quaternion.identity);

            controller.RepositionImmediately(_teleportPosition, true);
            controller.TogglePlayer(false);

            AudioSource.PlayClipAtPoint(_teleportInClip, _teleportPosition);

            StartCoroutine(ActivatePlayer(controller));
        }

        private IEnumerator ActivatePlayer(IPlayerController controller)
        {
            yield return new WaitForSeconds(_teleportDelay);
            controller.TogglePlayer(true);
            Instantiate(_teleportParticlePrefab, _teleportPosition, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_teleportOutClip, _teleportPosition);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, _teleportPosition);
            Gizmos.DrawSphere(_teleportPosition, 0.2f);
        }
    }
}