using System;
using System.Collections;
using UnityEngine;

namespace Asteroids.Runtime.Asteroids.Mono
{
    public class AsteroidView : MonoBehaviour, IDisposable
    {
        [SerializeField] private Animator _animator;

        private static readonly int ExplosionHash = Animator.StringToHash("Explosion");

        public Vector2 ColliderSize;

        public void Dispose()
        {
            _animator.Play(ExplosionHash);
            var length = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            StartCoroutine(Destroying(length));
        }

        private IEnumerator Destroying(float time)
        {
            yield return new WaitForSeconds(time);

            Destroy(gameObject);
        }
    }
}