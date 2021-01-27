using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(0,99)]
        [SerializeField] int currentLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] bool useModifiers = false;
        Experience experience;

        public event Action onLevelUp;

        bool leveledUp = false;

        private void Start()
        {
            experience = GetComponent<Experience>();
            if(experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }
        public float GetStat(Stats stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * GetPercentageModifier(stat);
        } 

        private float GetBaseStat(Stats stat)
        {
            return progression.GetStat(stat, characterClass, currentLevel);
        }

        private void UpdateLevel()
        {
            CalculateLevel();
            if (leveledUp)
            {
                onLevelUp();
                leveledUp = false;
            }
        }
        public int GetLevel()
        {
            if (currentLevel < 1)
            {
                CalculateLevel();
            }
            return currentLevel;
        }

        private float GetAdditiveModifier(Stats stat)
        {
            float total = 0;
            if (!useModifiers) return total;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach(float modifier in provider.GetAdditiveModifier(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }
        private float GetPercentageModifier(Stats stat)
        {
            float total = 1;
            if (!useModifiers) return total;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifier(stat))
                {
                    total *= modifier;
                }
            }
            return total;
        }
        public void CalculateLevel()
        {
            var exp = GetComponent<Experience>();

            float currentExp = exp.GetExp();
            if(currentExp >= progression.GetStat(Stats.LevelUp, characterClass, currentLevel + 1))
            {
                currentLevel++;
                exp.ResetExp();
                leveledUp = true;
            }
        }

    }
}
