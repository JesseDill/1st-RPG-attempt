using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] float waypointRadius = .3f;
        public Transform[] transforms;
        private void Start()
        {

        }
        private void OnDrawGizmos()
        {
            transforms = new Transform[transform.childCount];
            for (int i = 1; i < transform.childCount; i++)
            {
                transforms[i - 1] = transform.GetChild(i - 1);

                Gizmos.DrawSphere(transform.GetChild(i-1).position, waypointRadius);
                Gizmos.DrawLine(transform.GetChild(i-1).position, transform.GetChild(i).position);

                if (i == transform.childCount - 1 && transform.childCount > 2)
                {
                    Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(0).position);
                    Gizmos.DrawSphere(transform.GetChild(i).position, waypointRadius);

                    transforms[i] = transform.GetChild(i);
                }
            }
        }
        
    }
}
