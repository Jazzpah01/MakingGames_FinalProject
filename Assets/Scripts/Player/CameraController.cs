using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [HideInInspector]
    public Transform target;
    public Transform basePos;
    public GameObject spawnpointParent;

    public Vector3 offset;
    public float zoomSpeed = 4f;
    public float minZoom = 5f, maxZoom = 10f;
    public float startZoom;

    private float currentZoom = 10f;
    private bool intro;

    private void Start()
    {
        currentZoom = startZoom;
        target = PlayerManager.instance.player.transform;

    }

    void LateUpdate()
    {
        if (!intro)
        {
            transform.position = target.position - offset * currentZoom;
        }
    }

    public void StartCameraIntro()
    {
        intro = true;
        target = basePos;
        StartCoroutine(ZoomOutIntro());
    }

    private  IEnumerator ZoomOutIntro()
    {
        while (true)
        {
            currentZoom+=0.1f;
            transform.position = target.position - offset * currentZoom;
            if (currentZoom >= maxZoom)
            {
                currentZoom = maxZoom;
                break;
            }
            yield return new WaitForSeconds(0.001f);
        }
        StartCoroutine(HiglightSpawnIntro());
    }

    private IEnumerator HiglightSpawnIntro()
    {
        
        float n = 1;
        float m = 1;
        while (true)
        {
            n += 0.01f * m;
            if (n >= 5)
            {
                m*=-1;
            }
            if (n<= 1)
            {
                n = 1;
                for (int i = 0; i < spawnpointParent.transform.childCount; i++)
                {
                    GameObject go = spawnpointParent.transform.GetChild(i).gameObject;
                    go.transform.localScale = new Vector3(n, n, n);
                }
                break;
            }
            for(int i = 0; i < spawnpointParent.transform.childCount; i++)
            {
                GameObject go = spawnpointParent.transform.GetChild(i).gameObject;
                go.transform.localScale = new Vector3(n,n,n);
            }

            yield return new WaitForSeconds(0.001f);
        }
            StartCoroutine(ZoomInPlayerIntro());
    }

    private IEnumerator ZoomInPlayerIntro()
    {
        target = PlayerManager.instance.player.transform;

        while (true)
        {
            currentZoom-=0.1f;
            transform.position = target.position - offset * currentZoom;
            if (currentZoom <= startZoom)
            {
                currentZoom = startZoom;
                break;
            }
            yield return new WaitForSeconds(0.001f);
        }
        intro = false;
    }

    private void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel")*zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom,minZoom,maxZoom);
    }
}
