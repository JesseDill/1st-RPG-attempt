using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISavable
    {
        [SerializeField] float healthPoints = -1f;
        BaseStats baseStats;
        private bool isDead = false;
        GameObject instigator;
        bool onlyOnce = false;

        private void Awake()
        {

                healthPoints = GetComponent<BaseStats>().GetStat(Stats.Stats.Health);

        }
        private void Start()
        {
            baseStats = GetComponent<BaseStats>();
            baseStats.onLevelUp += LevelUpHP;
        }

        private void LevelUpHP()
        {
            healthPoints = GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
        }

        public void TakeDamage(GameObject gameObject, float damage)
        {
            instigator = gameObject;
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if(healthPoints <= 0 && !isDead)
            {
                TriggerDeathState();
            }
        }

        public float GetPercentage()
        {
            float percentage = Mathf.Round((healthPoints / GetComponent<BaseStats>().GetStat(Stats.Stats.Health)) * 100);
            return percentage;
        }
        private void TriggerDeathState()
        {
            isDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<Animator>().SetTrigger("Death");
            GiveEXP();
        }

        private void GiveEXP()
        {
            if (!onlyOnce)
            {
                float expReward = GetComponent<BaseStats>().GetStat(Stats.Stats.Exp);
                if (instigator.GetComponent<Experience>() == null) return;
                instigator.GetComponent<Experience>().GainEXP(expReward);
                onlyOnce = true;
            }
        }

        public bool GetIsDead()
        {
            return isDead;
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            float returnHealth = (float)state;
            healthPoints = returnHealth;
            if (returnHealth <= 0)
            {
                TriggerDeathState();
            }
        }
        public float GetHealth()
        {
            return healthPoints;
        }
    }
}
