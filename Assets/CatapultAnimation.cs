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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void testShoot()
    {
        if(parent.GetComponent<Catapult>().getEnemies().Count > 0)
        {
            parent.GetComponent<Catapult>().shoot();
        }
    }

    public void pauseAnimation()
    {
       animator.speed = 0;
    }
}
