using Asteroids.Runtime.Ships.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Transform = Asteroids.Runtime.CellLists.Components.Transform;

namespace Asteroids.Runtime.Input.Systems
{
    public class PlayerShipInputSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<Camera> _camera = default;
        private readonly EcsFilterInject<Inc<ShipInput, Player, Transform>> _filter = default;
        private readonly EcsPoolInject<ShipInput> _inputs = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var input = ref _inputs.Value.Get(entity);
                ref var transform = ref _transforms.Value.Get(entity);
                var horizontal = UnityEngine.Input.GetAxis("Horizontal");
                var vertical = UnityEngine.Input.GetAxis("Vertical");
                var mousePosition = _camera.Value.ScreenToViewportPoint(UnityEngine.Input.mousePosition);
                var shipPosition = _camera.Value.WorldToViewportPoint(transform.Position);
                var direction = mousePosition - shipPosition;
                direction.z = 0;
                
                input.MovementDirection = new Vector2(horizontal, vertical);
                input.LookDirection = direction.normalized;
            }
        }
    }
}