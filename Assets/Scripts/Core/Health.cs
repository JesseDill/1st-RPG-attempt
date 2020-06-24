using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISavable
    {
        [SerializeField] float health = 100f;
        private bool isDead = false;

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            if(health <= 0 && !isDead)
            {
                TriggerDeathState();
            }
        }

        private void TriggerDeathState()
        {
            isDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<Animator>().SetTrigger("Death");
        }
        public bool GetIsDead()
        {
            return isDead;
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            float returnHealth = (float)state;
            if (returnHealth <= 0)
            {
                TriggerDeathState();
            }
            health = returnHealth;
        }
    }
}
