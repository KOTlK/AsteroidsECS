using System;
using System.Collections;
using UnityEngine;

namespace Asteroids.Runtime.Weapons.Mono
{
    public class ProjectileView : MonoBehaviour, IDisposable
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _explosionTime = 0.45f;

        private static readonly int ExplosionHash = Animator.StringToHash("BulletExplosion");

        public void Dispose()
        {
            _animator.Play(ExplosionHash);
            StartCoroutine(Destroying(_explosionTime));
        }

        private IEnumerator Destroying(float time)
        {
            yield return new WaitForSeconds(time);

            Destroy(gameObject);
        }
    }
}