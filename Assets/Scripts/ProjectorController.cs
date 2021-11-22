using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorController : MonoBehaviour
{
    public float size;
    public bool lockPosition;

    private LayerMask ignoreLayer;
    private Projector proj;
    private Material mat;

    private void Start()
    {
        proj = GetComponent<Projector>();
        mat = GetComponent<Material>();
        size = proj.orthographicSize;
        ignoreLayer = proj.ignoreLayers;
    }
}
