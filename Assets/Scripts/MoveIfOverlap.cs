using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveIfOverlap : MonoBehaviour
{

    private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.tag.Equals("Environment"))
        {
            Vector3 v;
            v = collisionInfo.transform.position - transform.position;
            transform.position -= v;
        }
    }
}
