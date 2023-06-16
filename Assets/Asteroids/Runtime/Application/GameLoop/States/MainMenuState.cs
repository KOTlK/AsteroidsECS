using System;
using Asteroids.Runtime.AI.Systems;
using Asteroids.Runtime.Asteroids.Systems;
using Asteroids.Runtime.CellLists.Systems;
using Asteroids.Runtime.Collisions.Systems;
using Asteroids.Runtime.Enemies.Systems;
using Asteroids.Runtime.GameCamera.Systems;
using Asteroids.Runtime.GameTime.Systems;
using Asteroids.Runtime.HP.Systems;
using Asteroids.Runtime.Initialization.Systems;
using Asteroids.Runtime.Input.Components;
using Asteroids.Runtime.Input.Systems;
using Asteroids.Runtime.Score;
using Asteroids.Runtime.Score.Systems;
using Asteroids.Runtime.Ships.Systems;
using Asteroids.Runtime.Transforms.Systems;
using Asteroids.Runtime.UI;
using Asteroids.Runtime.Utils;
using Asteroids.Runtime.View.Systems;
using Asteroids.Runtime.Weapons.Systems;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine;
using Collision = Asteroids.Runtime.Collisions.Components.Collision;
using Time = Asteroids.Runtime.GameTime.Services.Time;
using Transform = Asteroids.Runtime.CellLists.Components.Transform;

namespace Asteroids.Runtime.Application.GameLoop.States
{
    public class MainMenuState : State
    {
        private readonly ViewRoot _viewRoot;
        private readonly Camera _camera;
        private readonly InputMap _inputMap;
        private readonly Config _config;
        private readonly DifficultySelector _difficultySelector;
        private readonly PlayerShipSelector _playerShipSelector;
        private readonly RunningSystems _runningSystems;
        private readonly GameScore _score;

        private EcsSystems _systems;
        
        public event Action NewGameStarted = delegate {  };

        public MainMenuState(ViewRoot viewRoot, Camera camera, InputMap inputMap, Config config, DifficultySelector difficultySelector, PlayerShipSelector playerShipSelector, RunningSystems runningSystems, GameScore score)
        {
            _viewRoot = viewRoot;
            _camera = camera;
            _inputMap = inputMap;
            _config = config;
            _difficultySelector = difficultySelector;
            _playerShipSelector = playerShipSelector;
            _runningSystems = runningSystems;
            _score = score;
        }

        public override void Enter()
        {
            _viewRoot.MainMenu.IsActive = true;
            _viewRoot.InGameWindow.IsActive = false;
            _viewRoot.LoseWindow.IsActive = false;
            _viewRoot.PauseWindow.IsActive = false;
            _viewRoot.MainMenu.StartGame.onClick.AddListener(StartNewGame);
            _viewRoot.MainMenu.ExitGame.onClick.AddListener(CloseGame);
        }

        public override void Exit()
        {
            _viewRoot.MainMenu.IsActive = false;
            _viewRoot.MainMenu.StartGame.onClick.RemoveListener(StartNewGame);
            _viewRoot.MainMenu.ExitGame.onClick.RemoveListener(CloseGame);
        }

        private void CloseGame()
        {
            UnityEngine.Application.Quit();
        }
        
        private void StartNewGame()
        {
            _score.Reset();
            _viewRoot.InGameWindow.Score.Display(0);
            var world = new EcsWorld();
            var physicsWorld = new EcsWorld();
            var eventsWorld = new EcsWorld();
            _systems = new EcsSystems(world);
            _systems.AddWorld(physicsWorld, Constants.PhysicsWorldName);
            _systems.AddWorld(eventsWorld, Constants.EventsWorldName);

            var debugEntity = world.NewEntity(); // entity for correct debug visualization
            var transformsPool = world.GetPool<Transform>();
            transformsPool.Add(debugEntity);

            AddInitSystems();
            _systems
                .Add(new TimeSystem())
                .Add(new AsteroidsSpawnSystem())
                .Add(new EnemiesSpawnSystem())
                .Add(new PlayerShipInputSystem())
                .Add(new PlayerWeaponInputSystem())
                .Add(new PatrolSystem())
                .Add(new FollowSystem())
                .Add(new AttackSystem())
                .Add(new PositionRestrictionSystem())
                .Add(new ShipMovementSystem())
                .Add(new ShipRotationSystem())
                .Add(new WeaponRotationSystem())
                .Add(new VelocitySystem())
                .Add(new ShootingSystem())
                .Add(new ShootDelaySystem())
                .Add(new ReloadingSystem())
                .Add(new ProjectileSpawnSystem())
                .Add(new ProjectileMoveSystem())
                .Add(new AsteroidsMovementSystem())
                .Add(new AsteroidsLifeSystem())
                .Add(new InsertTransformSystem())
                .Add(new RemoveFromCellListsSystem())
                .Add(new CellListsRebuildSystem())
                //.Add(new CellDrawSystem())
                //.Add(new CellNeighboursDrawSystem())
                //.Add(new DisplayNeighboursSystem())
                .DelHere<Collision>(Constants.PhysicsWorldName)
                .Add(new AABBCollisionDetectionSystem())
                //.Add(new CollisionsDebugSystem())
                .Add(new CollisionsHandleSystem())
                .Add(new DamageSystem())
                .Add(new ProjectileDestroySystem())
                .Add(new AsteroidsDestroySystem())
                .Add(new EnemyDeathSystem())
                .Add(new EnemyDestroySystem())
                .Add(new ScoreSystem())
                .Add(new BackgroundMovementSystem())
                .Add(new CameraFollowSystem())
                .Add(new SyncTransformSystem())
                .Inject(new Time(), _score, _config, _playerShipSelector.Selected.Config, _difficultySelector.Selected, _inputMap, _camera)
                .Init();
            _runningSystems.Switch(_systems);
            NewGameStarted.Invoke();
        }
        
        private void AddInitSystems()
        {
            _systems
                .Add(new CellListsInitSystem())
                .Add(new PlayerInitializationSystem())
                ;
        }
    }
}