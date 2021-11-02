using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnvironmentGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject NonInteractiveEnvironmentObject;

    [SerializeField]
    private Vector2 spawnField;

    [SerializeField]
    int bush, grass, rock, tree;

    private List<GameObject> environmentObjects;

    // Start is called before the first frame update
    void Start()
    {
        environmentObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (environmentObjects.Count > 0)
            {
                foreach (GameObject EO in environmentObjects)
                    Destroy(EO);
                environmentObjects.Clear();
            }
            SpawnObjects(bush, 0,2);
            SpawnObjects(grass, 3,5);
            SpawnObjects(rock, 6,8);
            SpawnObjects(tree, 9,12);
        }
    }

    private void SpawnObjects(int n, int rangeX, int rangeY)
    {
        for (int i = 0; i < n; i++)
        {
            Vector3 v3 = new Vector3(UnityEngine.Random.Range(spawnField.x,spawnField.y),0, UnityEngine.Random.Range(spawnField.x,spawnField.y));
            Vector3 rotation = new Vector3(transform.rotation.x, transform.rotation.y + UnityEngine.Random.Range(0, 360), transform.rotation.z);
            GameObject go = Instantiate(NonInteractiveEnvironmentObject, v3, Quaternion.Euler(rotation), transform);
            go.GetComponent<NotInteractiveEnvironmentObject>().SetObject(UnityEngine.Random.Range(rangeX, rangeY));
            environmentObjects.Add(go);
        }
        
    }
}
