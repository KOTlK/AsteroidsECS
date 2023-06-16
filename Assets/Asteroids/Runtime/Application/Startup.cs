using Asteroids.Runtime.Application.GameLoop;
using Asteroids.Runtime.Application.GameLoop.States;
using Asteroids.Runtime.Input.Components;
using Asteroids.Runtime.Score;
using Asteroids.Runtime.UI;
using UnityEngine;

namespace Asteroids.Runtime.Application
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Config _config;
        [SerializeField] private InputMap _inputMap;
        [SerializeField] private ViewRoot _viewRoot;
        [SerializeField] private DifficultySelector _difficultySelector;
        [SerializeField] private PlayerShipSelector _playerShipSelector;

        private GameScore _score;
        private StateMachine _stateMachine;
        private RunningSystems _runningSystems;

        private void Start()
        {
            UnityEngine.Application.targetFrameRate = 0;

            _stateMachine = new StateMachine(
                new MainMenuState(
                    _viewRoot,
                    _camera,
                    _inputMap,
                    _config,
                    _difficultySelector,
                    _playerShipSelector,
                    _runningSystems = new RunningSystems(),
                    _score = new GameScore(_viewRoot.InGameWindow.Score)),
                new GameRunningState(
                    _viewRoot,
                    _inputMap,
                    _runningSystems),
                new GamePausedState(
                    _viewRoot),
                new LoseState(
                    _viewRoot,
                    _score),
                _runningSystems);
        }

        

        private void Update()
        {
            _stateMachine.Execute();
        }

        private void OnDestroy()
        {
            _runningSystems.Destroy();
        }
    }
}