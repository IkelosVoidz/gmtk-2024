using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


namespace TarodevController{
    public class SetViewPivot : MonoBehaviour
    {
        [field: SerializeField] public PlayerStats Stats { get; private set; }
        private SpriteRenderer _sprite;

        private void Awake() {
            _sprite = GetComponentInParent<SpriteRenderer>(); 
            
        }

        // Update is called once per frame
        void Update()
        {
            transform.SetLocalPositionAndRotation(new Vector3(0 + Stats.CharacterSize.Width/2 * ( _sprite.flipX ? -1 : 1), 0 + Stats.CharacterSize.Height , 0 ), Quaternion.identity) ;
        }
    }

}
