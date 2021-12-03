using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDebugger : MonoBehaviour
{
    [Range(1, 120)]
    public int targetFrameRate = 60;

    private void Start()
    {
        Application.targetFrameRate = targetFrameRate;
    }
}