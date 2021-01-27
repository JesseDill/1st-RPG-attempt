using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISavable
    {
        [SerializeField] float expPoints = 0;

        //public delegate void ExperienceGainedDelegate();
        public event Action onExperienceGained;

        public void GainEXP(float experience)
        {
            expPoints += experience;
            onExperienceGained();
        }

        public float GetExp()
        {
            return expPoints;
        }
        
        public void ResetExp()
        {
            expPoints = 0;
        }

        public object CaptureState()
        {
            return expPoints;
        }
        public void RestoreState(object state)
        {
            float returnExp = (float)state;
            expPoints = returnExp;
        }
    }
}
 
