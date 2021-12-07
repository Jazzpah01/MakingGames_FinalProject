using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [HideInInspector]
    public Transform target;

    public Vector3 offset;
    public float zoomSpeed = 4f;
    public float minZoom = 5f, maxZoom = 10f;
    public float startZoom;

    private float currentZoom = 10f;

    HUD hud;

    private void Start()
    {
        currentZoom = startZoom;
        target = PlayerManager.instance.player.transform;
        hud = GameManager.instance.hud;
    }

    void LateUpdate()
    {
        transform.position = target.position - offset * currentZoom;
    }

    void Update()
    {
        if (!hud.mouseOver)
        {
            currentZoom -= Input.GetAxis("Mouse ScrollWheel")*zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom,minZoom,maxZoom);
        }
    }
}
