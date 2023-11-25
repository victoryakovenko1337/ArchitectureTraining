using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class Aggro : MonoBehaviour
    {
        public TriggerObserver TriggerObserver;
        public Follow Follow;

        [SerializeField] private float _cooldown;
        private Coroutine _aggroCoroutine;
        private bool _hasAggroTarget;

        private void Start()
        {
            TriggerObserver.TriggerEnter += TriggerEnter;
            TriggerObserver.TriggerExit += TriggerExit;

            SwitchFollowOff();
        }

        private void TriggerExit(Collider collider)
        {
            if (_hasAggroTarget)
            {
                _hasAggroTarget = false;

                _aggroCoroutine = StartCoroutine(SwitchFollowAfterCooldown());
            }
        }
        private void TriggerEnter(Collider collider)
        {
            if (!_hasAggroTarget)
            {
                _hasAggroTarget = true;
                StopAggroCoroutine();

                SwitchFollowOn();
            }
        }

        private IEnumerator SwitchFollowAfterCooldown()
        {
            yield return new WaitForSeconds(_cooldown);

            SwitchFollowOff();
        }

        private void StopAggroCoroutine()
        {
            if (_aggroCoroutine != null)
            {
                StopCoroutine(_aggroCoroutine);
                _aggroCoroutine = null;
            }
        }

        private void SwitchFollowOn() =>
            Follow.enabled = true;

        private void SwitchFollowOff() =>
            Follow.enabled = false;
    }
}