using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 1f;

    Health target = null;

    float damage = 0;
    void Update()
    {
        if (target == null) return;
        LockOnTarget();
        Shoot();
    }

    public void SetTarget(Health target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    private void Shoot()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void LockOnTarget()
    {
        transform.LookAt(GetAimLocation());
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
        if (other.GetComponent<Health>() == target)
        {
            target.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

}
