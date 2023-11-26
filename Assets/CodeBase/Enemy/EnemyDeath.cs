using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator), typeof(EnemyHealth))]
    public class EnemyDeath : MonoBehaviour
    {
        public event Action Happened;
        
        public EnemyHealth Health;
        public EnemyAnimator Animator;

        public GameObject DeathFx;

        private void Start()
        {
            Health.HealthChanged += HealthChanged;
        }

        private void OnDestroy()
        {
            Health.HealthChanged -= HealthChanged;
        }

        private void HealthChanged()
        {
            if (Health.Current <= 0)
                Die();
        }

        private void Die()
        {
            Health.HealthChanged -= HealthChanged;
            Animator.PlayDeath();

            StopMoving();
            SpawnDeathFx();
            StartCoroutine(DestroyTimer());

            Happened?.Invoke();
        }

        private void StopMoving()
        {
            if (gameObject.TryGetComponent(out AgentMoveToPlayer move))
                move.enabled = false;
        }

        private void SpawnDeathFx()
        {
            Instantiate(DeathFx, transform.position, Quaternion.identity);
        }

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }
    }
}