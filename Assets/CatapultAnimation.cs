using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultAnimation : MonoBehaviour
{
    public GameObject parent;
    private bool shootReady;
    
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
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
        if(parent.GetComponent<Catapult>().getEnemies().Count > 0)
        {
            parent.GetComponent<Catapult>().shoot();
            shootReady = false;
        }
    }

    public void readyShot()
    {
        shootReady = true;
    }
}
