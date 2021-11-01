using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeSpan = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.root.name == "Enemy")
        {
            transform.parent = collision.transform.root;
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.GetComponent<Rigidbody>().isKinematic = true;
            Destroy(gameObject, lifeSpan);
        }
        if (collision.transform.root.name == "Environment")
        {
            transform.parent = collision.transform.parent;
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.GetComponent<Rigidbody>().isKinematic = true;
            Destroy(gameObject, lifeSpan);
        }
    }
}
