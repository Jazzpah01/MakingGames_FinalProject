using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceTextScript : MonoBehaviour
{
    TextMeshProUGUI TMP;
    // Start is called before the first frame update
    void Start()
    {
        TMP = this.gameObject.GetComponent<TextMeshProUGUI>();   
    }

    // Update is called once per frame
    void Update()
    {
        TMP.SetText("Nectar Essence: " + GameManager.instance.resource);
    }
}
