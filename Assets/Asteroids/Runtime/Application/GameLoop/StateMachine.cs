using Asteroids.Runtime.Application.GameLoop.States;

namespace Asteroids.Runtime.Application.GameLoop
{
    public class StateMachine
    {
        private readonly MainMenuState _mainMenuState;
        private readonly GameRunningState _gameRunningState;
        private readonly GamePausedState _gamePausedState;
        private readonly LoseState _loseState;
        private readonly RunningSystems _runningSystems;
        
        private State _executingState;

        public StateMachine(MainMenuState mainMenuState, GameRunningState gameRunningState, GamePausedState gamePausedState, LoseState loseState, RunningSystems runningSystems)
        {
            _mainMenuState = mainMenuState;
            _gameRunningState = gameRunningState;
            _gamePausedState = gamePausedState;
            _loseState = loseState;
            _runningSystems = runningSystems;
            _executingState = mainMenuState;
            _executingState.Enter();
            mainMenuState.NewGameStarted += ToGameRunning;
            gameRunningState.Paused += ToPause;
            gameRunningState.GameLost += ToLose;
            gamePausedState.GameContinued += ToGameRunning;
            gamePausedState.ExitedToMenu += ToMainMenu;
            loseState.ReturnedToMenu += ToMainMenu;
        }


        public void Execute()
        {
            _executingState.Execute();
        }

        private void SwitchState(State state)
        {
            _executingState.Exit();
            _executingState = state;
            _executingState.Enter();
        }

        private void ToGameRunning()
        {
            SwitchState(_gameRunningState);
        }

        private void ToPause()
        {
            SwitchState(_gamePausedState);
        }

        private void ToMainMenu()
        {
            SwitchState(_mainMenuState);
            _runningSystems.Destroy();
        }

        private void ToLose()
        {
            SwitchState(_loseState);
        }
    }
}