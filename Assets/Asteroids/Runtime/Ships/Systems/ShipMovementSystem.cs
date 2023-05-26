using Asteroids.Runtime.Input.Components;
using Asteroids.Runtime.Ships.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Time = Asteroids.Runtime.GameTime.Services.Time;

namespace Asteroids.Runtime.Ships.Systems
{
    public class ShipMovementSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<Time> _time = default;
        private readonly EcsFilterInject<Inc<Ship, ShipInput>> _filter = default;
        private readonly EcsPoolInject<Ship> _ships = default;
        private readonly EcsPoolInject<ShipInput> _inputs = default;
        
        public void Run(IEcsSystems systems)
        {
            var time = _time.Value;
            foreach (var entity in _filter.Value)
            {
                ref var ship = ref _ships.Value.Get(entity);
                ref var input = ref _inputs.Value.Get(entity);
                var velocity = ship.Velocity;

                if (input.MovementDirection == Vector2.zero)
                {
                    velocity = Vector2.MoveTowards(velocity, Vector2.zero, ship.Damping * time.DeltaTime);
                }
                else
                {
                    if (input.MovementDirection.x == 0)
                    {
                        velocity.x = Mathf.MoveTowards(velocity.x, 0, ship.Damping * time.DeltaTime);
                    } 
                    else if (input.MovementDirection.y == 0)
                    {
                        velocity.y = Mathf.MoveTowards(velocity.y, 0, ship.Damping * time.DeltaTime);
                    }
                    
                    velocity += input.MovementDirection * ship.Acceleration * time.DeltaTime;
                }

                velocity = Vector2.ClampMagnitude(velocity, ship.MaxSpeed);
                ship.Velocity = velocity;
            }
        }
    }
}