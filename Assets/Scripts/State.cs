    using UnityEngine;
    using System;
    [Serializable]
    public abstract class State
    {
        protected StateMachine stateMachine;
        protected Transform transform;

        protected State(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public virtual void Enter()
        {

        }

        public virtual void HandleInput()
        {

        }

        public virtual void LogicUpdate()
        {

        }

        public virtual void PhysicsUpdate()
        {

        }

        public virtual void Exit()
        {

        }

    }