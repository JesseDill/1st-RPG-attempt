using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] Weapon weapon = null;

        public CursorType GetCursorType()
        {
            return CursorType.Interact;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.GetComponent<Fighter>());
            }
            return true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                Pickup(other.gameObject.GetComponent<Fighter>());
            }
        }

        private void Pickup(Fighter other)
        {
            other.EquipWeapon(weapon);
            Destroy(gameObject);
        }
    }
}
