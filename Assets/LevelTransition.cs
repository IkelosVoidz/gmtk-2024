using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [SerializeField]  private Animator _animator;
    [SerializeField] private int _levelToLoad;


    public void TriggerFadeOut(){
        _animator.SetTrigger("FadeOut");
    }

    public void OnFadeOutComplete()
    {
        SceneManager.LoadScene(_levelToLoad);
    }
}
