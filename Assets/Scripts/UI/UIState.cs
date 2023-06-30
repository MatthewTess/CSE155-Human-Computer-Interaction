using UnityEngine;

public class UIState : State
{
    protected UIHandler handler;
    public UIState(StateMachine stateMachine, UIHandler handler) : base(stateMachine)
    {
        this.handler = handler;
    }
}