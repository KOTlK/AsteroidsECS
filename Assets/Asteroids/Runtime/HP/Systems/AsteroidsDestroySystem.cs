using Asteroids.Runtime.Asteroids.Components;
using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.Collisions.Utils;
using Asteroids.Runtime.HP.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.HP.Systems
{
    public class AsteroidsDestroySystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsFilterInject<Inc<Asteroid, AsteroidReference, Destroy>> _filter = default;
        private readonly EcsFilterInject<Inc<Cell, CellNeighbours>> _cellsFilter = default;
        private readonly EcsPoolInject<Cell> _cells = default;
        private readonly EcsPoolInject<CellNeighbours> _neighbours = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        private readonly EcsPoolInject<AsteroidReference> _asteroidReferences = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var reference = ref _asteroidReferences.Value.Get(entity);
                ref var transform = ref _transforms.Value.Get(entity);
                reference.Dispose();

                foreach (var cellEntity in _cellsFilter.Value)
                {
                    ref var cell = ref _cells.Value.Get(cellEntity);

                    if (CollisionDetection.AABBContainsPoint(cell.Position, cell.AABB, transform.Position))
                    {
                        ref var neighbours = ref _neighbours.Value.Get(cellEntity);
                        neighbours.ContainingTransforms.Remove(entity);
                        break;
                    }
                }
                
                _world.Value.DelEntity(entity);
            }
        }
    }
}