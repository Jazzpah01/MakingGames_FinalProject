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
    public float speedModifyer = 1;

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

        if (actor.isActorType(ActorType.Enemy))
        {
            actor.speedModifyer *= speedModifyer;
            //Utility.DelayedAbility(0.5f, delegate
            //{
            //    actor.speedModifyer /= speedModifyer;
            //});
        }
    }

    private void Detection_Exit(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;

        if (actor.isActorType(ActorType.Enemy))
        {
            actor.speedModifyer /= speedModifyer;
        }
    }

    public void setValues(float speedModifyer, float lifeTime)
    {
        this.speedModifyer = speedModifyer;
        this.lifeTime = lifeTime;
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
