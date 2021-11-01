using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterSelectDummyController : MonoBehaviour
{
    public float lightIntensityHigh, lightIntensityLow;
    public Light light;


    private Animator animator;
    private Shader[] materials;
    private bool selected,mouseOff = true;
    private Vector3 startPos;

    void Start()
    {
        animator = GetComponent<Animator>();
        light = GetComponentInChildren<Light>();
        startPos = transform.position;
        animator.SetBool("isIdle",true);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)&& mouseOff)
        {
            animator.SetBool("isIdle", true);
            selected = false;
            light.intensity = lightIntensityLow;
        }
        if ((transform.position == startPos + new Vector3(0.1f,0.1f,0.1f)) || (transform.position == startPos - new Vector3(0.1f, 0.1f, 0.1f)))
        {
            //StartCoroutine(MoveBack());
        }

    }

    private void OnMouseEnter()
    {
        mouseOff = false;
        light.intensity = lightIntensityHigh;
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("isIdle", false);
            selected = true;
            //StartCoroutine(MoveBack());
        }
    }

    private void OnMouseExit()
    {
        mouseOff = true;
        if (!selected)
        {
            light.intensity = lightIntensityLow;
        }
    }

    IEnumerator MoveBack()
    {
        Debug.Log("moving");
        float x, y, z;
        bool good = true;
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;
        if(transform.position.x > startPos.x+0.01f)
        {
            x -= 0.001f;
            good = false;
        }
        else if (transform.position.x < startPos.x-0.01f)
        {
            x += 0.001f;
            good = false;
        }
        if (transform.position.y > startPos.y + 0.01f)
        {
            y -= 0.001f;
            good = false;
        }
        else if (transform.position.y < startPos.y - 0.01f)
        {
            y += 0.001f;
            good = false;
        }
        if (transform.position.z > startPos.z + 0.01f)
        {
            z -= 0.001f;
            good = false;
        }
        else if (transform.position.z < startPos.z - 0.01f)
        {
            z += 0.001f;
            good = false;
        }
        transform.position = new Vector3(x,y,z);
        yield return new WaitForSeconds(0.01f);
        if (!good)
        {
            StartCoroutine(MoveBack());
        }
    }
}
