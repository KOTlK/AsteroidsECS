using Asteroids.Runtime.Input.Components;
using Asteroids.Runtime.Ships.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Asteroids.Runtime.Input.Systems
{
    public class PlayerShipInputSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ShipInput, Player>> _filter = default;
        private readonly EcsPoolInject<ShipInput> _inputs = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var input = ref _inputs.Value.Get(entity);

                var horizontal = UnityEngine.Input.GetAxis("Horizontal");
                var vertical = UnityEngine.Input.GetAxis("Vertical");

                input.MovementDirection = new Vector2(horizontal, vertical);
            }
        }
    }
}