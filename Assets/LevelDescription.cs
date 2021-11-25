using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDescription : MonoBehaviour
{
    [HideInInspector]public List<GameObject> contents;
    private int index;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            contents.Add(child.gameObject);
        }

        index = contents.Count-1;
        foreach (GameObject go in contents)
        {
            go.SetActive(false);
        }
        contents[index].SetActive(true);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            contents[index].SetActive(false);
            index--;
            if (index >= 0)
            {
                contents[index].SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }
}
