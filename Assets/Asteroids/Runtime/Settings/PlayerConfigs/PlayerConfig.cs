using Asteroids.Runtime.Ships.Components;
using Asteroids.Runtime.Ships.Mono;
using Asteroids.Runtime.Weapons.Components;
using UnityEngine;

namespace Asteroids.Runtime.Settings.PlayerConfigs
{
    [CreateAssetMenu(menuName = "New/PlayerConfig", fileName = "PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        public ShipView PlayerShipPrefab;
        public Ship PlayerShipConfig;
        public Weapon PlayerWeaponConfig;
        public float PlayerHealth;
    }
}