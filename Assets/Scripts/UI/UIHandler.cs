using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    StateMachine stateMachine;
    public CanvasGroup pointer,choice,calibration,lostConnection;
    void Start()
    {
        stateMachine = new StateMachine();
        stateMachine.Initialize(new GameplayState(stateMachine,this));
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.CurrentState.HandleInput();
        stateMachine.CurrentState.LogicUpdate();
    }
}
