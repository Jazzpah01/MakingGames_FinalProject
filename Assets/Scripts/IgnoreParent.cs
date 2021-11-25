using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreParent : MonoBehaviour
{
    [Header("Options")]
    public bool ignoreParentPosition = false;
    public bool ignoreParentRotation = false;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        ResetTransformValues();
    }

    public void ResetTransformValues()
    {
        if (ignoreParentPosition)
            originalPosition = transform.position;
        if (ignoreParentRotation)
            originalRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        if (ignoreParentPosition)
            transform.position = originalPosition;

        if (ignoreParentRotation)
            transform.rotation = originalRotation;
    }
}