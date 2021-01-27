using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }
        private IEnumerator LoadLastScene() 
        {
            //IEnumerator allows start to call IEnumerator method without startcouroutine
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
        }
        private void Update()
        {
            CheckForQuickSave();
            CheckForQuickLoad();
        }

        private void CheckForQuickLoad()
        {
            if (Input.GetKeyDown(KeyCode.F8))
            {
                Load();
            }
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void CheckForQuickSave()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                Save();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }
    }
}
