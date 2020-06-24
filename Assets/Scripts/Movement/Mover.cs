using RPG.Core;
using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISavable
    {
        NavMeshAgent navMeshAgent;
        Health health;
        Vector3 localVelocity;
        void Start()
        {
            health = GetComponent<Health>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }


        void Update()
        {
            navMeshAgent.enabled = !health.GetIsDead();
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }
        public void SetMovementSpeed(float speed)
        {
            navMeshAgent.speed = speed;
        }
        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.SetDestination(destination);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

    
        private void UpdateAnimator()
        {
            localVelocity = transform.InverseTransformDirection(navMeshAgent.velocity);
            float speedInZ = Mathf.Abs(localVelocity.z);
            GetComponent<Animator>().SetFloat("forwardSpeed", speedInZ);
        }

        public object CaptureState()
        {
            return new SerializableVector(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector vector3 = (SerializableVector)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = vector3.DeserializeVector();
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
