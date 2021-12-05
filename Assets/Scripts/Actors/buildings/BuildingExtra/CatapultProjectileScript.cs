using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultProjectileScript : MonoBehaviour
{
    public GameObject Target;   
    public CollisionObserver detectionCollision;
    public CollisionObserver damagerCollision;
    public List<GameObject> EnemiesInAoE;
    private float damage;

    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        detectionCollision.Subscribe(Detection_Enter, CollisionObserver.CollisionType.Enter);
        detectionCollision.Subscribe(Detection_Exit, CollisionObserver.CollisionType.Exit);

        damagerCollision.Subscribe(Detection_EnterDamageZone, CollisionObserver.CollisionType.Enter);
        damagerCollision.Subscribe(Detection_ExitDamageZone, CollisionObserver.CollisionType.Exit);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Target.gameObject == null)
        {
            Destroy(this.gameObject);
        } else {
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, speed * Time.fixedDeltaTime);
        }
    }

    private void Detection_Enter(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;

        if(actor.isActorType(ActorType.Enemy) && actor.gameObject == Target.gameObject)
        {
            foreach(GameObject obj in EnemiesInAoE)
            {
                obj.GetComponent<IActor>().Health -= damage;
            }
            Destroy(this.gameObject);
        }
    }

    private void Detection_Exit(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;
    }

    private void Detection_EnterDamageZone(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;

        if(actor.isActorType(ActorType.Enemy))
        {
            EnemiesInAoE.Add(other.gameObject);
        }
    }

    private void Detection_ExitDamageZone(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;

        if (actor.isActorType(ActorType.Enemy))
        {
            EnemiesInAoE.Remove(other.gameObject);
        }
    }

    public void setValues(float d, float s)
    {
        damage = d;
        speed = s;
    }
    public void setTarget(GameObject t)
    {
        Target = t;
    }
}
