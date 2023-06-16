using System;
using Asteroids.Runtime.UI;

namespace Asteroids.Runtime.Application.GameLoop.States
{
    public class GamePausedState : State
    {
        private readonly ViewRoot _viewRoot;

        public GamePausedState(ViewRoot viewRoot)
        {
            _viewRoot = viewRoot;
        }

        public event Action GameContinued = delegate {  };
        public event Action ExitedToMenu = delegate {  };
        
        public override void Enter()
        {
            _viewRoot.PauseWindow.IsActive = true;
            _viewRoot.PauseWindow.Continue.onClick.AddListener(Continue);
            _viewRoot.PauseWindow.ExitToMenu.onClick.AddListener(ExitToMenu);
            _viewRoot.PauseWindow.ExitGame.onClick.AddListener(Quit);
        }

        public override void Exit()
        {
            _viewRoot.PauseWindow.IsActive = false;
            _viewRoot.PauseWindow.Continue.onClick.RemoveListener(Continue);
            _viewRoot.PauseWindow.ExitToMenu.onClick.RemoveListener(ExitToMenu);
            _viewRoot.PauseWindow.ExitGame.onClick.RemoveListener(Quit);
        }

        private void Continue()
        {
            GameContinued.Invoke();
        }

        private void ExitToMenu()
        {
            ExitedToMenu.Invoke();
        }

        private void Quit()
        {
            UnityEngine.Application.Quit();
        }
    }
}