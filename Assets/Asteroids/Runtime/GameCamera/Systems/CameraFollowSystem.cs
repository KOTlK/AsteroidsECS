using Asteroids.Runtime.Application;
using Asteroids.Runtime.Ships.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Time = Asteroids.Runtime.GameTime.Services.Time;
using Transform = Asteroids.Runtime.CellLists.Components.Transform;

namespace Asteroids.Runtime.GameCamera.Systems
{
    public class CameraFollowSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<Config> _config = default;
        private readonly EcsCustomInject<Time> _time = default;
        private readonly EcsFilterInject<Inc<Transform, Ship, Player>> _filter = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var camera = _config.Value.Camera;
                var cellListsConfig = _config.Value.CellListsConfig;
                ref var transform = ref _transforms.Value.Get(entity);
                var cameraPosition = camera.transform.position;
                var max = cellListsConfig.Center + cellListsConfig.Size * 0.5f;
                var min = cellListsConfig.Center - cellListsConfig.Size * 0.5f;
                var targetX = Mathf.Clamp(transform.Position.x, min.x, max.x);
                var targetY = Mathf.Clamp(transform.Position.y, min.y, max.y);
                
                camera.transform.position = Vector3.Lerp(cameraPosition,
                    new Vector3(targetX, targetY, cameraPosition.z),
                    _config.Value.CameraSpeed * _time.Value.DeltaTime);
            }
        }
    }
}