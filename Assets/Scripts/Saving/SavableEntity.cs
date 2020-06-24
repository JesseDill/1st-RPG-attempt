using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using RPG.Core;
using UnityEngine.AI;
using System;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SavableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";

        //for referencing all UUIDs
        static Dictionary<string, SavableEntity> globalLookup = new Dictionary<string, SavableEntity>();
        
        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }
        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach(ISavable savable in GetComponents<ISavable>())
            {
                state[savable.GetType().ToString()] = savable.CaptureState();
            }
            return state;
        }
        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>) state;
            foreach (ISavable savable in GetComponents<ISavable>())
            {
                string typeString = savable.GetType().ToString();
                if (stateDict.ContainsKey(typeString)) 
                {
                    savable.RestoreState(stateDict[typeString]);
                }
            }
        }

#if UNITY_EDITOR //removes this section of code when packaging build
        private void Update()
        {  
            if (Application.isPlaying) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return; //returns if its a prefab

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            if(string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }
            globalLookup[property.stringValue] = this;
        }
#endif
        private bool IsUnique(string candidate)
        {
            if (!globalLookup.ContainsKey(candidate)) return true; //checks if UUID exists

            if (globalLookup[candidate] == this) return true; //checks if UUID matches with current entity
           
            if(globalLookup[candidate] == null)//if object has been deleted (loading different scenes)
            {
                globalLookup.Remove(candidate);
                return true;
            }

            //if for some reason UUID doesn't match up 
            if (globalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                globalLookup.Remove(candidate);
                return true;
            }

            return false;
        }

    }
}
