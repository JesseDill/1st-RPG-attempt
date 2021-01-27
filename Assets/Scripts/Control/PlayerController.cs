using RPG.Core;
using RPG.Combat;
using RPG.Movement;
using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Fighter fighter;
        Health health;
        [SerializeField] float navMeshPlayerProjectionDistance = 1f;
        [SerializeField] float maxNavPathLength = 40f;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;

        private static Ray GetMouseRay()
        {
            Vector3 screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            return Camera.main.ScreenPointToRay(screenPoint);
        }

        void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            //action priority
            if (InteractWithUI()) return;
            if (health.GetIsDead()) 
            {
                SetCursor(CursorType.None);
                return;
            }
            if (InteractWithComponent()) return;
            if(InteractWithMovement()) return;
            SetCursor(CursorType.None);
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] iRaycastables = hit.transform.GetComponents<IRaycastable>();
                foreach(IRaycastable raycastable in iRaycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;    
                    }
                }
            }
            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hits.Length];

            for(int i=0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            //SortArray(distances);

            Array.Sort(distances, hits);

            return hits;
        }

        //private void SortArray(float[] distances)
        //{//bubble sort
        //    for(int i = 0; i < distances.Length-1; i++)
        //    {
        //        for(int j = 0; j<distances.Length - i - 1; j++)
        //        {
        //            if(distances[j] > distances[j + 1])
        //            {//swaps places
        //                float temp = distances[j];
        //                distances[j] = distances[j + 1];
        //                distances[j + 1] = temp;
        //            }
        //        }
        //    }
        //}

        private bool InteractWithUI()
        {
            if(EventSystem.current.IsPointerOverGameObject()) //checks for UI
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        //private bool InteractWithCombat()
        //{
         
        //    RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
        //    foreach (RaycastHit hit in hits)
        //    {
        //        CombatTarget target = hit.transform.GetComponent<CombatTarget>();
        //        if (target == null) continue;
        //        if (target.GetComponent<Health>().GetIsDead()) continue;

        //        if (Input.GetMouseButton(0))
        //        {
        //            fighter.Attack(target.gameObject);
        //        }
        //        SetCursor(CursorType.Combat);
        //        return true;
        //    }
        //    return false;
        //}

        private bool InteractWithMovement()
        {
            //RaycastHit hit;
            //bool hasHit = Physics.Raycast(GetMouseRay(), out hit); //hit = whatever the ray collides with in real world
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                        GetComponent<Mover>().StartMoveAction(target); 
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }
        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool closeEnoughToNavMesh = NavMesh.SamplePosition(
                hit.point, out navMeshHit, navMeshPlayerProjectionDistance, NavMesh.AllAreas);

            if(!closeEnoughToNavMesh) return false;

            target = navMeshHit.position;

            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if(path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLength) return false;

            return true;

        }

        private float GetPathLength(NavMeshPath path)
        {
            float totalDistance = 0;
            if (path.corners.Length < 2) return totalDistance;
            for(int i=0; i < path.corners.Length - 1; i++)
            {
                totalDistance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            return totalDistance;   
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach(CursorMapping mapping in cursorMappings)
            {
                if(mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }
    }

}