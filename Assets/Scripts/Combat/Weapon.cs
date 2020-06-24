using UnityEngine;
using RPG.Core;
namespace RPG.Combat
{
    [CreateAssetMenu(fileName ="Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon: ScriptableObject
    {
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] float weaponDamage = 0f;
        [SerializeField] float weaponRange = 0f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;
        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if(equippedPrefab != null) 
            {
                if (isRightHanded)
                {
                    Instantiate(equippedPrefab, rightHand);
                }
                else
                {
                    Instantiate(equippedPrefab, leftHand);
                }
            }
            if(animatorOverride !=null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            
        }
        public bool HasProjectile()
        {
            return projectile != null;
        }
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
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
            projectileInstance.SetTarget(target, weaponDamage);
        }
        public float GetWeaponDamage()
        {
            return weaponDamage;
        }
        public float GetWeaponRange()
        {
            return weaponRange;
        }

    }
}