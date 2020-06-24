using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantObjectSpawner : MonoBehaviour
{

    [SerializeField] GameObject persistantGameObjectPrefab;

    static bool hasSpawned = false;
    void Awake()
    {
        if (hasSpawned) return;

        SpawnPersistantObjects();
        hasSpawned = true;
    }

    private void SpawnPersistantObjects()
    {
        GameObject persistantObject = Instantiate(persistantGameObjectPrefab);
        DontDestroyOnLoad(persistantObject);
    }

}
