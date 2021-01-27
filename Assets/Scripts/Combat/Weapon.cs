using UnityEngine;
using RPG.Attributes;
using System;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName ="Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon: ScriptableObject
    {
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] float weaponDamage = 0f;
        [SerializeField] float weaponRange = 0f;
        [SerializeField] float percentageEffect = 1f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            if(equippedPrefab != null) 
            {
                if (isRightHanded)
                {
                    GameObject weapon = Instantiate(equippedPrefab, rightHand);
                    weapon.name = weaponName;
                }
                else
                {
                    GameObject weapon = Instantiate(equippedPrefab, leftHand);
                    weapon.name = weaponName;
                }
            }
            if(animatorOverride !=null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else
            {
                // var is null if it's just character animator controller
                var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
                if(overrideController != null)
                {
                    //runtime animator controller for overrider is just default controller
                    //seting animator to defaul in other words
                    animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
                }
            }
            
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if(oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";//makes sure there's no name confusion
            Destroy(oldWeapon.gameObject);
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator)
        {
            Projectile projectileInstance;
            if (isRightHanded)
            {
                projectileInstance = Instantiate(projectile, rightHand.position,Quaternion.identity);
            }
            else
            {
                projectileInstance = Instantiate(projectile, leftHand.position, Quaternion.identity);
            }
            projectileInstance.SetTarget(target, instigator, weaponDamage);
        }
        public float GetWeaponDamage()
        {
            return weaponDamage;
        }
        public float GetWeaponRange()
        {
            return weaponRange;
        }
        
        public float GetPercentageModifier()
        {
            return percentageEffect;
        }

    }
}