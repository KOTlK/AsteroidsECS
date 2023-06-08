using Asteroids.Runtime.Asteroids.Systems;
using Asteroids.Runtime.CellLists.Systems;
using Asteroids.Runtime.Collisions.Systems;
using Asteroids.Runtime.Enemies.Systems;
using Asteroids.Runtime.GameTime.Systems;
using Asteroids.Runtime.HP.Systems;
using Asteroids.Runtime.Initialization.Systems;
using Asteroids.Runtime.Input.Components;
using Asteroids.Runtime.Input.Systems;
using Asteroids.Runtime.Score;
using Asteroids.Runtime.Score.Systems;
using Asteroids.Runtime.Score.View;
using Asteroids.Runtime.Ships.Systems;
using Asteroids.Runtime.Transforms.Systems;
using Asteroids.Runtime.Utils;
using Asteroids.Runtime.Weapons.Systems;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Mitfart.LeoECSLite.UnityIntegration.Plugins.Mitfart.LeoECSLite.UnityIntegration.Runtime;
using Mitfart.LeoECSLite.UnityIntegration.Plugins.Mitfart.LeoECSLite.UnityIntegration.Runtime.Name; //wtf
using UnityEngine;
using Collision = Asteroids.Runtime.Collisions.Components.Collision;
using Time = Asteroids.Runtime.GameTime.Services.Time;
using Transform = Asteroids.Runtime.CellLists.Components.Transform;

namespace Asteroids.Runtime.Application
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Config _config;
        [SerializeField] private Difficulty _difficulty;
        [SerializeField] private InputMap _inputMap;
        [SerializeField] private ScoreView _scoreView;
        
        private EcsSystems _systems;

        private void Start()
        {
            UnityEngine.Application.targetFrameRate = 0;
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
                .Add(new CellListsInitSystem())
                .Add(new InsertTransformSystem())
                .Add(new RemoveFromCellListsSystem())
                .Add(new CellListsRebuildSystem())
                .Add(new CellDrawSystem())
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
                .Add(new SyncTransformSystem())

#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
                .Add(new EcsWorldDebugSystem(Constants.PhysicsWorldName, new NameSettings(true)))
                .Add(new EcsWorldDebugSystem(Constants.EventsWorldName, new NameSettings(true)))
#endif
                
                .Inject(new Time(), new GameScore(_scoreView) , _config, _difficulty, _inputMap, _camera)
                .Init();
        }

        private void AddInitSystems()
        {
            _systems
                .Add(new WorldCreationSystem())
                .Add(new PlayerInitializationSystem())
                ;
        }

        private void Update()
        {
            _systems.Run();
        }

        private void OnDestroy()
        {
            _systems.Destroy();
        }
    }
}