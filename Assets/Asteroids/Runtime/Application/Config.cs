﻿using System;
using Asteroids.Runtime.Ships.Components;
using UnityEngine;

namespace Asteroids.Runtime.Application
{
    [Serializable]
    public class Config
    {
        public GameObject PlayerShipPrefab;
        public Ship PlayerShipConfig;
    }
}