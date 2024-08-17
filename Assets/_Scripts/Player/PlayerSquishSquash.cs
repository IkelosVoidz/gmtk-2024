using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TarodevController{

    public class PlayerSquishSquash : MonoBehaviour
    {

        [field: SerializeField] public PlayerStats Stats { get; private set; }

        public static event Action OnSquashFinished;
        public static event Action OnSquishFinished;

        [SerializeField]
        private float maxSide = 8;
        [SerializeField]
        private float minSide = 1f;



        
        private void OnEnable() {
            PlayerController.OnSquish += CalculateSquish;
            PlayerController.OnSquash += CalculateSquash;

        }

        private void OnDisable() {
            PlayerController.OnSquish -= CalculateSquish;
            PlayerController.OnSquash -= CalculateSquash;
        }

        private void CalculateSquish(){
            float currentWidth = Stats.CharacterSize.Width;
            float currentHeight = Stats.CharacterSize.Height;


            float newWidth = currentWidth -0.2f;
            float newHeight = currentHeight + 0.2f;

            if(newHeight > maxSide && newWidth < minSide) return;

            if(newHeight < maxSide) Stats.CharacterSize.SetHeight(newHeight);
            if(newWidth > minSide) Stats.CharacterSize.SetWidth(newWidth);

            OnSquishFinished?.Invoke();
        }

        private void CalculateSquash(){

            float currentWidth = Stats.CharacterSize.Width;
            float currentHeight = Stats.CharacterSize.Height;
            
            float newWidth = currentWidth +0.2f;
            float newHeight = currentHeight - 0.2f;

            if(newHeight < minSide && newWidth > maxSide) return;

            if(newWidth < maxSide) Stats.CharacterSize.SetHeight(newHeight);
            if(newHeight > minSide) Stats.CharacterSize.SetWidth(newWidth);

            OnSquishFinished?.Invoke();
        }
    }
}
