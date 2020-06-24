using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.Core;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] float timePerLocation = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 3f;
        [SerializeField] float patrolSpeed = 3f;
        [SerializeField] float chaseSpeed = 4f;

        GameObject player;
        Fighter fighter;
        Health health;
        Mover mover;

        Vector3 guardPosition;
        Vector3 nextPostion;

        float playerDistance;
        float lastTimePlayerSeen = Mathf.Infinity;
        float timeAtPosition = 0;
        int reference;

        private void Start()
        {
            health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();

            guardPosition = transform.position;
            nextPostion = guardPosition;
        }
        private void Update()
        {
            playerDistance = Vector3.Distance(player.transform.position, transform.position);
            CheckDistance();
        }

        private void CheckDistance()
        {
            if (health.GetIsDead()) return;
            if (playerDistance <= chaseDistance)
            {
                lastTimePlayerSeen = 0;
                AttackBehavior();
            }
            else if (lastTimePlayerSeen <= suspicionTime)
            {
                SuspicionBehavior();
            }
            else
            {
                PatrolBehavior();
            }
            lastTimePlayerSeen += Time.deltaTime;
        }

        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehavior()
        {
            mover.SetMovementSpeed(chaseSpeed);
            fighter.Attack(player);
        }

        private void PatrolBehavior()
        {
            if(patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeAtPosition += Time.deltaTime;
                    if (timeAtPosition < timePerLocation) return;
                    CycleWaypoint();
                    timeAtPosition = 0;
                }
                nextPostion = GetCurentWaypoint();
            }
            mover.SetMovementSpeed(patrolSpeed);
            mover.StartMoveAction(nextPostion);
        }

        private Vector3 GetCurentWaypoint()
        {
            return patrolPath.transforms[reference].position;
            
        }

        private void CycleWaypoint()
        {
            if (reference >= patrolPath.transforms.Length - 1)
                reference = 0;
            else
                reference++;
        }

        private bool AtWaypoint()
        {
            return Vector3.Distance(transform.position, new Vector3(nextPostion.x,transform.position.y, nextPostion.z)) <= waypointTolerance;
        }

        //called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

    }

}