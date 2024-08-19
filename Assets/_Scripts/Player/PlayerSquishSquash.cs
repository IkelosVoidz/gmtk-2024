using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TarodevController{

    public class PlayerSquishSquash : MonoBehaviour
    {
        [field: SerializeField] public PlayerStats Stats { get; private set; }
        


        public static event Action OnSquishOrSquashFinished;

        public ReshapeType _reshapeType = ReshapeType.IDLE;

       
        [SerializeField]
        private float maxSide = 8;
        [SerializeField]
        private float minSide = 1f - 0.1f;

        [SerializeField]
        private float _sideClearance = .1f;

        [SerializeField] private float reshapeSpeed = .1f;
        [SerializeField] private float longSideSpeedMultiplier = 3;
        [SerializeField] private float thinSideSpeedDivider = .3f;


        private Rigidbody2D _rb;

        private void Awake() {
             _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable() {
            PlayerController.OnSquish += HandleSquishInput;
            PlayerController.OnSquash += HandleSquashInput;
            PlayerController.OnResetShape += HandleResetInput;
        }

         private void OnDisable() {
            PlayerController.OnSquish -= HandleSquishInput;
            PlayerController.OnSquash -= HandleSquashInput;
            PlayerController.OnResetShape -= HandleResetInput;
        }

         private void HandleSquishInput()
        {
            HandleReshapeStates();

            if (_reshapeType == ReshapeType.SQUASH || _reshapeType == ReshapeType.SQUASH_RETURN)
            {
                _reshapeType = ReshapeType.SQUASH_RETURN;
                return;
            }
           
            _reshapeType = ReshapeType.SQUISH;
            CalculateSquish();       
        }

        private void HandleSquashInput()
        {
            HandleReshapeStates();

            if (_reshapeType == ReshapeType.SQUISH || _reshapeType == ReshapeType.SQUISH_RETURN)
            {
                _reshapeType = ReshapeType.SQUISH_RETURN;
                return;
            }
        
            _reshapeType = ReshapeType.SQUASH;
            CalculateSquash();    
        }

        private void HandleResetInput()
        {
            if (_reshapeType == ReshapeType.SQUISH)
            {
                _reshapeType = ReshapeType.SQUISH_RETURN;
            }
            else if (_reshapeType == ReshapeType.SQUASH)
            {
                _reshapeType = ReshapeType.SQUASH_RETURN;
            }

            HandleReshapeStates();
        }



         private void HandleReshapeStates()
        {
            switch (_reshapeType)
            {
                case ReshapeType.SQUISH_RETURN:
                    ResetShape(ReshapeType.SQUISH_RETURN);
                    break;

                case ReshapeType.SQUASH_RETURN:
                    ResetShape(ReshapeType.SQUASH_RETURN);
                    break;

                default:
                    // Stay idle or continue current reshape (Squish/Squash)
                    break;
            }
        }

        private void ResetShape(ReshapeType returnType)
        {

            float currentWidth = Stats.CharacterSize.Width;
            float currentHeight = Stats.CharacterSize.Height;

            float defaultWidth = Stats.CharacterSize.DefaultWidth;
            float defaultHeight = Stats.CharacterSize.DefaultHeight;

            

            float widthAdjustment = reshapeSpeed * (returnType == ReshapeType.SQUISH_RETURN ? thinSideSpeedDivider : -longSideSpeedMultiplier);
            float heightAdjustment = reshapeSpeed * (returnType == ReshapeType.SQUISH_RETURN ? -longSideSpeedMultiplier : thinSideSpeedDivider);
            float newWidth = currentWidth;
            float newHeight = currentHeight;

            if (currentWidth != defaultWidth)
            {
                newWidth = currentWidth + widthAdjustment;

                if(returnType == ReshapeType.SQUISH_RETURN) {
                    if (newWidth >= defaultWidth) newWidth = defaultWidth;

                    if(CanExpandHorizontally(newWidth , newHeight)){
                        Stats.CharacterSize.SetWidth(newWidth);
                    }
                }
                else{ //squash return 
                    if (newWidth <= defaultWidth) newWidth = defaultWidth;

                    if(newWidth > currentWidth && CanExpandHorizontally(newWidth , newHeight)){
                        Stats.CharacterSize.SetWidth(newWidth);
                    }
                    else if(newWidth <= currentWidth) Stats.CharacterSize.SetWidth(newWidth);

                }
               
               
            }

            if (currentHeight != defaultHeight)
            {
                newHeight = currentHeight + heightAdjustment;

                if(returnType == ReshapeType.SQUISH_RETURN) {
                    if (newHeight <= defaultHeight) newHeight = defaultHeight;

                    Stats.CharacterSize.SetHeight(newHeight);
                     if(newHeight > currentHeight && CanExpandVertically(newWidth , newHeight)){
                        Stats.CharacterSize.SetHeight(newHeight);
                    } 
                }
                else{ //squash return 
                    if (newHeight >= defaultHeight) newHeight = defaultHeight;

                    if(CanExpandVertically(newWidth , newHeight)){
                        Stats.CharacterSize.SetHeight(newHeight);
                    } 
                    else if(newHeight  <= currentHeight ) Stats.CharacterSize.SetHeight(newHeight);
                }   
            }

            if (newWidth == defaultWidth && newHeight == defaultHeight)
            {
                _reshapeType = ReshapeType.IDLE; 
            }

            OnSquishOrSquashFinished?.Invoke();
        }



        private void CalculateSquish(){

            float currentWidth = Stats.CharacterSize.Width;
            float currentHeight = Stats.CharacterSize.Height;


            float newWidth = currentWidth -(reshapeSpeed * thinSideSpeedDivider);
            float newHeight = currentHeight + (reshapeSpeed * longSideSpeedMultiplier);

            if(newHeight == maxSide && newWidth == minSide) return;
            
            if(CanExpandVertically(newWidth ,newHeight)){
                if(newHeight < maxSide) Stats.CharacterSize.SetHeight(newHeight);
                else  {
                    Stats.CharacterSize.SetHeight(maxSide);
                }
            }

            if(newWidth > minSide) Stats.CharacterSize.SetWidth(newWidth);
            else Stats.CharacterSize.SetWidth(minSide);

            OnSquishOrSquashFinished?.Invoke();
        }

        private void CalculateSquash(){

            float currentWidth = Stats.CharacterSize.Width;
            float currentHeight = Stats.CharacterSize.Height;
            
            float newWidth = currentWidth +(reshapeSpeed * longSideSpeedMultiplier);
            float newHeight = currentHeight - (reshapeSpeed * thinSideSpeedDivider);

            if(newHeight == minSide && newWidth == maxSide) return;
            if(CanExpandHorizontally(newWidth ,newHeight)){

                if(newWidth < maxSide) Stats.CharacterSize.SetWidth(newWidth);
                else{ 
                    Stats.CharacterSize.SetWidth(maxSide);
                }

            }


            if(newHeight > minSide) Stats.CharacterSize.SetHeight(newHeight);
            else Stats.CharacterSize.SetHeight(minSide);

            OnSquishOrSquashFinished?.Invoke();
        }

        private bool CanExpandHorizontally(float width , float height) {
            
            
            Physics2D.queriesHitTriggers = false;
            var hitL = Physics2D.OverlapBox(_rb.position + new Vector2(-(width/2) - (_sideClearance/2), height/2 ) , new Vector2(_sideClearance , height - _sideClearance), 0, Stats.CollisionLayers);
            var hitR = Physics2D.OverlapBox(_rb.position + new Vector2((width/2) + (_sideClearance/2) , height/2 ) , new Vector2(_sideClearance , height - _sideClearance), 0, Stats.CollisionLayers);
            
            return !(hitL && hitR);
        }

        private bool CanExpandVertically(float width , float height) {
           
            Physics2D.queriesHitTriggers = false;

            var hitT = Physics2D.OverlapBox(_rb.position + new Vector2(0 , height + (_sideClearance/2)) , new Vector2(width - _sideClearance, _sideClearance), 0, Stats.CollisionLayers);
            var hitB = Physics2D.OverlapBox(_rb.position - new Vector2(0 , _sideClearance/2) , new Vector2(width - _sideClearance , _sideClearance), 0, Stats.CollisionLayers);

            return !(hitT && hitB);
        } 
    }
}

public enum ReshapeType {
    SQUISH,
    SQUISH_RETURN,
    SQUASH,
    SQUASH_RETURN,
    IDLE,
}
