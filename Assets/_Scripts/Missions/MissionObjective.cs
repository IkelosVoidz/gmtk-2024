using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "MissionObjecive", menuName = "MissionObjective", order = 0)]
public class MissionObjective : ScriptableObject
{
    public string _name;
    [TextArea] public string _description;
    public string _mission;
    [HideInInspector] public int _order;
    public bool _completed;


    [SerializeField] private AudioClip _completeObj;

    private void Awake()
    {
        _completed = false;
        Debug.Log(name + " _completed: " + _completed);
    }

    public virtual void OnSelected()
    {
        //play radio message or whatever
    }

    public virtual void OnCompleted()
    {
        //play radio message or whatever

        SoundManager.Instance.Play2DSoundFXClip(_completeObj, .3f);
    }

    public virtual void OnUpdate() //que se llame o del mismo mission manager o usando eventos de c# en los HIJOS
    {

    }
}