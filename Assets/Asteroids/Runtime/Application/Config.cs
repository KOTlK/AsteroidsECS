using System;
using Asteroids.Runtime.Asteroids.Mono;
using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.Ships.Components;
using Asteroids.Runtime.Weapons.Components;
using UnityEngine;

namespace Asteroids.Runtime.Application
{
    [Serializable]
    public class Config
    {
        public GameObject PlayerShipPrefab;
        public Ship PlayerShipConfig;
        public Weapon PlayerWeaponConfig;
        public float PlayerHealth;
        public CreateCellLists CellListsConfig;
        public AsteroidView[] AsteroidPrefabs;
    }
}