using UnityEngine;
using System;
using System.Collections.Generic;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stats, float[]>> lookupTable = null;
        public float GetStat(Stats stat, CharacterClass characterClass, int level)
        {
            BuildLookup();

            float[] levels = lookupTable[characterClass][stat];

            if (levels.Length < level) 
            {
                return 0; //no level exists for it
            }
            return levels[level - 1];

            //foreach(ProgressionCharacterClass progressionClass in characterClasses)
            //{
            //    if (progressionClass.characterClass != characterClass) continue;

            //    foreach (ProgressionStat progressionStat in progressionClass.stats)
            //    {
            //        if (progressionStat.levels.Length < level) continue;
            //        return progressionStat.levels[level - 1];
            //    }             

            //}
            //return 0;
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stats, float[]>>();

            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                var lookupStat = new Dictionary<Stats, float[]>();
                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    lookupStat[progressionStat.stat] = progressionStat.levels;
                }
                lookupTable[progressionClass.characterClass] = lookupStat;

            }

        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            
            public ProgressionStat[] stats; 
        }
        [System.Serializable] 
        class ProgressionStat
        {
            public Stats stat;
            public float[] levels;
        }
    }

}