using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneToLoad = 0;
        Transform spawnPoint;
        [SerializeField] DestinationIdentifier destinationIdentifier;
        SavingWrapper saveWrapper;

        enum DestinationIdentifier
        {
            A,B,C,D,E
        }

        private void Awake()
        {
            spawnPoint = transform.GetChild(0);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);

            saveWrapper = FindObjectOfType<SavingWrapper>();
            saveWrapper.Save(); 

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            saveWrapper.Load();
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            saveWrapper.Save();

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        private Portal GetOtherPortal()
        {
            var portals = FindObjectsOfType<Portal>();
            foreach(Portal portal in portals)
            {
                if (portal == this) continue;
                if (portal.destinationIdentifier != destinationIdentifier) continue;

                return portal;
            }
            return null;
        }
    }
}