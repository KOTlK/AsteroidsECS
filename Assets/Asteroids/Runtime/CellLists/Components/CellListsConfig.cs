using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.Runtime.CellLists.Components
{
    [Serializable]
    public class CellListsConfig
    {
        public Vector2 Size;
        public Vector2 Center;
        public int Width;
        public int Height;

        private Vector2 HalfExtents => Size * 0.5f;

        public Vector2 RandomPointInside()
        {
            var min = Center - Size * 0.45f;
            var max = Center + Size * 0.45f;

            return new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
        }

        public bool Inside(Vector2 point)
        {
            var min = Center - HalfExtents;
            var max = Center + HalfExtents;
            
            return point.x <= max.x &&
                   point.x >= min.x &&
                   point.y <= max.y &&
                   point.y >= min.y;
        }
    }
}