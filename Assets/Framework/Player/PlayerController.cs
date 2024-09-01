using System;
using Framework.Behaviours.Movement;
using Framework.State_Machine;
using Project.Utils;
using UnityEngine;

namespace Framework.Player
{
    public class PlayerController : MonoBehaviour
    {
        private readonly StateMachine _stateMachine = new StateMachine();

        [field:SerializeField] public PlayerMovement PlayerMovement { get; private set; }

        private void Awake()
        {
            var locomotionState = new PlayerLocomotionState(this);
            var dashState = new PlayerDashState(this);
            
            //_stateMachine.AddTransition(locomotionState, dashState, new FuncPredicate(() => ));
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }
    }

    public abstract class PlayerStateMachine : BaseState
    {
        protected readonly PlayerController playerController;

        protected PlayerStateMachine(PlayerController playerController)
        {
            this.playerController = playerController;
        }
        
        public override void OnEnter()
        {
            
        }

        public override void Update()
        {
            
        }

        public override void FixedUpdate()
        {

        }

        public override void OnExit()
        {
            
        }
    }

    public class PlayerLocomotionState : PlayerStateMachine
    {
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            playerController.PlayerMovement.Move();
            playerController.PlayerMovement.Rotate();
        }

        public PlayerLocomotionState(PlayerController playerController) : base(playerController)
        {
        }
    }

    public class PlayerDashState : PlayerStateMachine
    {
        public PlayerDashState(PlayerController playerController) : base(playerController)
        {
        }
    }
}
