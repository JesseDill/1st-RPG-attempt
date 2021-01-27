using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience exp;
        private void Awake()
        {
            exp = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }
        private void Update()
        {
            GetComponent<Text>().text = exp.GetExp().ToString();
        }
    }
}
