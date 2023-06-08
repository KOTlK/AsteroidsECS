using Asteroids.Runtime.Weapons.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Transform = Asteroids.Runtime.CellLists.Components.Transform;

namespace Asteroids.Runtime.Weapons.Systems
{
    public class WeaponRotationSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<Weapon, WeaponInput, Transform>> _filter = default;
        private readonly EcsPoolInject<WeaponInput> _weaponInputs = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var weaponInput = ref _weaponInputs.Value.Get(entity);
                ref var transform = ref _transforms.Value.Get(entity);

                weaponInput.LookDirection = transform.Up;
            }
        }
    }
}