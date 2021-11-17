using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorController : MonoBehaviour
{
    private float size;
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

    private void Update()
    {
        
    }

}
