using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.GameTime.Services;
using Asteroids.Runtime.HP.Components;
using Asteroids.Runtime.Weapons.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.Weapons.Systems
{
    public class ProjectileMoveSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<Time> _time = default;
        private readonly EcsFilterInject<Inc<Projectile, Transform>> _filter = default;
        private readonly EcsPoolInject<Projectile> _projectiles = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        private readonly EcsPoolInject<Destroy> _destroy = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var projectile = ref _projectiles.Value.Get(entity);
                ref var transform = ref _transforms.Value.Get(entity);

                var distance = projectile.Speed * _time.Value.DeltaTime;

                transform.Position += projectile.Direction * distance;
                projectile.DistancePassed += distance;

                if (projectile.DistancePassed >= projectile.MaxDistance)
                {
                    _destroy.Value.Add(entity);
                }
            }
        }
    }
}