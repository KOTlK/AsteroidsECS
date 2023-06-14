using Asteroids.Runtime.Ships.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Time = Asteroids.Runtime.GameTime.Services.Time;
using Transform = Asteroids.Runtime.CellLists.Components.Transform;

namespace Asteroids.Runtime.Ships.Systems
{
    public class ShipRotationSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<Time> _time = default;
        private readonly EcsFilterInject<Inc<Ship, ShipInput, Transform>> _filter = default;
        private readonly EcsPoolInject<Ship> _ships = default;
        private readonly EcsPoolInject<ShipInput> _inputs = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var input = ref _inputs.Value.Get(entity);
                ref var transform = ref _transforms.Value.Get(entity);
                ref var ship = ref _ships.Value.Get(entity);
                var currentRotation = transform.Rotation;
                var direction = input.LookDirection;
                var targetRotation = Quaternion.FromToRotation(Vector3.up, direction);

                transform.Rotation = Quaternion.Slerp(currentRotation, targetRotation,
                    ship.RotationSpeed * _time.Value.DeltaTime);
            }
        }
    }
}