﻿using System;
using Asteroids.Runtime.HP.Components;
using UnityEngine;

namespace Asteroids.Runtime.Application
{
    [Serializable]
    [CreateAssetMenu(menuName = "New/Difficulty", order = 0)]
    public class Difficulty : ScriptableObject
    {
        public Damage AsteroidsDamage;
        public float AsteroidsSpeed;
        public float AsteroidsLifeTime;
        public int AsteroidsSpawnMinDelay;
        public int AsteroidsSpawnMaxDelay;
        public int AsteroidsAmountPerSpawn;
    }
}