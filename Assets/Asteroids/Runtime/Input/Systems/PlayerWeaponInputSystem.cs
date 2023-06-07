using Asteroids.Runtime.Input.Components;
using Asteroids.Runtime.Ships.Components;
using Asteroids.Runtime.Weapons.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.Input.Systems
{
    public class PlayerWeaponInputSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<InputMap> _inputMap = default;
        private readonly EcsFilterInject<Inc<Weapon, WeaponInput, Player>> _filter = default;
        private readonly EcsPoolInject<WeaponInput> _inputs = default;
        
        public void Run(IEcsSystems systems)
        {
            var inputMap = _inputMap.Value;
            
            foreach (var entity in _filter.Value)
            {
                ref var input = ref _inputs.Value.Get(entity);

                input.Shooting = UnityEngine.Input.GetKey(inputMap.Shoot);
                input.Reloading = UnityEngine.Input.GetKeyDown(inputMap.Reload);
            }
        }
    }
}