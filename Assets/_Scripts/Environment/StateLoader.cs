using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TarodevController.Demo
{
    /// <summary>
    /// Please don't use this in production. It's just a simple example showing you how to use controller state
    /// </summary>
    public class StateLoader : MonoBehaviour
    {
        public bool PreserveState { get; private set; }

        private IPlayerController _player;

        private bool ShouldAct => PreserveState && _player != null;
        private static string SaveKey => $"Controller_Debug_Loader_{SceneManager.GetActiveScene().name}";
        private static string PreserveKey => $"Controller_Debug_Preserve_{SceneManager.GetActiveScene().name}";

        private void Awake()
        {
            var possiblePlayer = FindObjectOfType<PlayerController>();
            if (!possiblePlayer)
            {
                Destroy(gameObject);
                return;
            }

            _player = possiblePlayer.GetComponent<IPlayerController>();

            PreserveState = PlayerPrefs.GetInt(PreserveKey, 0) == 1;
        }

        internal void TogglePreserve(bool on)
        {
            PreserveState = on;
            PlayerPrefs.SetInt(PreserveKey, on ? 1 : 0);
        }

        private IEnumerator Start()
        {
            if (!ShouldAct) yield break;

            if (PlayerPrefs.HasKey(SaveKey))
            {
                yield return null;
                var state = JsonUtility.FromJson<ControllerState>(PlayerPrefs.GetString(SaveKey));
                _player.LoadState(state);
            }
        }

        private void OnDestroy()
        {
            PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(_player.State));
        }
    }
}