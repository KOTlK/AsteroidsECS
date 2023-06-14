using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.HP.Components;
using Asteroids.Runtime.Utils;
using Asteroids.Runtime.Weapons.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Transform = Asteroids.Runtime.CellLists.Components.Transform;

namespace Asteroids.Runtime.Weapons.Systems
{
    public class ProjectileDestroySystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsFilterInject<Inc<Projectile, ProjectileReference, Destroy, Transform>> _filter = default;
        private readonly EcsPoolInject<ProjectileReference> _references = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        private readonly EcsPoolInject<CellNeighbours> _neighbours = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var reference = ref _references.Value.Get(entity);
                ref var transform = ref _transforms.Value.Get(entity);
                ref var neighbours = ref _neighbours.Value.Get(transform.Cell);
                
                neighbours.ContainingTransforms.Remove(entity);
                reference.Dispose();

                _world.Value.DelEntity(entity);
            }
        }
    }
}