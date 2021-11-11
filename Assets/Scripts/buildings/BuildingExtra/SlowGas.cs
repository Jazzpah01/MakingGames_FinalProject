using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowGas : MonoBehaviour
{
    double StartPos;
    public CollisionObserver detectionCollision;
    public CollisionObserver slowCollision;

    public float lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position.x;
        detectionCollision.Subscribe(Detection_Enter, CollisionObserver.CollisionType.Enter);
        detectionCollision.Subscribe(Detection_Exit, CollisionObserver.CollisionType.Exit);

        Destroy(this.gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Detection_Enter(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;

        switch (actor.type)
        {
            case ActorType.Enemy:
                actor.Speed = actor.Speed/2;
                break;
        }
    }

    private void Detection_Exit(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;


        switch (actor.type)
        {
            case ActorType.Enemy:
                actor.Speed = actor.Speed*2;
                break;
        }
    }

        private void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator SmoothSliderDecrease(float amount, Image image)
    {
        if (image != null)
            while (image.fillAmount > amount)
        {
            if(image != null)
            image.fillAmount -= 0.01f;

            yield return new WaitForSeconds(0.000001f);
        }
    }
}
