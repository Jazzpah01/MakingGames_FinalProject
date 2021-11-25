using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDescription : MonoBehaviour
{
    public GameObject[] contents;
    private int index;

    private void Start()
    {
        index = contents.Length-1;
        Debug.Log(index);
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
