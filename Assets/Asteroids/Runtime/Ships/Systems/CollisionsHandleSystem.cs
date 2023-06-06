using Asteroids.Runtime.Asteroids.Components;
using Asteroids.Runtime.Collisions.Components;
using Asteroids.Runtime.HP.Components;
using Asteroids.Runtime.Ships.Components;
using Asteroids.Runtime.Utils.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.Ships.Systems
{
    public class CollisionsHandleSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<Collision>> _filter = "Physics";
        private readonly EcsPoolInject<Collision> _collisions = "Physics";
        private readonly EcsPoolInject<Buffer<Damage>> _damageBuffers = default;
        private readonly EcsPoolInject<Destroy> _destroyCommands = default;
        private readonly EcsPoolInject<Enemy> _enemies = default;
        private readonly EcsPoolInject<Asteroid> _asteroids = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var collision = ref _collisions.Value.Get(entity);

                switch (collision)
                {
                    case { SenderLayer: PhysicsLayer.Player, ReceiverLayer: PhysicsLayer.Enemy }:
                    {
                        ref var playerDamageBuffer = ref _damageBuffers.Value.Get(collision.Sender);
                        ref var enemy = ref _enemies.Value.Get(collision.Receiver);

                        playerDamageBuffer.Add(enemy.DamageOnPlayerCollision);
                        _destroyCommands.Value.Add(collision.Receiver);
                        break;
                    }
                    case { SenderLayer: PhysicsLayer.Asteroid, ReceiverLayer: PhysicsLayer.Player }:
                    {
                        ref var playerDamageBuffer = ref _damageBuffers.Value.Get(collision.Receiver);
                        ref var asteroid = ref _asteroids.Value.Get(collision.Sender);

                        playerDamageBuffer.Add(asteroid.Damage);
                        _destroyCommands.Value.Add(collision.Sender);
                        break;
                    }
                }
            }
        }
    }
}