using System;
using Asteroids.Runtime.Score;
using Asteroids.Runtime.UI;

namespace Asteroids.Runtime.Application.GameLoop.States
{
    public class LoseState : State
    {
        private readonly ViewRoot _viewRoot;
        private readonly GameScore _score;

        public LoseState(ViewRoot viewRoot, GameScore score)
        {
            _viewRoot = viewRoot;
            _score = score;
        }

        public event Action ReturnedToMenu = delegate {  };
        
        public override void Enter()
        {
            _viewRoot.LoseWindow.IsActive = true;
            _score.Visualize(_viewRoot.LoseWindow.Score);
            _viewRoot.LoseWindow.BackToMenu.onClick.AddListener(OnBackToMenuClick);
            _viewRoot.LoseWindow.Quit.onClick.AddListener(OnQuit);
        }

        public override void Exit()
        {
            _viewRoot.LoseWindow.IsActive = false;
            _score.Reset();
            _viewRoot.LoseWindow.BackToMenu.onClick.RemoveListener(OnBackToMenuClick);
            _viewRoot.LoseWindow.Quit.onClick.RemoveListener(OnQuit);
        }

        private void OnQuit()
        {
            UnityEngine.Application.Quit();
        }

        private void OnBackToMenuClick()
        {
            ReturnedToMenu.Invoke();
        }
    }
}