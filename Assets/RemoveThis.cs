using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveThis : MonoBehaviour
{
    public float timeToRemove = 0.5f;

    void Update()
    {
        timeToRemove -= Time.deltaTime;

        if (timeToRemove <= 0)
        {
            Destroy(this);
        }
    }
}