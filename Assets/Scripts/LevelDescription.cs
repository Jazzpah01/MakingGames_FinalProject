using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelDescription : MonoBehaviour
{
    [NonSerialized] private List<GameObject> contents = new List<GameObject>();
    private int index = 0;
    private HUD hud;

    private void Start()
    {
        hud = GetComponentInParent<HUD>();
        hud.isLevelDescriptionActive = true;
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
                hud.isLevelDescriptionActive = false;
                gameObject.SetActive(false);
            }
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            hud.isLevelDescriptionActive = false;
            gameObject.SetActive(false);
        }
    }
}
