using System;
using Asteroids.Runtime.Asteroids.Mono;
using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.View.Mono;
using UnityEngine;

namespace Asteroids.Runtime.Application
{
    [Serializable]
    public class Config
    {
        public Camera Camera;
        public Background Background;
        public float MaxBackgroundSpeed;
        public float CameraSpeed;
        public CellListsConfig CellListsConfig;
        public AsteroidView[] AsteroidPrefabs;
    }
}