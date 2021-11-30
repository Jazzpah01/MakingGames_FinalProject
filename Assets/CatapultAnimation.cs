using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultAnimation : MonoBehaviour
{
    private GameObject parent;
    private bool shootReady;
    
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
        animator = transform.GetComponent<Animator>();
        shootReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void testShoot()
    {
        //Debug.Log("Shoot called from animation");
        parent.GetComponent<Catapult>().shoot();
        shootReady = false;
    }

    public void readyShot()
    {
        shootReady = true;
    }
}
