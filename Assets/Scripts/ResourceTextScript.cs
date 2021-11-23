using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceTextScript : MonoBehaviour
{
    GameManager gameManager;
    TextMeshProUGUI TMP;

    public string message = "Nectar Essence: ";

    float tempOld = -1;

    void Start()
    {
        TMP = gameObject.GetComponent<TextMeshProUGUI>();
        gameManager = GameManager.instance;
    }

    void Update()
    {
        if (tempOld != gameManager.resource)
        {
            TMP.SetText(message + gameManager.resource);
            tempOld = gameManager.resource;
        }
    }
}
