using System;
using UnityEngine;

public class LostTrackingState : UIState
{
    float timer;
    public LostTrackingState(StateMachine stateMachine, UIHandler handler) : base(stateMachine,handler)
    {
    }
    public override void Enter()
    {
        Debug.Log("Entered Gameplay State");
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        bool tracking = Gaze.IsTracking();
        if(tracking){
            stateMachine.ChangeState(new GameplayState(stateMachine,handler));
        }
    }

    public override void Exit()
    {
        Debug.Log("Exited Gameplay State");
    }
}