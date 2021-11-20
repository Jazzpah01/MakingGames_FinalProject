using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceTextScript : MonoBehaviour
{
    TextMeshProUGUI TMP;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        TMP = this.gameObject.GetComponent<TextMeshProUGUI>();
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        TMP.SetText("Nectar Essence: " + gameManager.resource);
    }
}
