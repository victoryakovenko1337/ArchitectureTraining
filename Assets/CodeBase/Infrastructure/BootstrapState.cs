using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class BootstrapState : IState
    {
        private GameStateMachine _stateMachine;

        public BootstrapState(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            RegisterServices();
        }

        public void Exit()
        {
            
        }

        private void RegisterServices()
        {
            Game.InputService = RegisterInputService();
        }

        private static IInputService RegisterInputService()
        {
            if (Application.isEditor)
            {
                return new StandaloneInputService();
            }
            else
            {
                return new MobileInputService();
            }
        }
    }
}