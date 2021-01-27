using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using RPG.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, IModifierProvider
    {
        [SerializeField] float timeBtwnAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] string defaultWeaponName = "Unarmed";

        Health target;
        Mover mover;
        Weapon currentWeapon;

        float timeSinceLastAttack = Mathf.Infinity;
        bool isAttacking = false;
        private void Start()
        {
            //looks fore weapon with name in resource folder
            //unity engine makes sure command doesn't look for namespace by default
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(defaultWeaponName);
            currentWeapon = defaultWeapon;
            EquipWeapon(weapon);
            mover = GetComponent<Mover>();
        }

        public void EquipWeapon(Weapon weapon)
        {
            if (weapon == null) return;
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            currentWeapon.Spawn(rightHandTransform,leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return target;
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.GetIsDead())
            {
                Cancel();
                return;
            }
            if (!isAttacking) return;
            
            bool inRange = Vector3.Distance(transform.position, target.transform.position) <= currentWeapon.GetWeaponRange();
            if (inRange)
            {
                TurnToTarget();
                mover.Cancel();
                AttackBehavior();
            }
            else
            {
                GetComponent<Animator>().ResetTrigger("Attack");
                GetComponent<Animator>().SetTrigger("EndAttack");
                mover.MoveTo(target.transform.position);
            }

        }

        private void AttackBehavior()
        {
            if (timeSinceLastAttack >= timeBtwnAttacks) {
                //GetComponent<Animator>().ResetTrigger("EndAttack");
                GetComponent<Animator>().SetTrigger("Attack");
                timeSinceLastAttack = 0;
            }
        }

        private void TurnToTarget()
        {
            transform.LookAt(target.transform);
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
            isAttacking = true;
        }
        public void Cancel()
        {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("EndAttack");
            isAttacking = false;
        }
        void Hit() //Animation Event!!!!
        {
            if (target == null) return;
            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject);
            }
            else 
            {
                target.TakeDamage(gameObject, currentWeapon.GetWeaponDamage());
            }
        }
        void Shoot()//Animation Event!!!! (can't change name)
        {
            Hit();
        }
        public bool GetIsAttack()
        {
            return isAttacking;
        }
        public Weapon GetCurrentWeapon()
        {
            return currentWeapon;
        }

        public IEnumerable<float> GetAdditiveModifier(Stats.Stats stat)
        {
            //ienumerator doesnt always have to return something!
            if (stat == Stats.Stats.Damage)
            {
                yield return currentWeapon.GetWeaponDamage();
            }
        }
        public IEnumerable<float> GetPercentageModifier(Stats.Stats stat)
        {
            if(stat == Stats.Stats.Damage)
            {
                yield return currentWeapon.GetPercentageModifier();
            }
        }
    }
}
