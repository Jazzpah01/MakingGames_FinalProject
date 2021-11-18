using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscriptionOnMouseOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        Debug.Log("hovering over" + this.gameObject.name);
    }
}
