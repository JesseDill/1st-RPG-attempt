using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter player;
        private void Awake()
        {
            player = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }
        private void Update()
        {   
            if (player.GetTarget() != null)
            GetComponent<Text>().text = player.GetTarget().GetPercentage() + "%";
        }
    }
}
