using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GState
{
    Menu,
    Gameplay,
    Paused,
    GameOver
}

[Serializable]
public class GameState
{
    [HideInInspector] public string Name { get; private set; }
    public GState StateKey;
    public UnityEvent OnStateEnter, OnStateExit;

    public void EnterState()
    {
        OnStateEnter?.Invoke();
    }

    public void ExitState()
    {
        OnStateExit?.Invoke();
    }

    public void SetName()
    {
        Name = StateKey.ToString();
    }
}

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private List<GameState> _gameStates;
    [SerializeField] private GameState _curentState;
    [SerializeField] private int _stateIndex = -1;

    private void OnValidate()
    {
        if (_gameStates.Count > 0)
        {
            for (int i = 0; i < _gameStates.Count; i++)
            {
                _gameStates[i]?.SetName();
            }
        }
    }

    private void Awake()
    {
        SwitchStateNext();
    }

    private void SwitchStateNext()
    {
        _curentState?.ExitState();

        _stateIndex++;
        _curentState = _gameStates[_stateIndex];

        _curentState?.EnterState();
    }
}
