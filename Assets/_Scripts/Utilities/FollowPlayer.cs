using DG.Tweening;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _offsetX, _offsetZ;
    [SerializeField] private float _lerpSpeed;


    private Transform us;

    private void Awake()
    {
        us = transform;
    }
    private void LateUpdate()
    {
        us.DOMove(new Vector3(_target.position.x, us.position.y, _target.position.z), 1);


        us.eulerAngles = new Vector3(90, 0, -_target.eulerAngles.y);
    }
}
