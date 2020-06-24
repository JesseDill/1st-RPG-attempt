using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float timeBtwnAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;

        Health target;
        Mover mover;
        Weapon currentWeapon;

        float timeSinceLastAttack = Mathf.Infinity;
        bool isAttacking = false;
        private void Start()
        {
            currentWeapon = defaultWeapon;
            EquipWeapon(defaultWeapon);
            mover = GetComponent<Mover>();
        }

        public void EquipWeapon(Weapon weapon)
        {
            if (weapon == null) return;
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            currentWeapon.Spawn(rightHandTransform,leftHandTransform, animator);
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
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
            }
            else 
            {
                target.TakeDamage(currentWeapon.GetWeaponDamage());
            }
        }
        void Shoot()//Animation Event!!!! (can't change name)
        {
            Hit();
        }
    }
}
