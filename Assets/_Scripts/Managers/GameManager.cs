using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GameManager : PersistentSingleton<GameManager>
{
    public GameState State { get; private set; }  
    void Start()
    {
        ChangeState(GameState.Starting);
    }

    public void ChangeState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.Starting:
                
                break;
            case GameState:SpawningDefaultEnemies:

                break;
            //etc lo pillais supongo

        }
    }
}

[Serializable] //tengo que investigar que hace esto pero el chico lo pone
public enum GameState
{
    Starting, 
    SpawningDefaultEnemies, 
    SpawnWaves, 
    Win, 
    Lose, 
    CAGANDO
}
