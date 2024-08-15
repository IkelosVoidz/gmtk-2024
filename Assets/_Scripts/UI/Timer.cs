using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private float _remainingTime;
    private bool _timerActive;
    public UnityEvent onTimerEnded;


    private void Start()
    {
        _timerText.enabled = false;
    }

    public void StartTimer()
    {
        _timerActive = true;
        _timerText.enabled = true;
    }
    void Update()
    {
        if (_timerActive) {
            if (_remainingTime > 0) {
                _remainingTime -= Time.deltaTime;
            }
            else if (_remainingTime < 0) {
                _remainingTime = 0;
                _timerActive = false;
                _timerText.enabled = false;
                onTimerEnded?.Invoke();
            }

            int minutes = Mathf.FloorToInt(_remainingTime / 60);
            int seconds = Mathf.FloorToInt(_remainingTime % 60);
            _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
