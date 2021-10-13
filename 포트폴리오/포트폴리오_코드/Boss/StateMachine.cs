using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    public Istate<T> CurrentState { get; private set; }

    private T m_sender;

    public StateMachine(T sender, Istate<T> state)
    {
        m_sender = sender;
        SetState(state);
    }

    public void SetState(Istate<T> state)
    {
        if (m_sender == null)
        {
            Debug.LogError("invalid m_sender");
            return;
        }

        if (CurrentState == state)
        {
            Debug.Log("현재 이미 해당 상태입니다  "+ CurrentState);
            return;
        }

        if (CurrentState != null)
        {
            CurrentState.OnExit(m_sender);
        }

        CurrentState = state;

        if (CurrentState != null)
        {
            CurrentState.OnEnter(m_sender);
        }
    }

    public void OnFixedUpdate()
    {
        if (m_sender == null)
        {
            Debug.LogError("invalid m_sener");
            return;
        }
        CurrentState.OnFixedUpdate(m_sender);
    }

    public void OnUpdate()
    {
        if (m_sender == null)
        {
            Debug.LogError("invalid m_sener");
            return;
        }
        CurrentState.OnUpdate(m_sender);
    }
}
