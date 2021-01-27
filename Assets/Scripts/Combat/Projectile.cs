using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1f;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitFX = null;

        Health target = null;
        GameObject instigator = null;

        float damage = 0;
        bool firstShot = false;

        void Update()
        {
            if (target == null) return;
            LockOnTarget();
            Shoot();
        }

        public void SetTarget(Health target,GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
        }

        private void Shoot()
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }

        private void LockOnTarget()
        {
            if (isHoming && !target.GetIsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            else if (!isHoming && !firstShot)
            {
                transform.LookAt(GetAimLocation());
                firstShot = true;
            }
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }
        private void OnTriggerEnter(Collider other)
        {
            Health targetHealth = other.GetComponent<Health>();
            if (targetHealth == target)
            {
                if (!targetHealth.GetIsDead())
                {
                    target.TakeDamage(instigator, damage);
                    GameObject contactFX = Instantiate(hitFX, GetAimLocation(), transform.rotation);
                    Destroy(contactFX, contactFX.GetComponent<ParticleSystem>().main.startLifetime.constant);
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

    }
}
