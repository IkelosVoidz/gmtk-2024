using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


namespace TarodevController{
    public class SetViewPivot : MonoBehaviour
    {
        [field: SerializeField] public PlayerStats Stats { get; private set; }

        private void Awake() {

            
        }

        // Update is called once per frame
        void Update()
        {
            transform.SetLocalPositionAndRotation(new Vector3(0 , 0 + Stats.CharacterSize.Height/2 , 0 ), Quaternion.identity) ;
        }
    }

}
