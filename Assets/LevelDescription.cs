using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDescription : MonoBehaviour
{
    public List<GameObject> contents = new List<GameObject>();
    private int index = 0;

    private void Start()
    {
        //Debug.Log(contents.Count);
        foreach (Transform child in transform)
        {
            if (!contents.Contains(child.gameObject))
            {
                contents.Add(child.gameObject);
            }
            child.gameObject.SetActive(false);
        }
        contents[index].SetActive(true);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            contents[index].SetActive(false);
            index++;
            if (index < contents.Count)
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
