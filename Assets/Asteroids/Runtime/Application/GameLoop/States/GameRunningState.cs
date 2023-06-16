using System;
using Asteroids.Runtime.HP.Components;
using Asteroids.Runtime.Input.Components;
using Asteroids.Runtime.Ships.Components;
using Asteroids.Runtime.UI;

namespace Asteroids.Runtime.Application.GameLoop.States
{
    public class GameRunningState : State
    {
        private readonly ViewRoot _viewRoot;
        private readonly InputMap _inputMap;
        private readonly RunningSystems _systems;

        public GameRunningState(ViewRoot viewRoot, InputMap inputMap, RunningSystems systems)
        {
            _viewRoot = viewRoot;
            _inputMap = inputMap;
            _systems = systems;
        }

        public event Action Paused = delegate {  };
        public event Action GameLost = delegate {  };

        public override void Enter()
        {
            _viewRoot.InGameWindow.IsActive = true;
        }

        public override void Exit()
        {
            _viewRoot.InGameWindow.IsActive = false;
        }

        public override void Execute()
        {
            _systems.Execute();

            if (UnityEngine.Input.GetKeyDown(_inputMap.PauseGame))
            {
                Paused.Invoke();
            }
            
            var world = _systems.Systems.GetWorld();
            var filter = world.Filter<Player>().Inc<Health>().Inc<HealthOver>().End();

            if (filter.GetEntitiesCount() > 0)
            {
                GameLost.Invoke();
            }
        }
    }
}