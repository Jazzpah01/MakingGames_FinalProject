using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelDescription : MonoBehaviour
{
    public GameObject descriptionPrefab;
    HUD hud;

    [NonSerialized] private List<GameObject> contents = new List<GameObject>();
    private int index = 0;

    private void Start()
    {
        hud = GetComponentInParent<HUD>();
        LevelDescriptionList data = GameManager.instance.levelDescriptionData;

        if (data == null || data.descriptions == null || data.descriptions.Count == 0)
        {
            hud.CloseLevelDescription();
            return;
        }

        for (int i = 0; i < data.descriptions.Count; i++)
        {
            GameObject go = Instantiate(descriptionPrefab, transform.position, Quaternion.identity, transform);
            contents.Add(go);

            LevelDescriptionContent content = go.GetComponentInChildren<LevelDescriptionContent>();
            content.title.text = data.descriptions[i].title;
            content.description.text = data.descriptions[i].description;
            content.image.sprite = data.descriptions[i].image;
        }

        foreach (Transform child in transform)
        {
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
                hud.CloseLevelDescription();
            }
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            hud.CloseLevelDescription();
        }
    }
}
