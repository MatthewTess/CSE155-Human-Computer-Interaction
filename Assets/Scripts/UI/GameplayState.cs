using System;
using UnityEngine;

public class GameplayState : UIState
{
    float timer;
    public GameplayState(StateMachine stateMachine, UIHandler handler) : base(stateMachine,handler)
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
        if(!tracking){
            timer += Time.deltaTime;
            handler.choice.alpha = Mathf.Lerp(1,0,timer/5f);
        }
        else{
            timer = 0f;
            handler.choice.alpha = 1;
        }
        if(timer > 5f){
            stateMachine.ChangeState(new LostTrackingState(stateMachine,handler));
        }
    }

    public override void Exit()
    {
        Debug.Log("Exited Gameplay State");
    }
}