using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelDescription : MonoBehaviour
{
    public GameObject descriptionPrefab;

    [NonSerialized] private List<GameObject> contents = new List<GameObject>();
    private int index = 0;
    private HUD hud;

    public static LevelDescription instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        LevelDescriptionList data = GameManager.instance.levelDescriptionData;

        if (data == null || data.descriptions == null || data.descriptions.Count == 0)
        {
            gameObject.SetActive(false);
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

        hud = GetComponentInParent<HUD>();
        hud.isLevelDescriptionActive = true;
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

    private void SetDescriptionValues(DescriptionData data)
    {

    }
}
